using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInfo
{
    public string Name;
    public int Timeout;
    public int Exp;
    public int EnemyCount;
    public int[] EnemyIdx = new int[5];
    public int[] EnemyMin = new int[5];
    public int[] EnemyMax = new int[5];
}

[System.Serializable]
public class StageInfoHandler
{ 
    public List<StageInfo> stageinfos = new List<StageInfo>();
}

[System.Serializable]
public class EnemyStat
{
    public string name;
    public int HP;
    public int Defense;
    public int MoveSpeed;
}

[System.Serializable]
public class EnemyStatHandler
{
    public List<EnemyStat> enemystats = new List<EnemyStat>();
}

[System.Serializable]
public class RewardInfo
{
    public List<int> probs = new List<int>();
}

[System.Serializable]
public class RewardInfoHandler
{
    public List<RewardInfo> rewardinfos = new List<RewardInfo>();
}


public class JsonManager
{
    StageInfoHandler stageData;
    EnemyStatHandler statData;
    RewardInfoHandler rewardData;

    public void Init()
    {
        stageData = Util.ParseJson<StageInfoHandler>();
        statData = Util.ParseJson<EnemyStatHandler>();
        rewardData = Util.ParseJson<RewardInfoHandler>();
    }

    public StageInfo GetStageInfo(int stageIdx) => stageData.stageinfos[stageIdx];
    public EnemyStat GetEnemyStat(int enemyIdx) => statData.enemystats[enemyIdx];
    public RewardInfo GetRewardInfo(int time)
    {
        int idx = 0;
        if(time == 30)
            idx = 1;
        else if (time == 40)
            idx = 2;
        else if (time == 50)
            idx = 3;
        else if (time == 60)
            idx = 4;
        else if (time == 90)
            idx = 5;
        else if (time == 120)
            idx = 6;

        return rewardData.rewardinfos[idx];
    }
}
