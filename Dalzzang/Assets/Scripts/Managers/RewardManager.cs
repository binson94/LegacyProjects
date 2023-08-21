//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//public class RewardManager : MonoBehaviour
//{
//    //보상 로드를 위한 이름과 로드된 각 확률
//    //*******************************
//    public string[] rewardName = new string[13];
//    public int[] rewardProbability = new int[13];
//    //*******************************

//    //미리 할당된 스프라이트들, 생성된 보상에 따라 이 이미지를 적용
//    //*******************************
//    public Sprite[] rewardSprites;
//    //*******************************

//    //생성된 보상과 그에 따른 이미지와 텍스트
//    //*******************************
//    public int[] rewardIdx = new int[3];
//    public Image[] rewardImages;
//    public Text[] rewardTexts;

//    public Text ATKText;
//    public Text SPDText;
//    public GameObject ATKPlusText;
//    public GameObject SPDPlusText;
//    //*******************************

//    //보상 확률 이름 생성
//    public void RewardInit(int time)
//    {
//        rewardName[0] = "bomb";

//        for (int i = 1; i < 11; i++)
//            rewardName[i] = string.Concat("gem", i);
//        rewardName[11] = "PowerUp";
//        rewardName[12] = "SpeedUp";

//        RewardLoad(time);
//    }

//    //보상 확률 로드
//    private void RewardLoad(int time)
//    {
//        TextAsset txtAsset = Resources.Load<TextAsset>("Reward");
//        string loadStr = txtAsset.text;
//        JsonData json = JsonMapper.ToObject(loadStr);

//        int idx = 0;

//        if (time == 40)
//            idx = 1;
//        else if (time == 50)
//            idx = 2;
//        else if (time == 60)
//            idx = 3;
//        else if (time == 90)
//            idx = 4;
//        else if (time == 120)
//            idx = 5;

//        //Stage Data Load
//        for (int i = 0; i < 13; i++)
//        {
//            rewardProbability[i] = int.Parse(json[idx][rewardName[i]].ToString());
//        }
//    }

//    //보상 3개 생성
//    public void RewardGeneration()
//    {
//        ATKText.text = GameManager.gameData.dmg.ToString();
//        SPDText.text = string.Format("{0:F}", GameManager.gameData.atkSpd);

//        int bomb = 0;
//        bool[] visited = new bool[12];
//        for (int i = 0; i < 3;)
//        {
//            int rand = Random.Range(0, 100);
//            int total = rewardProbability[0];

//            //꽝
//            if (rand < total)
//            {
//                if (bomb < 2)
//                {
//                    bomb++;
//                    rewardIdx[i++] = 0;
//                }
//            }
//            else
//            {
//                for (int j = 1; j < 13; j++)
//                {
//                    total += rewardProbability[j];
//                    if (rand < total)
//                    {
//                        if (!visited[j - 1])
//                        {
//                            visited[j - 1] = true;
//                            rewardIdx[i++] = j;
//                            break;
//                        }
//                    }
//                }
//            }
//        }

//        for (int i = 0; i < 3; i++)
//        {
//            //꽝인 경우 Image 비활성화
//            if (rewardIdx[i] == 0)
//            {
//                rewardImages[i].gameObject.SetActive(false);
//                rewardTexts[i].gameObject.SetActive(false);
//            }
//            //보석 보상
//            else if (rewardIdx[i] < 11)
//            {
//                rewardImages[i].sprite = rewardSprites[rewardIdx[i] - 1];

//                int tmp = GameManager.gameData.gemCnt[rewardIdx[i] - 1];
//                rewardTexts[i].text = string.Concat(tmp, " -> ", tmp + 1);
//            }
//            //능력치 보상
//            else
//            {
//                rewardImages[i].sprite = rewardSprites[rewardIdx[i] - 1];

//                if (rewardIdx[i] == 11)
//                    rewardTexts[i].text = "ATK +2";
//                else
//                    rewardTexts[i].text = "SPD +0.2";
//            }
//        }
//    }

//    //
//    public void Btn_RewardPointEnter(int idx)
//    {
//        if (rewardIdx[idx] < 11)
//            return;

//        if (rewardIdx[idx] == 11)
//            ATKPlusText.SetActive(true);
//        else
//            SPDPlusText.SetActive(true);
//    }
    
//    public void Btn_RewardSelect(int idx)
//    {
//        GameManager.GetReward(rewardIdx[idx]);
//        GameManager.IncreaseStagePivot();

//        SceneManager.LoadScene(2);
//    }
    
//    public void Btn_RewardPointExit(int idx)
//    {
//        if (rewardIdx[idx] < 11)
//            return;

//        if (rewardIdx[idx] == 11)
//            ATKPlusText.SetActive(false);
//        else
//            SPDPlusText.SetActive(false);
//    }
//}
