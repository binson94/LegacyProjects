using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int top;        //스택처럼 이용
    public Card[] deck;
    public Card dummy;

    public int nextCardIdx;   //showCard 기준 다음 추가 위치

    public Player[] Players;    //0 : 나, 1 : 적

    public Transform DeckPos;           //덱의 위치
    public Transform[] SelectPos;       //카드 초기 나눠줄 때 위치

    public Transform[] HiddenCardPos;       //초기에 선택하지 않은 카드 위치
    public Transform[] HandPos;             //3장 위치

    public Transform[] FinalCardPos;        //최종 5장 위치

    public Transform CardParent;    //카드 부모
    public GameObject cardPrefab;   //카드 프리팹
    public Sprite[] cardImages;      //인스펙터 창에서 할당

    public GameObject SelectCanvas;
    public GameObject gameoverCanvas;
    public Text WinnerTxt;

    public Text[] MoneyText;

    public int money = 0;

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

    private void Start()
    {
        SelectCanvas.SetActive(false);

        dummy.number = 0;
        dummy.shape = Shape.Clover;

        //모든 카드 더미로 초기화
        for (int i = 0; i < 2; i++)
        {
            Players[i].money = 1000;

            for (int j = 0; j < 4; j++)
            {
                Players[i].TotalCards[j] = dummy;
                Players[i].ShowCard[j] = dummy;
            }
            Players[i].TotalCards[4] = dummy;
            Players[i].HiddenCard = dummy;
        }

        MakeDeck();

        StartCoroutine(MoveDeck(DeckPos.position));
        StartCoroutine(TextUpdate());
    }

    //덱 생성, 스 다 하 클 순서로 정렬된 덱으로 생성됨
    private void MakeDeck()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                deck[i * 13 + j] = Instantiate(cardPrefab).GetComponent<Card>();
                deck[i * 13 + j].shape = (Shape)i;
                deck[i * 13 + j].number = j + 2;
                deck[i * 13 + j].name = i.ToString() + ' ' + (j + 2).ToString();

                deck[i * 13 + j].cardSprite.sprite = cardImages[i * 13 + j];

                deck[i * 13 + j].transform.parent = CardParent;
            }
        }
    }

    public void Shuffle()
    {
        top = 51;
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
        Queue<Card> tmp = new Queue<Card>();        //셔플 시 위치 바꿀 카드들 넣을 공간

        int j;
        int pivot = Random.Range(7, 21);        //셔플 카드 선택 지점
        int length = Random.Range(15, 26);      //셔플 시 이동할 카드 장 수

        //셔플할 카드들 tmp에 저장
        for (j = pivot; j < pivot + length; j++)
        {
            tmp.Enqueue(deck[j]);
        }

        //셔플을 위해 빼낸 카드들 뒤에 있는 카드들 앞당기기
        for (j = pivot; j + length < 52; j++)
        {
            deck[j] = deck[j + length];
        }

        //tmp에 있던 카드들 뒤에 추가
        for (; j < 52; j++)
        {
            deck[j] = tmp.Dequeue();
        }
    }

    //파로 셔플 방식
    private void Shuffle_Type2()
    {
        Queue<Card> tmp = new Queue<Card>();       //셔플 시 위치 바꿀 카드들 저장할 공간

        //덱 절반을 tmp에 넣기
        for (int i = 26; i < 52; i++)
            tmp.Enqueue(deck[i]);

        //남은 카드들 사이사이 빈 공간 만들기
        for (int i = 25; i >= 0; i--)
        {
            deck[i * 2] = deck[i];
        }

        //tmp에 있는 카드들 사이사이 끼워넣기
        for (int i = 0; i < 26; i++)
        {
            deck[2 * i + 1] = tmp.Dequeue();
        }
    }

    //모든 카드를 순차적으로 이동시키는 함수
    private IEnumerator MoveDeck(Vector3 pos)
    {
        yield return new WaitForSeconds(1f);
        Vector2 tmp;
        for (int i = 51; i >= 0; i--)
        {
            tmp.x = pos.x;
            tmp.y = pos.y;

            StartCoroutine(MoveCard(deck[i].gameObject, tmp));
            yield return new WaitForSeconds(0.05f);
        }
        
        Shuffle();
        
        StartCoroutine(DivideCard());
    }

    private IEnumerator MoveCard(GameObject card, Vector2 TargetV)
    {
        Vector2 moveVector = new Vector2(TargetV.x - card.transform.position.x, TargetV.y - card.transform.position.y);

        for (float f = 0; f <= 1; f += 0.050f)
        {
            card.transform.Translate(moveVector * 0.050f);
            yield return new WaitForSeconds(0.025f);
        }
        card.transform.position = TargetV;
    }

    //초기에 카드 2장씩 배분
    private IEnumerator DivideCard()
    {
        if(Players[0].money == 0)
        {
            WinnerTxt.text = "Player 1 Win";
            yield break;
        }
        else if(Players[1].money == 0)
        {
            WinnerTxt.text = "Player 2 Win";
            yield break;
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(MoveCard(deck[top].gameObject, SelectPos[i].position));

            Players[i % 2].TotalCards[i / 2] = deck[top--];

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        Players[0].ShowHand();
        SelectCanvas.SetActive(true);
    }

    //c는 선택받지 못한 카드의 인덱스
    public void PlayerChoose(int c)
    {
        money = 20;

        for (int i = 0; i < 2; i++)
            Players[i].money -= 10;

        SelectCanvas.SetActive(false);

        Players[0].ChooseHidden(c);
        Players[1].ChooseHidden();

        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(MoveCard(Players[i].HiddenCard.gameObject, HiddenCardPos[i].position));
            StartCoroutine(MoveCard(Players[i].ShowCard[0].gameObject, HandPos[i * 4].position));
        }

        nextCardIdx = 1;
        
        BettingManager.instance.StartBetting();
    }
    
    public void NextCard(int player)
    {
        StartCoroutine(GiveCard(player));
    }

    private IEnumerator GiveCard(int player)
    {
        if (nextCardIdx == 0)
        {
            StartCoroutine(DivideCard());
        }
        else if (nextCardIdx == 4)
        {
            GameEnd();
            nextCardIdx++;
        }
        else
        {
            if (player == 0)
            {
                deck[top].Show(true);
                Players[0].ShowCard[nextCardIdx] = Players[0].TotalCards[nextCardIdx + 1] = deck[top--];
                StartCoroutine(MoveCard(Players[0].ShowCard[nextCardIdx].gameObject, HandPos[nextCardIdx].position));

                yield return new WaitForSeconds(0.5f);

                if (nextCardIdx != 3)
                    deck[top].Show(true);

                Players[1].ShowCard[nextCardIdx] = Players[1].TotalCards[nextCardIdx + 1] = deck[top--];
                StartCoroutine(MoveCard(Players[1].ShowCard[nextCardIdx].gameObject, HandPos[nextCardIdx + 4].position));
            }
            else
            {
                if (nextCardIdx != 3)
                    deck[top].Show(true);

                Players[1].ShowCard[nextCardIdx] = Players[1].TotalCards[nextCardIdx + 1] = deck[top--];
                StartCoroutine(MoveCard(Players[1].ShowCard[nextCardIdx].gameObject, HandPos[nextCardIdx + 4].position));

                yield return new WaitForSeconds(0.5f);

                deck[top].Show(true);
                Players[0].ShowCard[nextCardIdx] = Players[0].TotalCards[nextCardIdx + 1] = deck[top--];
                StartCoroutine(MoveCard(Players[0].ShowCard[nextCardIdx].gameObject, HandPos[nextCardIdx].position));
            }

            nextCardIdx++;
        }

        yield return new WaitForSeconds(1f);

        if (nextCardIdx != 5)
            BettingManager.instance.StartBetting();
    }

    private void GameEnd()
    {
        Players[1].HiddenCard.Show(true);
        Players[1].ShowCard[3].Show(true);

        for(int i =0;i<2;i++)
        {
            Players[i].TotalStateUpdate();

            for (int j = 0; j < 5; j++)
            {
                StartCoroutine(MoveCard(Players[i].TotalCards[j].gameObject, FinalCardPos[j + 5 * i].position));
            }
        }

        FindWinner();
    }

    //GameEnd에서 호출
    private void FindWinner()
    {
        if (Players[0].TotalState > Players[1].TotalState) 
        {
            Win(0);
        }
        else if (Players[0].TotalState == Players[1].TotalState)
        {
            if(Players[0].topNumber > Players[1].topNumber)
            {
                Win(0);
            }
            else if(Players[0].topNumber == Players[1].topNumber)
            {
                if(Players[0].topShape < Players[1].topShape)
                {
                    Win(0);
                }
                else
                {
                    Win(1);
                }
            }
            else
            {
                Win(1);
            }
        }
        else
        {
            Win(1);
        }
    }

    public void Win(int player)
    {
        WinnerTxt.text = "Player " + (player + 1).ToString() + " Win";

        Players[player].money += money;
        gameoverCanvas.SetActive(true);
    }

    public void Re()
    {
        money = 0;
        gameoverCanvas.SetActive(false);
        BettingManager.instance.whoAllined = false;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (Players[i].TotalCards[j] == dummy)
                    continue;

                Players[i].TotalCards[j].Show(false);
                StartCoroutine(MoveCard(Players[i].TotalCards[j].gameObject, DeckPos.position));

                Players[i].TotalCards[j] = dummy;
            }

            for (int j = 0; j < 4; j++)
                Players[i].ShowCard[j] = dummy;
            Players[i].HiddenCard = dummy;
        }

        Shuffle();
        nextCardIdx = 0;

        StartCoroutine(DivideCard());
    }

    public IEnumerator TextUpdate()
    {
        while (true)
        {
            MoneyText[0].text = money.ToString();
            MoneyText[1].text = Players[0].money.ToString();
            MoneyText[2].text = Players[1].money.ToString();
            MoneyText[3].text = BettingManager.instance.moneyStack.ToString();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
