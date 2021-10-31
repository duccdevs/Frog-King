using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Middle : MonoBehaviour
{
    public Transform A;
    public Transform B;

    public bool FirstBossDone = false;
    public bool SecondBossDone = false;

    public bool MonkeBossman = false;
    public bool SquidBossman = false;
    public bool FrogBossman = false;

    public GameObject FrogHolder;
    public Vector3 oldPos;

    public GameObject SquidHolder;
    bool Once = true;
    bool Once1 = true;
    public bool Dir = true;

    void Start()
    {
        oldPos = transform.position;
    }

    void Update()
    {
        if (MonkeBossman)
        {
            if (B.transform.position.y < 101)
            {
                FirstBossDone = false;
                transform.position = 0.5F * (A.position + B.position);
            }
            else
            {
                FirstBossDone = true;
            }
        }

        if (SquidBossman)
        {
            if (B.transform.position.y < 230 && B.transform.position.y > 101)
            {
                SecondBossDone = false;
                if (A.position.x < B.position.x)
                {
                    transform.position = 0.5F * (new Vector3((A.position.x / 6) + 11.5F, A.position.y, 0) + B.position);
                    if (Once)
                    {
                        SquidHolder.GetComponent<Animator>().SetTrigger("Left");
                        SquidHolder.GetComponent<Schmovin>().StartAttack();
                        Once = false;
                    }
                }
                else
                {
                    transform.position = 0.5F * (new Vector3((A.position.x / 6) - 11.5F, A.position.y, 0) + B.position);
                    if (!Once)
                    {
                        SquidHolder.GetComponent<Animator>().SetTrigger("Right");
                        SquidHolder.GetComponent<Schmovin>().StartAttack();
                        Once = true;
                    }
                }
            }
            else
            {
                SecondBossDone = true;
            }
        }

        if (FrogBossman && FrogHolder.activeSelf)
        {
            if (B.transform.position.y >= 230 && B.transform.position.y < 400)
            {
                transform.position = B.transform.position;

                if (oldPos != B.transform.position && !A.GetComponent<PlayerMovement>().InFrog)
                {
                    //Scmove
                    if (oldPos.y < B.transform.position.y)
                    {
                        FrogHolder.transform.position = new Vector2(0, transform.position.y + 1);
                        FrogHolder.GetComponent<FrogBossManager>().EndAttack();
                        if (Dir)
                        {
                            FrogHolder.GetComponent<Animator>().SetTrigger("UpL");
                        }
                        else
                        {
                            if (Once1)
                            {
                                Once1 = false;
                            }
                            else
                            {
                                FrogHolder.GetComponent<Animator>().SetTrigger("UpR");
                            }
                        }

                        Dir = !Dir;
                        oldPos = B.transform.position;
                        return;
                    }
                    else
                    {
                        FrogHolder.GetComponent<FrogBossManager>().EndAttack();
                        FrogHolder.transform.position = new Vector2(0, transform.position.y + 1);
                        if (Dir)
                        {
                            FrogHolder.GetComponent<Animator>().SetTrigger("DownR");
                        }
                        else
                        {
                            FrogHolder.GetComponent<Animator>().SetTrigger("DownL");
                        }

                        Dir = !Dir;
                        oldPos = B.transform.position;
                        return;
                    }
                }
            }
        }
    }
}
