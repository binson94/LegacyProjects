using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currStage;
    public int clearCnt;
    public int stagePivot;
    public bool[] visited = new bool[33];

    public int lvl;
    public int dmg;
    public float atkSpd;
    public int atkPen;

    public int[] gemCnt = new int[10];

    public GameData()
    {
        lvl = 1;
        dmg = 12;
        atkSpd = 2;
        atkPen = 0;
    }
}

public class SaveManager
{
    GameData gameData;

    public void Init()
    {
        if (PlayerPrefs.HasKey("GameData"))
            gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("GameData"));
        else
            gameData = new GameData();
    }

    public int CurrStage { get => gameData.currStage; }
    public int Lvl { get => gameData.lvl; }
    public int Dmg { get => gameData.dmg; }
    public float AtkSpd {  get => gameData.atkSpd; }
    public int AtkPen { get => gameData.atkPen; }
    public int StagePivot { get => gameData.stagePivot; }
    
    public bool Visited(int stageIdx) => gameData.visited[stageIdx];

    /// <summary>
    /// ���� ���������� �Ѿ ��, ���� ���� ����
    /// </summary>
    /// <param name="stageIdx">�Ѿ�� ��������</param>
    public void NextStage(int stageIdx)
    {
        gameData.currStage = stageIdx;
        gameData.visited[stageIdx] = true;

        SaveData();
    }

    /// <summary>
    /// ���� ȹ�� ���� ����
    /// </summary>
    /// <param name="rewardIdx">ȹ�� ���� idx</param>
    public void GetReward(Define.Reward reward)
    {
        if ((int)reward > 0)
        {
            if ((int)reward < 11)
            {
                gameData.gemCnt[(int)reward - 1]++;
            }
            else if ((int)reward == 11)
                gameData.dmg += 2;
            else
                gameData.atkSpd += 0.2f;
        }

        SaveData();
    }

    /// <summary>
    /// ���� �������� �������� ���� ���ذ� ���, �������� Ŭ���� �� ȣ��
    /// </summary>
    public void IncreaseStagePivot()
    {
        gameData.clearCnt++;
        if (gameData.clearCnt > 2 && gameData.stagePivot < 23)
        {
            gameData.clearCnt = 0;
            gameData.stagePivot++;
        }

        SaveData();
    }

    public void SaveData() => PlayerPrefs.SetString("GameData", JsonUtility.ToJson(gameData));
}
