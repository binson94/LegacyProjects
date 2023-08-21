using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moon 클래스가 발사하는 총알 클래스
public class Bullet : MonoBehaviour
{
    Moon shoter;
    Pool<Bullet> pool;

    bool isCrash = false;
    bool isDmg = false;

    public Vector2 moveVector;

    //총알 발사 시작, Moon 클래스에서 호출, Moon의 데미지, 방어력 관통 받아옴
    public void Init(Moon caster, Pool<Bullet> p)
    {
        transform.GetChild(0).rotation = Quaternion.LookRotation(Vector3.forward, moveVector);

        shoter = caster;
        pool = p;
        isCrash = isDmg = false;
        StartCoroutine(Move());
    }

    //타겟을 향해 이동
    IEnumerator Move()
    {
        while (!isCrash)
        {
            transform.Translate(moveVector * 0.45f);
            yield return new WaitForSeconds(0.03f);
        }
    }
    
    //적과 충돌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCrash && collision.gameObject.tag == "Enemy")
        {
            isCrash = true;
            Damage(collision.gameObject.GetComponent<Enemy>());
        }
    }

    //총알이 빗나간 경우 처리
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isCrash = true;
            pool.Remove(this);
            gameObject.SetActive(false);
        }
    }

    //적에게 데미지
    void Damage(Enemy ene)
    {
        if (isDmg)
            return;
        isDmg = true;

        //피해 입히기
        ene.GetDamage(shoter);
        pool.Remove(this);
        gameObject.SetActive(false);
    }
}
