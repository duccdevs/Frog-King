using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePos : MonoBehaviour
{
    LineRenderer line;
    public GameObject Tongue;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, Tongue.transform.position);
    }

    void OnDisable()
    {
        line.SetPosition(0, Vector2.zero);
        line.SetPosition(1, Vector2.zero);
    }

    void OnEnable()
    {
        print("Hello");
        Tongue.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
        Invoke("StopTongue", 0.575F);
    }
    void StopTongue()
    {
        Tongue.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
    }
}
