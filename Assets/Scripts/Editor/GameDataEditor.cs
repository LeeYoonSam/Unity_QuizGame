using System.Collections;
using UnityEditor;
using UnityEngine;
using System.IO;


public class GameDataEditor : EditorWindow
{
    public GameData gameData;
    private string gameDataProjectFilePath = "/StreamingAssests/data.json";

    // 유니티 Window메뉴 추가하기
    [MenuItem("Window/Game Data Editor")]
    static void Init()
    {
        GameDataEditor Window = (GameDataEditor)EditorWindow.GetWindow(typeof(GameDataEditor));
        Window.Show();
    }

    void OnGUI()
    {
        if (gameData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("gameData");

            EditorGUILayout.PropertyField(serializedProperty, true);

            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save Data"))
            {
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load Data"))
        {
            LoadGameData();
        }
    }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            gameData = new GameData();
        }
    }

    private void SaveGameData()
    {
        string dataAsJson = JsonUtility.ToJson(gameData);
        string filePath = Application.dataPath + gameDataProjectFilePath;

        File.WriteAllText(filePath, dataAsJson);
    }

}
