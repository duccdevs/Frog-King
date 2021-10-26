using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowThing : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    public float SmoothOffset = 0.075F;
    float smoothTimeDefault;
    Vector2 velocity = Vector2.zero;
    public Vector2 offset;
    Vector2 offsetStart;
    public bool ChargeMode = false;
    public bool Attacking = false;
    public bool BossHead = false;
    public bool SquidBossHead = false;
    public GameObject TentacleHolder;
    public bool Timetodie = false;

    void Start()
    {
        smoothTimeDefault = smoothTime;
        offsetStart = offset;
        SetRandomIzer();

        if (SquidBossHead && !Timetodie)
        {
            Timetodie = true;
        }
    }

    void Update()
    {
        if (ChargeMode)
        {
            offset = new Vector2(offset.x, 2);
        }
        else
        {
            offset = offsetStart;
        }
        Vector2 targetPos = target.TransformPoint(offset);

        if (!Attacking && !SquidBossHead)
        {
            transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }

        if (SquidBossHead)
        {
            transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
    }

    void SetRandomIzer()
    {
        smoothTime = smoothTimeDefault + Random.Range(-SmoothOffset, SmoothOffset);
        Invoke("SetRandomIzer", Random.Range(0.5F, 1.5F));
    }
}
