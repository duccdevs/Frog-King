using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentclu : MonoBehaviour
{
    public GameObject InkObj;
    GameObject Holder;
    GameObject Aim;
    bool Once = true;
    bool DoInks = false;

    float InkTimer = 1.0F;

    void Start()
    {
        Holder = transform.parent.gameObject;
        Aim = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Holder.GetComponent<Schmovin>().Attacking)
        {
            if (Once)
            {
                InkTimer = 0.65F;
                DoInks = false;
                Invoke("StartShoot", 0.35F);
                Once = false;
            }
        }
        else
        {
            Once = true;
        }

        if (DoInks)
        {
            InkTimer -= Time.deltaTime;
            if (InkTimer <= 0.0F)
            {
                int RrandomShot = 0;
                if (RrandomShot == 0 && Holder.GetComponent<Schmovin>().Attacking)
                {
                    //ShootInk
                    GameObject inkinstance = Instantiate(InkObj, Aim.transform.position, transform.rotation);
                    inkinstance.GetComponent<Rigidbody2D>().AddForce(-transform.up * 2.3F, ForceMode2D.Impulse);
                }

                InkTimer = 1.2F;
            }
        }
    }

    void StartShoot()
    {
        if (Holder.GetComponent<Schmovin>().Attacking)
        {
            DoInks = true;
        }
    }
}
