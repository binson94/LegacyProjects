//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class ReadyManager : MonoBehaviour
//{
//    //Scene 내 관리
//    public GameScene ingameM;
//    public GameObject readyCanvas;

//    //Stage 정보
//    //***************************
//    public Text stageNameText;
//    public Text timeText;

//    public Sprite[] enemySprite;
//    public Image[] pivot5;
//    public Image[] pivot4;
//    //***************************
    
//    int time;

//    private void Start()
//    {
//        StartCoroutine(Timer());
//    }

//    public void ImageUpdate()
//    {
//        int count;
//        for (count = 0; ingameM.stageInfo[3 * count + 2] != 0; count++) ;

//        if (count % 2 == 0)
//            for (int i = 0; i < count; i++)
//            {
//                pivot4[i].sprite = enemySprite[ingameM.stageInfo[3 * i + 2] - 1];
//                pivot4[i].gameObject.SetActive(true);
//            }
//        else
//            for (int i = 0; i < count; i++)
//            {
//                pivot5[i].sprite = enemySprite[ingameM.stageInfo[3 * i + 2] - 1];
//                pivot5[i].gameObject.SetActive(true);
//            }
//    }

//    //idx : 0 ~ 9
//    public void EquipJem(int idx)
//    {
//        /*
//        if (equipGems[0] == null)
//            equipGems[0] = GameManager.instance.gems[idx];
//        else if (equipGems[1] == null)
//            equipGems[1] = GameManager.instance.gems[idx];
//        else if (equipGems[2] == null)
//            equipGems[2] = GameManager.instance.gems[idx];
//        else
//        {
//            equipGems[0] = equipGems[1];
//            equipGems[1] = equipGems[2];
//            equipGems[2] = GameManager.instance.gems[idx];
//        }
//        */
//    }

//    //idx : 0 ~ 2
//    public void UnEquipJem(int idx)
//    {
//        /*
//        for (int i = idx; i < 2; i++)
//            equipGems[i] = equipGems[i + 1];

//        equipGems[2] = null;
//        */
//    }

//    IEnumerator Timer()
//    {
//        time = 15;

//        while(time > 0)
//        {
//            time--;
//            timeText.text = time.ToString();
//            yield return new WaitForSeconds(1);
//        }

//        ingameM.StageStart();
//        readyCanvas.SetActive(false);
//    }

//    //현재 타이머에 관계 없이 바로 게임 시작
//    public void Btn_Start()
//    {
//        time = 0;
//    }
//}