using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Reward : UI_PopUp
{
    enum Images
    {
        RewardImage1,
        RewardImage2,
        RewardImage3,
    }

    enum Buttons
    { 
        RewardBtn1,
        RewardBtn2,
        RewardBtn3,
    }

    enum Texts
    {
        RewardTxt1,
        RewardTxt2,
        RewardTxt3,
        AtkTxt,
        SpdTxt,
    }

    List<Define.Reward> _rewards = new List<Define.Reward>();

    public override void Init()
    {
        base.Init();
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));

        BindButtonEvent();

        GetText((int)Texts.RewardTxt1).text = "aa";
    }

    void BindButtonEvent()
    {
        GameObject btn = GetButton((int)Buttons.RewardBtn1).gameObject;
        BindEvent(btn, SelectReward, 0);
        BindPointEvent(btn, PointerEnter, 0);
        BindPointEvent(btn, PointerExit, 0, Define.PointEvent.Exit);

        btn = GetButton((int)Buttons.RewardBtn2).gameObject;
        BindEvent(btn, SelectReward, 1);
        BindPointEvent(btn, PointerEnter, 1);
        BindPointEvent(btn, PointerExit, 1, Define.PointEvent.Exit);

        btn = GetButton((int)Buttons.RewardBtn3).gameObject;
        BindEvent(btn, SelectReward, 2);
        BindPointEvent(btn, PointerEnter, 2);
        BindPointEvent(btn, PointerExit, 2, Define.PointEvent.Exit);
    }

    void SelectReward(PointerEventData evt, int idx)
    {
        Debug.Log($"select {_rewards[idx]}");
        if (_rewards[idx] == Define.Reward.Miss && _rewards.Any(x => x != Define.Reward.Miss))
            return;

        GameManager.Save.GetReward(_rewards[idx]);
        SceneLoader.LoadScene(Define.Scenes.After);
    }

    void PointerEnter(PointerEventData evt, int idx)
    {
        Debug.Log($"enter {_rewards[idx]}");
    }

    void PointerExit(PointerEventData evt, int idx)
    {
        Debug.Log($"exit {idx}");
    }

    public void LoadReward(int time)
    {
        RewardInfo rewardInfo = GameManager.Json.GetRewardInfo(time);

        for(int i = 0;i < 3;i++)
        {
            int rand = UnityEngine.Random.Range(0, 100);
            int total = 0;

            Define.Reward reward;
            for (reward = Define.Reward.Miss; reward < Define.Reward.Speed; reward++)
            {
                total += rewardInfo.probs[(int)reward];
                if(rand < total)
                    break;
            }
            _rewards.Add(reward);
        }

        _rewards.Sort();

        SetImages();
    }

    void SetImages()
    {
        for(int i = 0;i < 3;i++)
        {
            string imageName = $"RewardImage{i + 1}";
            Images imageEnum = (Images)Enum.Parse(typeof(Images), imageName);
            Image image = GetImage((int)imageEnum);

            if (_rewards[i] == Define.Reward.Miss)
                image.gameObject.SetActive(false);
            else
            {
                image.sprite = GameManager.Resource.Load<Sprite>($"Sprites/Rewards/{_rewards[i]}");

            }
                
        }
    }

    public override void ReOpen()
    {

    }
}
