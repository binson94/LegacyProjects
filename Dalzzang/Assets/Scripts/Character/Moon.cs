using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//주인공 Moon 클래스
public class Moon : MonoBehaviour
{

    [SerializeField] GameScene gs;


    [Header("Bullet")]
    [SerializeField] Bullet bulletPrefab;
    Pool<Bullet> bulletPool;


    /// <summary>
    /// 현재 공격 중인가 여부 -> 적을 빨리 잡아서 비는 타임이 생겼을 때 공격을 멈추기 위해
    /// </summary>
    bool isAttack = false;
    /// <summary>
    /// 공격 대상 대기열, 많이 돈 녀석 먼저 때림
    /// </summary>
    List<Enemy> enemies = new List<Enemy>();

    /// <summary>
    /// 공격력
    /// </summary>
    public int dmg;
    /// <summary>
    /// 공격 속도
    /// </summary>
    float atkSpd;

    public Animator anim;

    public void Start()
    {
        bulletPool = new Pool<Bullet>(bulletPrefab);
        dmg = GameManager.Save.Dmg;
        atkSpd = GameManager.Save.AtkSpd;
    }

    //적이 감지 범위 내 들어오면 실행
    //적이 하나라도 남아있다면 반복
    IEnumerator Attack()
    {
        //이미 이 함수 실행 중 == (isAttack == true) -> 함수 끝내기
        if (isAttack)
            yield break;
        isAttack = true;

        while(!gs.win && !gs.lose)
        {
            for (int i = 0; i < enemies.Count; i++)
                if (!enemies[i].isActiveAndEnabled)
                {
                    enemies.RemoveAt(i);
                    i--;
                }

            if (enemies.Count > 0)
            {
                enemies.Sort();
                Shot(enemies[0]);

                yield return new WaitForSeconds(1 / atkSpd);
            }
            else
                break;
        }

        isAttack = false;
    }

    //총알 발사
    void Shot(Enemy target)
    {
        if (!target)
            return;

        Bullet bullet = bulletPool.Create();

        //총알 위치 Moon으로 이동
        bullet.transform.position = transform.position;

        //총알 나아갈 방향 = 달에서 타겟 바라보는 방향 + 타겟 이동 보정
        bullet.moveVector = (target.transform.position - transform.position + target.targetVector * 0.2f * target.MoveSpd).normalized;

        //총알 활성화
        bullet.gameObject.SetActive(true);
        bullet.Init(this, bulletPool);
    }

    //새로운 적 생성 -> List에 넣기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            enemies.Add(collision.GetComponent<Enemy>());
        }

        StartCoroutine(Attack());
    }
}
