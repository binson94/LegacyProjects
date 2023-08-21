using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TitleExit : UI_PopUp
{
    enum Buttons
    {
        QuitBtn,
        CloseBtn,
    }
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.QuitBtn).gameObject, Btn_Quit);
        BindEvent(GetButton((int)Buttons.CloseBtn).gameObject, Btn_Close);
    }

    void Btn_Quit(PointerEventData evt)
    {
        Application.Quit();
    }

    void Btn_Close(PointerEventData evt)
    {
        ClosePopUpUI();
    }


    public override void ReOpen()
    {

    }
}
