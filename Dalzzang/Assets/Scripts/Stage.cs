using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    public int stageIdx;
    StageInfo stageInfo;

    [SerializeField] Text stageNameTxt;
    /// <summary>
    /// 이미 알려진 맵 -> 몬스터 표시
    /// </summary>
    [SerializeField] Image[] enemyImages;
    /// <summary>
    /// 모르는 맵 -> 물음표 표시
    /// </summary>
    [SerializeField] GameObject[] questionMarks;

    public void ImageUpdate()
    {
        stageInfo = GameManager.Json.GetStageInfo(stageIdx);

        //이미 방문한 적 있는 스테이지 -> 적 정보 표시
        if (GameManager.Save.Visited(stageIdx))
        {
            stageNameTxt.text = stageInfo.Name;
            for (int i = 0; i < stageInfo.EnemyCount; i++) 
            { 
                enemyImages[i].sprite = GameManager.Resource.Load<Sprite>($"Sprites/Enemies/e{stageInfo.EnemyIdx[i]}");
                enemyImages[i].gameObject.SetActive(true);
            }

        }
        //방문한 적 없음 -> ?로 표시
        else
        {
            stageNameTxt.text = "???";
            for (int i = 0; i < stageInfo.EnemyCount; i++)
                questionMarks[i].SetActive(true);
        }

    }
}
