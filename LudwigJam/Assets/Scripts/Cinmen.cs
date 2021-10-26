using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinmen : MonoBehaviour
{
    public GameObject CinemaHolder;

    public void EnterCinema(float Delay, float Timer)
    {
        Invoke("PlayCin", Delay);
        Invoke("ExitCinema", Timer);
    }

    void PlayCin()
    {
        CinemaHolder.GetComponent<Animator>().SetTrigger("Cin");
    }

    void ExitCinema()
    {
        CinemaHolder.GetComponent<Animator>().SetTrigger("Ex");
    }
}
