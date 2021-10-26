using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBossManager : MonoBehaviour
{
    public GameObject Mouth;
    public GameObject MouthEnd;
    public GameObject Bag;
    public Animator frogAnim;

    public GameObject Middle;
    public GameObject B;
    public GameObject C;

    public GameObject Player;
    Vector2 newPos;

    int UDd;

    bool once = true;
    bool once1 = true;

    void StartAttack()
    {
        //Ribbit
        frogAnim.SetTrigger("Rib");
        print("Attack");
    }
    public void SetMousePos()
    {
        newPos = Player.transform.position;
    }

    public void DoAttack()
    {
        Mouth.SetActive(true);
        MouthEnd.transform.position = newPos;

        Invoke("EndAttack", 1);
    }

    public void EndAttack()
    {
        Mouth.SetActive(false);

        if (Player.GetComponent<PlayerMovement>().InFrog)
        {
            Bag.SetActive(true);
        }
    }

    void Update()
    {
        if (!B.GetComponent<CameraMovement>().FirstBoss)
        {
            if (B.transform.position.y < 230 || B.transform.position.y > 390)
            {
                if (once1)
                {
                    frogAnim.SetTrigger("Do");
                    once = true;
                    once1 = false;
                }
            }
            else
            {
                if (once)
                {
                    frogAnim.SetTrigger("Rib");
                    once1 = true;
                    once = false;
                }
            }
        }
    }

    public void SetFrog()
    {
        StartCoroutine(DoThing());
    }

    IEnumerator DoThing()
    {
        yield return 0;

        //Scmove
        if (!B.GetComponent<CameraMovement>().FirstBoss)
        {
            if (Middle.GetComponent<Middle>().Dir)
            {
                transform.position = new Vector2(5, C.transform.position.y + 1);
            }
            else
            {
                transform.position = new Vector2(-5, C.transform.position.y + 1);
            }
        }
    }
}
