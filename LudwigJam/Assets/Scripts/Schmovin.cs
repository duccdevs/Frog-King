using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schmovin : MonoBehaviour
{
    float StartAttackTimer = 2.5F;
    public bool Attacking = false;
    float Acceleration = 3.0F;
    public Animator animHolder;

    void Start()
    {
        GameObject.Find("NamePlate").GetComponent<NamePlate>().ShowName(3);
    }

    public void StartAttack()
    {
        transform.rotation = Quaternion.identity;
        GetComponent<Animator>().SetBool("Attack", false);
        StartAttackTimer = 0.75F;
        Attacking = false;
        Acceleration = 6.0F;
        print("HO");
        animHolder.SetTrigger("Bob");
    }

    void Update()
    {
        StartAttackTimer -= Time.deltaTime;
        if (StartAttackTimer <= 0.0F && !Attacking)
        {
            Attacking = true;
            StartAttackTimer = 0.0F;
        }

        if (Attacking)
        {
            Acceleration += Time.deltaTime * 19;
            transform.Rotate(0, 0, (8 * Mathf.Clamp(Acceleration, 1, 40)) * Time.deltaTime);
            GetComponent<Animator>().SetBool("Attack", true);
        }
    }
}
