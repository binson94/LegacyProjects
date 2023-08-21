using System.Collections;
using UnityEngine;

public enum MonVariable
{
    basic = 1, fast = 2, attackSpeed =3,
    escape = 4, avoid = 5, defense = 6,
    divide = 7, invinsible = 8, defenseplus = 9,
    mars = 10, jupiter = 11, saturn = 12
}

//경로를 도는 적에 관한 클래스
public class Enemy : MonoBehaviour, System.IComparable<Enemy>
{
    /// <summary>
    /// monster idx
    /// </summary>
    public int idx;
    /// <summary>
    /// stat 정보
    /// </summary>
    EnemyStat stat;
    /// <summary>
    /// 현재 체력
    /// </summary>
    int currHp;
    public float MoveSpd { get => stat.MoveSpeed; }

    [SerializeField] SpriteRenderer enemySprite;

    System.Action<float> hpBarUpdate;
    /// <summary>
    /// 어그로 수치
    /// </summary>
    int aggro;
    /// <summary>
    /// 현재 향하는 기준점 idx
    /// </summary>
    int nowTargetIdx;
    public Vector3 targetVector = new Vector3();
    Transform[] pivots;

    public void Init(System.Action<float> hpBar, Transform[] pivots)
    {
        hpBarUpdate = hpBar;
        this.pivots = pivots;
        enemySprite.sprite = GameManager.Resource.Load<Sprite>($"Sprites/Enemies/e{idx}");
        StatLoad();
    }

    public void InstanceStart()
    {
        aggro = 0;
        nowTargetIdx = 1;
        transform.position = pivots[0].position;

        StartCoroutine(MoveToPivot());
    }

    void DivideStart()
    {
        StartCoroutine(MoveToPivot());
    }

    //다음 pivot으로 이동하는 코루틴, 게임 끝날 때까지 무한 반복
    IEnumerator MoveToPivot()
    {
        while (true)
        {
            //목표 pivot 방향으로 나아가기 위한 단위 벡터 생성
            targetVector = pivots[nowTargetIdx].transform.position - transform.position;
            targetVector.Normalize();

            //목표 pivot과 접근할 때까지 이동
            while (Vector3.Distance(transform.position, pivots[nowTargetIdx].transform.position) >= 0.05f)
            {
                transform.Translate(targetVector * stat.MoveSpeed * 0.03f, Space.World);
                yield return new WaitForSeconds(0.03f);
            }

            //목표 pivot 도착 완료, 다음 pivot 설정
            nowTargetIdx = (nowTargetIdx + 1) % 6;
            aggro++;
        }
    }

    public void StatLoad()
    {
        stat = GameManager.Json.GetEnemyStat(idx);
        currHp = stat.HP;
    }

    //피해를 받는 함수
    public void GetDamage(Moon m)
    {
        int dmg = Mathf.Max(1, m.dmg - stat.Defense);

        currHp -= dmg;
        if (currHp <= 0)
            Death();

        hpBarUpdate?.Invoke((float)currHp / stat.HP);
    }

    public void Death()
    {        
        gameObject.SetActive(false);
        currHp = 0;
    }

    public int CompareTo(Enemy other)
    {
        return -aggro.CompareTo(other.aggro);
    }
}
