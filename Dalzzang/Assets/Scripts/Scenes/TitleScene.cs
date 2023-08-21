using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.UI.ShowSceneUI<UI_Title>();
    }
}
