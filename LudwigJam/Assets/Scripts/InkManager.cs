using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    private GameObject ScreenPos;
    SpriteRenderer SpriteHolder;

    public Sprite GoldenInk;

    void Start()
    {
        ScreenPos = Camera.main.transform.parent.gameObject;
        SpriteHolder = GetComponent<SpriteRenderer>();
        Invoke("EnableBox", 0.25F);
        Destroy(gameObject, 10);

        if (PlayerPrefs.GetInt("GSquid", 0) == 1)
        {
            SetGolden();
        }
    }

    void SetGolden()
    {
        SpriteHolder.sprite = GoldenInk;
    }

    void Update()
    {
        if (transform.position.x > 12 || transform.position.x < -12)
        {
            Destroy(gameObject);
        }

        if (transform.position.y > ScreenPos.transform.position.y + 5.25F || transform.position.y < ScreenPos.transform.position.y - 5.25F || transform.position.y > 225)
        {
            Destroy(gameObject);
        }
    }

    void EnableBox()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
