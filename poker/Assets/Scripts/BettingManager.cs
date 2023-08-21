using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeforeState
{
    None, Raise, Check
}

public class BettingManager : MonoBehaviour
{
    public static BettingManager instance = null;
    
    public int nowPlayer = 0;         //현재 배팅하는 플레이어
    public int PriorityPlayer = 0;    //우선권을 가진 플레이어

    public bool whoAllined;           //둘 중 한명이라도 올인하면 참

    public int moneyStack = 0; //1번 배팅에서 각 플레이어가 낼 돈

    BeforeState bState = BeforeState.None;

    public GameObject bettingCanvas;
    public GameObject allinCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartBetting()
    {
        for (int i = 0; i < 2; i++)
            GameManager.instance.Players[i].ShowStateUpdate();

        if (whoAllined)
        {
            GameManager.instance.NextCard(PriorityPlayer);
            return;
        }

        moneyStack = 0;
        bState = BeforeState.None;
        nowPlayer = PriorityPlayer;

        if (nowPlayer == 0)
        {
            bettingCanvas.SetActive(true);
        }
        else
        {
            StartCoroutine(AIBetting());
        }
    }

    //인공지능이 배팅
    private IEnumerator AIBetting()
    {
        yield return new WaitForSeconds(1f);

        if (GameManager.instance.Players[1].money < GameManager.instance.Players[0].money)
        {
            // CPU has less money than the player, so it will be more careful
            if (GameManager.instance.Players[0].ShowState > GameManager.instance.Players[1].ShowState)
            {
                int randomnr = Random.Range(0, 11);
                // 30% chance that CPU will bluff
                if (randomnr < 3)
                {
                    Raise(1);
                    KeepBetting();
                }
                else
                {
                    if(bState == BeforeState.None)
                    {
                        Check(1);
                        KeepBetting();
                    }
                    else
                    {
                        Call(1);
                    }
                }
            }
            else if (GameManager.instance.Players[0].ShowState == GameManager.instance.Players[1].ShowState)
            {
                int randomnr = Random.Range(0, 11);
                // 40% chance that CPU will raise on equal score
                if (randomnr < 4)
                {
                    Raise(1);
                    KeepBetting();
                }
                else
                {
                    if(bState == BeforeState.None)
                    {
                        Check(1);
                        KeepBetting();
                    }
                    else
                    {
                        Call(1);
                    }
                }
            }
            else
            {
                int randomnr = Random.Range(0, 11);
                // 50% chance that CPU will raise with better cards
                if (randomnr < 5)
                {
                    Raise(1);
                    KeepBetting();
                }
                else
                {
                    if (bState == BeforeState.None)
                    {
                        Check(1);
                        KeepBetting();
                    }
                    else
                        Call(1);
                }
            }
        }
        else
        {
            // CPU has more money than the player, so it will be more daring
            if (GameManager.instance.Players[0].ShowState > GameManager.instance.Players[1].ShowState)
            {
                int randomnr = Random.Range(0, 11);
                // 40% chance that CPU will bluff
                if (randomnr < 4)
                {
                    Raise(1);
                    KeepBetting();
                }
                else
                {
                    if (bState == BeforeState.None)
                    {
                        Check(1);
                        KeepBetting();
                    }
                    else
                        Call(1);
                }
            }
            else if (GameManager.instance.Players[0].ShowState == GameManager.instance.Players[1].ShowState)
            {
                int randomnr = Random.Range(0, 11);
                // 50% chance that CPU will raise on equal score
                if (randomnr < 5)
                {
                    Raise(1);
                    KeepBetting();
                }
                else
                {
                    if (bState == BeforeState.None)
                    {
                        Check(1);
                        KeepBetting();
                    }
                    else
                        Call(1);
                }
            }
            else
            {
                int randomnr = Random.Range(0, 11);
                // 70% chance that CPU will raise with better cards
                if (randomnr < 7)
                {
                    Raise(1);
                    KeepBetting();
                }
                else
                {
                    if (bState == BeforeState.None)
                    {
                        Check(1);
                        KeepBetting();
                    }
                    else
                        Call(1);
                }
            }
        }
    }

    private void KeepBetting()
    {
        nowPlayer++;
        nowPlayer %= 2;

        if(nowPlayer == 0)
        {
            if (whoAllined)
            {
                allinCanvas.SetActive(true);
            }
            else
            {
                bettingCanvas.SetActive(true);
            }
        }
        else
        {
            if (whoAllined)
            {
                Call(1);
            }
            else
                StartCoroutine(AIBetting());
        }
    }

    public void Btn_Betting(int kind)
    {
        bettingCanvas.SetActive(false);
        allinCanvas.SetActive(false);

        if (kind == 0)
        {
            if(bState == BeforeState.None)
            {
                Check(0);
                KeepBetting();
            }
            else
            {
                Call(0);
            }
        }
        else if (kind == 1)
        {
            Raise(0);
            KeepBetting();
        }
        else
        {
            Fold(0);
        }
    }

    public void Check(int player)
    {
        PriorityPlayer = player;
        bState = BeforeState.Check;
    }

    public void Call(int player)
    {
        //돈이 모자란 경우
        if (GameManager.instance.Players[player].Raise(moneyStack) != 0)
        {
            moneyStack = GameManager.instance.Players[player].money;
            whoAllined = true;
        }

        for (int i = 0; i < 2; i++)
            GameManager.instance.Players[i].Bet(moneyStack);

        moneyStack = 0;
        GameManager.instance.NextCard(PriorityPlayer);
    }

    public void Raise(int player)
    {
        if(GameManager.instance.Players[player].Raise(moneyStack) == 0)
        {
            PriorityPlayer = player;

            moneyStack += 25;
        }
        else
        {
            whoAllined = true;
        }

        bState = BeforeState.Raise;
    }

    public void Fold(int player)
    {
        if (player == 0)
            GameManager.instance.Win(1);
        else
            GameManager.instance.Win(0);
    }
}
