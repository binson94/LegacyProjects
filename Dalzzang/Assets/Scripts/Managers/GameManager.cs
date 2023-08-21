using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance 
    { 
        get
        {
            if (instance == null)
                Init();
            return instance;
        }
    }

    JsonManager _jsonManager = new JsonManager();
    SaveManager _saveManager = new SaveManager();
    UIManager _uiManager = new UIManager();
    ResourceManager _resourceManager = new ResourceManager();
    public static JsonManager Json { get => Instance._jsonManager; }
    public static SaveManager Save { get => Instance._saveManager; }
    public static UIManager UI { get => Instance._uiManager; }
    public static ResourceManager Resource { get => Instance._resourceManager; }

    static void Init()
    {
        GameObject container = new GameObject() { name = "@GameManager" };
        instance = container.AddComponent<GameManager>();
        DontDestroyOnLoad(container);

        instance._jsonManager.Init();
        instance._saveManager.Init();
    }

    public void Clear()
    {
        _uiManager.Clear();
    }
}