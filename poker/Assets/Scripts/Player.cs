using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Top, Pair, TwoPair, Triple, Straight,
    BackStraight, Mountain, Flush, FullHouse,
    FourCard, StraightFlush, BackStraightFlush, RoyalStraightFlush
}


public class Player : MonoBehaviour
{
    public int topNumber;
    public Shape topShape;
    public Card dummy;

    public int money;

    public Card HiddenCard;         //나만 보는 1장 저장
    public Card[] ShowCard;         //보이는 4장 저장
    public Card[] TotalCards;       //전체 5장 저장, 처음에 앞 2장 중 선택

    public State ShowState;
    public State TotalState;

    public void ShowHand()
    {
        TotalCards[0].Show(true);
        TotalCards[1].Show(true);
    }

    public void ChooseHidden(int c = -1)
    {
        if(c == -1)
        {
            if (TotalCards[0].number < TotalCards[1].number)
            {
                HiddenCard = TotalCards[0];
                ShowCard[0] = TotalCards[1];
            }
            else
            {
                HiddenCard = TotalCards[1];
                ShowCard[0] = TotalCards[0];
            }

            ShowCard[0].Show(true);
        }
        else
        {
            HiddenCard = TotalCards[c];

            if (c == 0)
                ShowCard[0] = TotalCards[1];
            else
                ShowCard[0] = TotalCards[0];
        }
    }

    public void ShowStateUpdate()
    {
        Card[] tmp = new Card[5];

        for (int i = 0; i < 4; i++)
            tmp[i] = ShowCard[i];
        tmp[4] = dummy;

        Sort(tmp);
        
        if (isTriple(tmp))
            ShowState = State.Triple;
        else if (isTwoPair(tmp))
            ShowState = State.TwoPair;
        else if (isPair(tmp))
            ShowState = State.Pair;
        else
        {
            ShowState = State.Top;
            topNumber = tmp[0].number;
            topShape = tmp[0].shape;
        }
    }

    public void TotalStateUpdate()
    {
        Sort(TotalCards);

        if (isMountain(TotalCards) && isFlush(TotalCards))
            TotalState = State.RoyalStraightFlush;
        else if (isBackStraight(TotalCards) && isFlush(TotalCards))
            TotalState = State.BackStraightFlush;
        else if (isStraight(TotalCards) && isFlush(TotalCards))
            TotalState = State.StraightFlush;
        else if (isFourCard(TotalCards))
            TotalState = State.FourCard;
        else if (isFullHouse(TotalCards))
            TotalState = State.FullHouse;
        else if (isFlush(TotalCards))
            TotalState = State.FullHouse;
        else if (isMountain(TotalCards))
            TotalState = State.FullHouse;
        else if (isBackStraight(TotalCards))
            TotalState = State.FullHouse;
        else if (isStraight(TotalCards))
            TotalState = State.FullHouse;
        else if (isTriple(TotalCards))
            TotalState = State.Triple;
        else if (isTwoPair(TotalCards))
            TotalState = State.TwoPair;
        else if (isPair(TotalCards))
            TotalState = State.Pair;
        else
        {
            TotalState = State.Top;
            topNumber = TotalCards[0].number;
            topShape = TotalCards[0].shape;
        }
    }

    public void Sort(Card[] c)
    {
        int max;
        Card tmp;
        for (int i = 0; i < 4 && c[i] != null; i++)
        {
            max = i;
            for (int j = i + 1; j<5 && c[j] != null; j++)
            {
                if (c[max].number < c[j].number)
                    max = j;
                else if (c[max].number == c[j].number)
                    if (c[max].shape > c[j].shape)
                        max = j;
            }

            tmp = c[max];
            c[max] = c[i];
            c[i] = tmp;
        }
    }

    private bool isFourCard(Card[] c)
    {
        for (int i = 0; i < 2; i++)
        {
            if ((c[i].number == c[i + 1].number) && (c[i + 1].number == c[i + 2].number) && (c[i + 2].number == c[i + 3].number))
            {
                if(c[i].number != 0)
                {
                    topNumber = c[i].number;
                    topShape = c[i].shape;
                    return true;
                }
            }
        }

        return false;
    }

    private bool isFullHouse(Card[] c)
    {
        if (c[4].number == 0)
            return false;

        if ((c[0].number == c[1].number) && (c[1].number == c[2].number) && c[3].number == c[4].number)
        {
            topNumber = c[0].number;
            topShape = c[0].shape;
            return true;

        }
        else if ((c[0].number == c[1].number) && (c[2].number == c[3].number) && (c[3].number == c[4].number))
        {
            topNumber = c[2].number;
            topShape = c[2].shape;
            return true;
        }

        return false;
    }

    private bool isFlush(Card[] c)
    {
        if (c[4].number == 0)
            return false;

        if ((c[0].shape == c[1].shape) && (c[1].shape == c[2].shape) && (c[2].shape == c[3].shape) && (c[3].shape == c[4].shape))
        {
            topShape = c[0].shape;
            topNumber = c[0].number;
            return true;
        }
        else
            return false;
    }

    private bool isMountain(Card[] c)
    {
        if (c[4].number == 0)
            return false;

        return (c[0].number == 14) && isStraight(c);
    }

    private bool isBackStraight(Card[] c)
    {
        if (c[4].number == 0)
            return false;

        if ((c[0].number == 14) && (c[1].number == 5) && (c[2].number == 4) && (c[3].number == 3) && (c[4].number == 2))
        {
            topShape = c[0].shape;
            topNumber = 14;
            return true;
        }
        else
            return false;
    }

    private bool isStraight(Card[] c)
    {
        if (c[4].number == 0)
            return false;

        if ((c[0].number - c[1].number == 1) && (c[1].number - c[2].number == 1) && (c[2].number - c[3].number == 1) && (c[3].number - c[4].number == 1))
        {
            topShape = c[0].shape;
            topNumber = c[0].number;
            return true;
        }
        else
            return false;
    }

    private bool isTriple(Card[] c)
    {
        for (int i = 0; i < 3; i++)
        {
            if ((c[i].number == c[i + 1].number) && (c[i + 1].number == c[i + 2].number))
            {
                if(c[i].number != 0)
                {
                    topNumber = c[i].number;
                    topShape = c[i].shape;
                    return true;
                }
            }
        }

        return false;
    }

    private bool isTwoPair(Card[] c)
    {
        if (c[0].number == c[1].number && c[2].number == c[3].number)
        {
            if (c[0].number != 0 && c[2].number != 0)
            {
                topNumber = c[0].number;
                topShape = c[0].shape;
                return true;
            }
        }
        else if (c[0].number == c[1].number && c[3].number == c[4].number)
        {
            if (c[0].number != 0 && c[3].number != 0)
            {
                topNumber = c[0].number;
                topShape = c[0].shape;
                return true;
            }
        }
        else if (c[1].number == c[2].number && c[3].number == c[4].number)
        {
            if (c[1].number != 0 && c[3].number != 0)
            {
                topNumber = c[1].number;
                topShape = c[1].shape;
                return true;
            }
        }

        return false;
    }

    private bool isPair(Card[] c)
    {
        for (int i = 0; i < 4; i++)
        {
            if (c[i].number != 0)
            {
                if (c[i].number == c[i + 1].number)
                {
                    topNumber = c[i].number;
                    topShape = c[i].shape;

                    return true;
                }
            }
        }

        return false;
    }

    public int Raise(int m)
    {
        if (money >= m)
            return 0;
        else
        {
            return money;
        }
    }

    public void Bet(int m)
    {
        money -= m;
        GameManager.instance.money += m;
    }
}
