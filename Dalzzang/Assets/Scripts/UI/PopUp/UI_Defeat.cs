using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Defeat : UI_PopUp
{
    enum Buttons
    { 
        GoToTitleBtn,
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.GoToTitleBtn).gameObject, Btn_GoToTitle);
    }

    void Btn_GoToTitle(PointerEventData evt)
    {
        SceneLoader.LoadScene(Define.Scenes.Title);
    }

    public override void ReOpen()
    {

    }
}
