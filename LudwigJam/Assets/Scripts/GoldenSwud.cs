using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenSwud : MonoBehaviour
{
    public Sprite GoldenSprite;
    SpriteRenderer spriteHolder;

    void Start()
    {
        spriteHolder = GetComponent<SpriteRenderer>();

        if (PlayerPrefs.GetInt("GSquid", 0) == 1)
        {
            SetGolden();
        }
    }

    void SetGolden()
    {
        spriteHolder.sprite = GoldenSprite;
    }
}
