using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimB : MonoBehaviour
{
    public AudioClip[] Sounds;

    public void SetShake(float time)
    {
        Camera.main.transform.parent.GetComponent<CameraMovement>().ShakeCam(time);
    }

    public void StartBattle()
    {
        Camera.main.transform.parent.GetComponent<CameraMovement>().StartFight();
    }

    public void SFX(int i)
    {
        GetComponent<AudioSource>().PlayOneShot(Sounds[i]);
    }

    public void StopSFX()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void ShowText()
    {
        GameObject.Find("NamePlate").GetComponent<NamePlate>().ShowName(5);
    }
}
