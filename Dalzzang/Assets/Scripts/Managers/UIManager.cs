/*
�ۼ��� : �̿쿭
�ۼ� ���� : 23.03.29

�ֱ� ���� ���� : 23.04.05
�ֱ� ���� ���� : �˾� UI ���� �� destroy�� �ƴ϶� SetActive(false)�� ����
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    /// <summary>
    /// �˾� UI ������ ���� stack
    /// </summary>
    [Header("Pop Up")]
    Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();
    /// <summary>
    /// popup ui ���� ������ ���� ����
    /// </summary>
    int _order = 1;

    /// <summary>
    /// popup ������ ���� ĳ��
    /// </summary>
    Dictionary<System.Type, GameObject> _popupInstances = new Dictionary<System.Type, GameObject>();

    /// <summary>
    /// UI�� �θ� 
    /// </summary>
    [Header("Root")]
    GameObject _root = null;
    public GameObject Root
    {
        get
        {
            if (_root == null)
            {
                if ((_root = GameObject.Find("UIRoot")) == null)
                    _root = new GameObject { name = "UIRoot" };
            }
            return _root;
        }
    }

    /// <summary>
    /// game object�� canvas �Ӽ� �ο�, ���� ����
    /// </summary>
    /// <param name="go">canvas �Ӽ��� �ִ� ���� ������Ʈ</param>
    /// <param name="sort">canvas ���� ����(popup->true, scene->false)</param>
    public void SetCanvas(GameObject go, bool sort = true, int order = 0)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
            canvas.sortingOrder = _order++;
        else
            canvas.sortingOrder = order;
    }

    /// <summary>
    /// Scene �⺻ UI ����
    /// </summary>
    /// <typeparam name="T">UI_Scene�� ��ӹ��� �� Scene�� UI</typeparam>
    /// <param name="name">Scene UI �̸�, null�̸� T �̸�</param>
    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);

        go.transform.SetParent(Root.transform);
        sceneUI.Init();

        return sceneUI;
    }

    /// <summary>
    /// Pop Up UI ����
    /// </summary>
    /// <typeparam name="T">UI_PopUp�� ��ӹ��� Pop up UI</typeparam>
    /// <param name="name">Pop Up UI �̸�, null�̸� T �̸�</param>
    public T ShowPopUpUI<T>(string name = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject popup;
        T popupUI;

        //������ ��� ��� ���� -> ����
        if (_popupInstances.TryGetValue(typeof(T), out popup) == false)
        {
            popup = GameManager.Resource.Instantiate($"UI/PopUp/{name}");
            _popupInstances.Add(typeof(T), popup);

            popupUI = Util.GetOrAddComponent<T>(popup);
            popupUI.Init();
        }
        //������ ��� ��� ���� -> ��Ȱ��ȭ
        else
        {
            popupUI = Util.GetOrAddComponent<T>(popup);
            if (popupUI.isActiveAndEnabled)
                return null;

            popupUI.GetComponent<Canvas>().sortingOrder = _order++;
            popupUI.ReOpen();
        }

        _popupStack.Push(popupUI);

        popup.transform.SetParent(Root.transform);
        popup.SetActive(true);

        return popupUI;
    }

    /// <summary>
    /// Ư�� pop up UI �ݱ�, stack�� ���� ���� �ƴϸ� ���� X
    /// </summary>
    /// <param name="popup">�ݰ��� �ϴ� popup</param>
    public void ClosePopUpUI(UI_PopUp popup)
    {
        if (_popupStack.Count <= 0) return;

        if (_popupStack.Peek() != popup)
        {
            Debug.LogError("Pop Up doesn't match. Can't close pop up.");
            return;
        }

        ClosePopUpUI();
    }
    /// <summary>
    /// ���� ���� pop up UI �ݱ�
    /// </summary>
    public void ClosePopUpUI()
    {
        if (_popupStack.Count <= 0) return;

        UI_PopUp popup = _popupStack.Pop();
        popup.gameObject.SetActive(false);

        _order--;
    }

    /// <summary>
    /// ��� pop up UI �ݱ�
    /// </summary>
    public void CloseAllPopUpUI()
    {
        while (_popupStack.Count > 0)
            ClosePopUpUI();
    }

    /// <summary>
    /// UI �ʱ�ȭ
    /// </summary>
    public void Clear()
    {
        CloseAllPopUpUI();
        _popupInstances.Clear();
    }
}
