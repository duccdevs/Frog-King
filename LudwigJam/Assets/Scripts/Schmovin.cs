using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schmovin : MonoBehaviour
{
    float StartAttackTimer = 2.5F;
    public bool Attacking = false;
    float Acceleration = 3.0F;

    void Start()
    {
        GameObject.Find("NamePlate").GetComponent<NamePlate>().ShowName(3);
    }

    public void StartAttack()
    {
        transform.rotation = Quaternion.identity;
        GetComponent<Animator>().SetBool("Attack", false);
        StartAttackTimer = 1.25F;
        Attacking = false;
        Acceleration = 4.0F;
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
            Acceleration += Time.deltaTime * 6;
            transform.Rotate(0, 0, (40 * Mathf.Clamp(Acceleration, 1, 10)) * Time.deltaTime);
            GetComponent<Animator>().SetBool("Attack", true);
        }
    }
}
