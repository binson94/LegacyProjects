/******
�ۼ��� : ���� �ۼ�
�ۼ� ���� : 23.03.29

�ֱ� ���� ���� : 23.04.10
�ֱ� ���� ���� : �ּ� ����
******/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    Dictionary<string, GameObject> _loadedObjects = new Dictionary<string, GameObject>();
    Dictionary<string, Sprite> _loadedSprites = new Dictionary<string, Sprite>();

    GameObject LoadGameObject(string path)
    {
        GameObject go;
        if (_loadedObjects.TryGetValue(path, out go))
            return go;

        go = Resources.Load<GameObject>(path);
        _loadedObjects.Add(path, go);
        return go;
    }

    Sprite LoadSprite(string path)
    {
        Sprite sprite;
        if (_loadedSprites.TryGetValue(path, out sprite))
            return sprite;

        sprite = Resources.Load<Sprite>(path);
        _loadedSprites.Add(path, sprite);

        return sprite;
    }

    /// <summary> Resources.Load�� �ҷ����� </summary>
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
            return LoadGameObject(path) as T;
        else if (typeof(T) == typeof(Sprite))
            return LoadSprite(path) as T;

        return Resources.Load<T>(path);
    }

    /// <summary> GameObject ���� </summary>
    public GameObject Instantiate(string path, Transform parent = null) => Instantiate<GameObject>(path, parent);
    /// <summary> T type object ���� </summary>
    public T Instantiate<T>(string path, Transform parent = null) where T : UnityEngine.Object
    {
        T prefab = Load<T>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {path}");
            return null;
        }

        T instance = UnityEngine.Object.Instantiate<T>(prefab, parent);
        instance.name = prefab.name;

        return instance;
    }

    public void Clear()
    {
        _loadedObjects.Clear();
        _loadedSprites.Clear();
    }
}