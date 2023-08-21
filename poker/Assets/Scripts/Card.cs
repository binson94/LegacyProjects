using UnityEngine;
using UnityEngine.UI;

public enum Shape
{
    Spade, Diamond, Heart, Clover
}

public class Card : MonoBehaviour
{
    public SpriteRenderer cardSprite;
    public Shape shape;
    public int number;

    //true -> 앞면 보이기, false -> 뒷면 보이기
    public void Show(bool t)
    {
        Vector3 tmp = new Vector3(cardSprite.gameObject.transform.position.x, cardSprite.gameObject.transform.position.y, 0.1f);

        if (t)
            tmp.z = -0.1f;

        cardSprite.gameObject.transform.position = tmp;
    }
}
