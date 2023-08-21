using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterScene : MonoBehaviour
{
    //중복된 스테이지 선택 방지
    bool[] selected = new bool[6];

    //Moon Stat 정보 표시
    //*****************************
    [SerializeField] Text lvlTxt;
    [SerializeField] Text dmgTxt;
    [SerializeField] Text spdTxt;
    //*****************************

    public Stage[] stages;

    private void Start()
    {
        GenerateStage();

        lvlTxt.text = $"{GameManager.Save.Lvl}";
        dmgTxt.text = $"{GameManager.Save.Dmg}";
        spdTxt.text = string.Format("{0:F}", GameManager.Save.AtkSpd);

        for (int i = 0; i < 3; i++)
            stages[i].ImageUpdate();
    }

    //다음 스테이지 생성
    void GenerateStage()
    {
        int pivot = GameManager.Save.StagePivot;

        for (int i = 0; i < 6; i++)
            selected[i] = false;

        for (int i = 0; i < 3;)
        {
            int rand = Random.Range(0, 100);

            if (rand < 25)
            {
                if(!selected[0])
                {
                    selected[0] = true;
                    stages[i++].stageIdx = pivot;
                }
            }
            else if (rand < 45)
            {
                if (!selected[1])
                {
                    selected[1] = true;
                    stages[i++].stageIdx = pivot + 1;
                }
            }
            else if (rand < 60)
            {
                if (!selected[2])
                {
                    selected[2] = true;
                    stages[i++].stageIdx = pivot + 2;
                }
            }
            else if (rand < 75)
            {
                if (!selected[3])
                {
                    selected[3] = true;
                    stages[i++].stageIdx = pivot + 3;
                }
            }
            else if (rand < 90)
            {
                if (!selected[4])
                {
                    selected[4] = true;
                    stages[i++].stageIdx = pivot + 4;
                }
            }
            else
            {
                if (!selected[5])
                {
                    selected[5] = true;
                    stages[i++].stageIdx = pivot + 5;
                }
            }
        }
    }

    //다음 스테이지 선택
    public void Btn_nextStage(int idx)
    {
        GameManager.Save.NextStage(stages[idx].stageIdx);
        SceneLoader.LoadScene(Define.Scenes.Game);
    }
    //타이틀로 돌아가기
    public void Btn_Title()
    {
        SceneLoader.LoadScene(Define.Scenes.Title);
    }
}
