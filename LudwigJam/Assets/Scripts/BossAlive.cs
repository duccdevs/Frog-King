using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAlive : MonoBehaviour
{
    public GameObject target;
    public GameObject Boss;

    public bool MonkeBoss = false;
    public bool SquidBoss = false;

    void Update()
    {
        if (MonkeBoss)
        {
            if (Boss.GetComponent<MonkeBoss>().TimeToGo)
            {
                if (target.GetComponent<Middle>().FirstBossDone)
                {
                    Boss.gameObject.SetActive(false);
                }
                else
                {
                    Boss.gameObject.SetActive(true);
                }
            }
        }

        if (SquidBoss)
        {
            if (Boss.transform.GetChild(0).GetComponent<FollowThing>().Timetodie)
            {
                if (target.GetComponent<Middle>().SecondBossDone)
                {
                    Boss.gameObject.SetActive(false);
                }
                else
                {
                    Boss.gameObject.SetActive(true);
                }
            }
        }
    }
}
