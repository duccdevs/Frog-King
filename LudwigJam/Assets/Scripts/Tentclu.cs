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

    public Sprite GoldenTent;
    SpriteRenderer spriteHolder;

    float InkTimer = 1.7F;

    void Start()
    {
        spriteHolder = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Holder = transform.parent.gameObject;
        Aim = transform.GetChild(0).gameObject;
        
        if (PlayerPrefs.GetInt("GSquid", 0) == 1)
        {
            SetGolden();
        }
    }

    void SetGolden()
    {
        spriteHolder.sprite = GoldenTent;
    }

    void Update()
    {
        if (Holder.GetComponent<Schmovin>().Attacking)
        {
            if (Once)
            {
                InkTimer = 1.6F;
                DoInks = false;
                Invoke("StartShoot", 0.4F);
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

                InkTimer = 1.8F;
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
