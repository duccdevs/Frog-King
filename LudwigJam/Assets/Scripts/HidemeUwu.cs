using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HidemeUwu : MonoBehaviour
{
    float timetime = 2.0F;
    bool once = true;
    public float TimeString;
    public Text TimeText;

    void Update()
    {
        if (transform.gameObject.activeSelf)
        {
            timetime -= Time.deltaTime;
            if (timetime <= 0.0F)
            {
                transform.gameObject.SetActive(false);
                timetime = 2.0F;
                once = false;
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(TimeString);
            TimeText.text = timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + ":" + timeSpan.Milliseconds.ToString("000");
        }
    }
}
