using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeBoss : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject Head;
    public GameObject Player;
    int RandomHand;
    public bool TimeToGo = false;
    Vector2 ThrowPos = Vector2.zero;

    public AudioClip[] Sounds;

    void Start()
    {
        Invoke("SwingHand", Random.Range(2, 3));
        TimeToGo = true;
        GameObject.Find("NamePlate").GetComponent<NamePlate>().ShowName(1);
    }

    void SwingHand()
    {
        if (Player.transform.position.x > 0)
        {
            RandomHand = 0;
        }
        else
        {
            RandomHand = 1;
        }

        if (RandomHand == 0)
        {
            LeftHand.GetComponent<FollowThing>().ChargeMode = true;
        }
        if (RandomHand == 1)
        {
            RightHand.GetComponent<FollowThing>().ChargeMode = true;
        }

        Head.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Attack");
        GetComponent<AudioSource>().clip = Sounds[1];
        GetComponent<AudioSource>().volume = 1F;
        GetComponent<AudioSource>().Play();
        Invoke("LockPos", 0.9F);
        Invoke("DoSwing", 1.2F);
    }

    void LockPos()
    {
        if (RandomHand == 0)
        {
            ThrowPos = Player.transform.position - LeftHand.transform.position;
        }
        if (RandomHand == 1)
        {
            ThrowPos = Player.transform.position - RightHand.transform.position;
        }
    }

    void DoSwing()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().volume = 0.65F;
        GetComponent<AudioSource>().clip = Sounds[0];
        GetComponent<AudioSource>().Play();

        if (RandomHand == 0)
        {
            LeftHand.GetComponent<FollowThing>().Attacking = true;
            LeftHand.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
            LeftHand.GetComponent<Rigidbody2D>().isKinematic = false;
            LeftHand.GetComponent<Rigidbody2D>().AddForce(ThrowPos * 4F, ForceMode2D.Impulse);
        }
        if (RandomHand == 1)
        {
            RightHand.GetComponent<FollowThing>().Attacking = true;
            RightHand.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
            RightHand.GetComponent<Rigidbody2D>().isKinematic = false;
            RightHand.GetComponent<Rigidbody2D>().AddForce(ThrowPos * 4F, ForceMode2D.Impulse);
        }
        Invoke("ResetHands", 0.75F);
        Invoke("SwingHand", 5);
    }

    void ResetHands()
    {
        RightHand.GetComponent<FollowThing>().Attacking = false;
        RightHand.GetComponent<FollowThing>().ChargeMode = false;
        RightHand.GetComponent<Rigidbody2D>().isKinematic = true;
        RightHand.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        RightHand.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        LeftHand.GetComponent<FollowThing>().Attacking = false;
        LeftHand.GetComponent<FollowThing>().ChargeMode = false;
        LeftHand.GetComponent<Rigidbody2D>().isKinematic = true;
        LeftHand.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        LeftHand.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
    }
}
