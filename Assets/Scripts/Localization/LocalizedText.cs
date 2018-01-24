﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string key;

    void Start()
    {
        Text cpntText = GetComponent<Text>();
        cpntText.text = LocalizationManager.instance.GetLocalizedValue(key);
    }
}
