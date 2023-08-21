using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GameScene : MonoBehaviour
{
    /// <summary>
    ///0 : basic,    1 : fast,       2 : AttackSpeed
    ///3 : Escape,   4 : Avoid,      5 : Defense
    ///6 : Divide,   7 : Invinsible, 8 : Defense++
    ///9 : Mars      10 : Jupiter,   11 : Saturn
    /// </summary>
    [Header("Prefab")]
    [SerializeField]
    Enemy enemyPrefab;

    /// <summary>
    /// stage 인덱스
    /// </summary>
    [Header("Stage Info")]
    int stageIdx;
    StageInfo stageInfo;
    /// <summary>
    /// 생성된 적
    /// </summary>
    List<Enemy> enemies = new List<Enemy>();
    /// <summary>
    /// 적 이동 위치 기준점
    /// </summary>
    [SerializeField] Transform[] pivots = new Transform[6];

    [Header("Communicate")]
    [SerializeField] Moon moon;

    /// <summary> timer text </summary>
    [Header("UI")]
    TMP_Text timerTxt;
    /// <summary> hp bar </summary>
    Slider hpBar;
    /// <summary> 게임 끝날 시 숨길 요소 </summary>
    GameObject ingamePanel;

    [HideInInspector] public bool win = false;
    [HideInInspector] public bool lose = false;

    private void Awake()
    {
        SetCameraRatio();
    }

    /// <summary> 화면 비율에 따라 카메라 비율 조정 </summary>
    void SetCameraRatio()
    {
        float pivotRatio = 1920f / 1080f;
        float currRatio = (float)Screen.height / Screen.width;

        if (currRatio < 1)
            return;

        float cameraRatio = currRatio / pivotRatio;

        Camera.main.orthographicSize = 5 * cameraRatio;
    }

    void Start()
    {
        LoadStageData();
        OpenIngameUI();
        EnemyInstantiate();

        StageStart();
    }

    /// <summary> 스테이지 정보 불러오기 </summary>
    void LoadStageData()
    {
        //gameManager에서 stage number 읽어오기
        stageIdx = GameManager.Save.CurrStage;
        stageInfo = GameManager.Json.GetStageInfo(stageIdx);
    }
    /// <summary> IngameUI 열기 </summary>
    void OpenIngameUI()
    {
        UI_InGame ui_InGame = GameManager.UI.ShowSceneUI<UI_InGame>();
        ui_InGame.StageNameUpdate(stageInfo.Name);
        ingamePanel = ui_InGame.GetPanel();
        hpBar = ui_InGame.GetHPBar();
        timerTxt = ui_InGame.GetTimer();
    }

    /// <summary> 읽은 Stage 정보를 통해 모든 적 생성 후 꺼두기 </summary>
    void EnemyInstantiate()
    {
        for (int i = 0; i < stageInfo.EnemyCount; i++)
        {
            int amount = Random.Range(stageInfo.EnemyMin[i], stageInfo.EnemyMax[i] + 1);

            for (int j = 0; j < amount; j++)
            {
                Enemy enemy = Instantiate(enemyPrefab);
                enemy.idx = stageInfo.EnemyIdx[i];
                enemy.Init(HPBarUpdate, pivots);
                enemy.gameObject.SetActive(false);
                enemies.Add(enemy);
            }
        }

        Shuffle();
    }

    #region Shuffle
    //Enemy Random Shuffle 용 - poker에서 인용
    //********************************
    private void Shuffle()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
                Shuffle_Type1();

            Shuffle_Type2();
        }
    }

    //힌두 셔플 방식
    private void Shuffle_Type1()
    {
        Queue<Enemy> tmp = new Queue<Enemy>();        //셔플 시 위치 바꿀 카드들 넣을 공간

        int j;
        int pivot = Random.Range(0, (int)(enemies.Count * 0.6f));
        int length = Random.Range((int)(enemies.Count * 0.1f), (int)(enemies.Count * 0.4f));      //셔플 시 이동할 카드 장 수

        //셔플할 카드들 tmp에 저장
        for (j = pivot; j < pivot + length; j++)
        {
            tmp.Enqueue(enemies[j]);
        }

        //셔플을 위해 빼낸 카드들 뒤에 있는 카드들 앞당기기
        for (j = pivot; j + length < enemies.Count; j++)
        {
            enemies[j] = enemies[j + length];
        }

        //tmp에 있던 카드들 뒤에 추가
        for (; j < enemies.Count; j++)
        {
            enemies[j] = tmp.Dequeue();
        }
    }

    //파로 셔플 방식
    private void Shuffle_Type2()
    {
        Queue<Enemy> tmp = new Queue<Enemy>();       //셔플 시 위치 바꿀 카드들 저장할 공간

        //덱 절반을 tmp에 넣기
        for (int i = enemies.Count / 2; i < enemies.Count; i++)
            tmp.Enqueue(enemies[i]);

        //남은 카드들 사이사이 빈 공간 만들기
        for (int i = enemies.Count / 2 - 1; i >= 0; i--)
        {
            enemies[i * 2] = enemies[i];
        }

        //tmp에 있는 카드들 사이사이 끼워넣기
        for (int i = 0; i < enemies.Count / 2; i++)
        {
            enemies[2 * i + 1] = tmp.Dequeue();
        }
    }
    #endregion

    public void StageStart()
    {
        StartCoroutine(EnemyAppear());
        StartCoroutine(Timer());
    }

    /// <summary> 차례대로 적 켜기 </summary>
    IEnumerator EnemyAppear()
    {
        int waitTime = (int)(stageInfo.Timeout * 0.6f / enemies.Count);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = pivots[0].position;
            enemies[i].gameObject.SetActive(true);
            enemies[i].InstanceStart();

            yield return new WaitForSeconds(waitTime);
        }

        StartCoroutine(WinCheck());
    }

    /// <summary> 타이머 </summary>
    IEnumerator Timer()
    {
        int time = stageInfo.Timeout;

        while(time > 0 && !win)
        {
            time--;
            timerTxt.text = time.ToString();
            yield return new WaitForSeconds(1);
        }

        if (!win)
            Defeat();
    }

    /// <summary> 승리 조건 검사 </summary>
    IEnumerator WinCheck()
    {
        while (!win && !lose)
        {
            win = !enemies.Any(x => x.isActiveAndEnabled);
            yield return new WaitForSeconds(0.5f);
        }

        if (win)
            Win();
    }

    /// <summary> 타이머 종료 시 패배 </summary>
    void Defeat()
    {
        lose = true;
        ingamePanel.SetActive(false);
        GameManager.UI.ShowPopUpUI<UI_Defeat>();

        moon.anim.SetInteger("State", 2);   //패배
    }

    /// <summary> 승리 </summary>
    void Win()
    {
        //보상 생성
        ingamePanel.SetActive(false);
        GameManager.UI.ShowPopUpUI<UI_Reward>().LoadReward(stageInfo.Timeout);

        moon.anim.SetInteger("State", 1);   //승리
    }


    void HPBarUpdate(float value)
    {
        hpBar.value = value;
    }
}
