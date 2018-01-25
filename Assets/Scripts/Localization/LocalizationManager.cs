using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;

    private Dictionary<string, string> localizedText;

    private PlayerProgress playerProgress;
    // 퀴즈 데이터
    private RoundData[] allRoundData;

    private bool isReady = false;
    private string missingTextString = "Localized text not found";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        LoadPlayerProgress();

        DontDestroyOnLoad(gameObject);
    }

    public void LoadLocalizedText(string fileName)
    {
        localizedText = new Dictionary<string, string>();
        // string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string filePath = Path.Combine(Application.dataPath, "StreamingAssests/" + fileName);
        Debug.Log("Flag Click - FilePath: " + filePath);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    // langType을 받아서 언어별 데이터 생성
    public void LoadLocalizedType(int langType)
    {
        // Default - English
        string menuFilePath = Path.Combine(Application.dataPath, "StreamingAssests/localizedText_en.json");
        string quizFilePath = Path.Combine(Application.dataPath, "StreamingAssests/quizText_en.json");

        switch (langType)
        {
            case 1:
                menuFilePath = Path.Combine(Application.dataPath, "StreamingAssests/localizedText_de.json");
                quizFilePath = Path.Combine(Application.dataPath, "StreamingAssests/quizText_de.json");
                break;

            case 2:
                break;
        }
        localizedText = new Dictionary<string, string>();

        // 언어별 메뉴 가져오기
        if (File.Exists(menuFilePath))
        {
            string dataAsJson = File.ReadAllText(menuFilePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        // 언어별 퀴즈 가져오기
        if (File.Exists(quizFilePath))
        {
            string dataAsJson = File.ReadAllText(quizFilePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            allRoundData = loadedData.allRoundData;
            Debug.Log("Data loaded, allRoundData: " + allRoundData.Length + " datas");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }

        isReady = true;
    }

    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }

        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }

    public RoundData GetCurrentRoundData()
    {
        return allRoundData[0];
    }

    public void SubmitNewPlayerScore(int newScore)
    {
        if (newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }

    private void LoadPlayerProgress()
    {
        playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
    }

    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
    }
}
