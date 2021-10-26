using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAmFrogme : MonoBehaviour
{
    public bool Frogged = false;
    public bool StartFrog = false;

    public GameObject NormalFrog;

    void Start()
    {
        if (StartFrog)
        {
            Invoke("StartTutorial", 8.0F);
        }
    }

    public void Frogme()
    {
        if (!Frogged)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<Animator>().SetTrigger("Note");
            Frogged = true;
        }
    }

    void StartTutorial()
    {
        GetComponent<Animator>().SetTrigger("Tut");
        Invoke("TutDone", 18);
    }

    public void DoRibbit()
    {
        GetComponent<AudioSource>().Play();
    }

    void TutDone()
    {
        StartFrog = false;
        NormalFrog.SetActive(true);
        NormalFrog.GetComponent<IAmFrogme>().Frogme();
        gameObject.SetActive(false);
        GameObject.Find("Player").GetComponent<PlayerMovement>().CanMove = true;
        Camera.main.transform.parent.GetComponent<CameraMovement>().enabled = true;
    }
}
