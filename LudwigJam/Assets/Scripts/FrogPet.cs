using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogPet : MonoBehaviour
{
    public Transform playerpos;

    public float smoothTime = 0.3F;
    private Vector2 velocity = Vector2.zero;

    public bool JumpAgain = true;

    public Sprite DefSprite;
    public Sprite JumpSprite;

    public void SetPos()
    {
        playerpos.position = new Vector2(playerpos.position.x + Random.Range(-0.25F, 0.25F), Mathf.Round(playerpos.position.y * 2) / 2);
        GetComponent<Animator>().SetBool("Jump", true);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = JumpSprite;

        if (transform.position.x > playerpos.position.x)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
        }
        JumpAgain = false;
    }

    void Update()
    {
        transform.position = Vector2.SmoothDamp(transform.position, playerpos.position, ref velocity, smoothTime);

        float dir = Vector2.Distance(transform.position, playerpos.position);
        if (dir <= 0.4F)
        {
            GetComponent<Animator>().SetBool("Jump", false);
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = DefSprite;
            JumpAgain = true;
        }
    }

    public void JumpThing()
    {
        JumpAgain = true;
    }
}
