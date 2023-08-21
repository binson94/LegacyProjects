using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Title : UI_Scene
{
    enum Buttons
    { 
        StartBtn,
        OptionBtn,
        ExitBtn
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        BindEvent(GetButton((int)Buttons.StartBtn).gameObject, Btn_Start);
        BindEvent(GetButton((int)Buttons.OptionBtn).gameObject, Btn_Option);
        BindEvent(GetButton((int)Buttons.ExitBtn).gameObject, Btn_Exit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Btn_Exit(null);
    }

    void Btn_Start(PointerEventData evt)
    {
        SceneLoader.LoadScene(Define.Scenes.Game);
    }

    void Btn_Option(PointerEventData evt)
    {
        Debug.Log("option");
    }

    void Btn_Exit(PointerEventData evt)
    {
        GameManager.UI.ShowPopUpUI<UI_TitleExit>();
    }
}
