using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMan : MonoBehaviour
{
    public GameObject thing;

    public void StopFader()
    {
        thing.GetComponent<GlobalMenu>().StopFade();
    }
}
