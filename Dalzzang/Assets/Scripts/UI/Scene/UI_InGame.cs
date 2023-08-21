using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : UI_Scene
{
    enum GameObjects
    {
        InGamePanel,
    }
    enum Sliders
    {
        HPBar,
    }

    enum Texts
    {
        TimerTxt,
        StageNameTxt,
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Slider>(typeof(Sliders));
        Bind<TMP_Text>(typeof(Texts));
    }

    public GameObject GetPanel() => GetGameObject((int)GameObjects.InGamePanel);
    public Slider GetHPBar() => Get<Slider>((int)Sliders.HPBar);
    public TMP_Text GetTimer() => GetText((int)Texts.TimerTxt);
    public void StageNameUpdate(string stage) => GetText((int)Texts.StageNameTxt).text = stage;
}
