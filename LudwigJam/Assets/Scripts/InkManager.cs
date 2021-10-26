using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkManager : MonoBehaviour
{
    private GameObject ScreenPos;

    void Start()
    {
        ScreenPos = Camera.main.transform.parent.gameObject;
        Destroy(gameObject, 20);
    }

    void Update()
    {
        if (transform.position.x > 12 || transform.position.x < -12)
        {
            Destroy(gameObject);
        }

        if (transform.position.y > ScreenPos.transform.position.y + 10 || transform.position.y < ScreenPos.transform.position.y - 6 || transform.position.y > 230)
        {
            Destroy(gameObject);
        }
    }
}
