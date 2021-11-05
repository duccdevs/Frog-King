using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogGolden : MonoBehaviour
{
    public Sprite GoldenSprite;
    SpriteRenderer spriteHolder;

    public Material GoldMat;

    public bool Mat = false;

    void Start()
    {
        spriteHolder = GetComponent<SpriteRenderer>();

        if (PlayerPrefs.GetInt("GFrog", 0) == 1)
        {
            SetGolden();
        }
    }

    void SetGolden()
    {
        if (Mat)
        {
            spriteHolder.material = GoldMat;
        }
        else
        {
            spriteHolder.sprite = GoldenSprite;
        }
    }
}
