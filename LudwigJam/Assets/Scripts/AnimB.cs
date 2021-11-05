using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimB : MonoBehaviour
{
    public AudioClip[] Sounds;

    public Sprite[] GoldenSprites;
    public SpriteRenderer[] Holder;

    void Start()
    {
        if (PlayerPrefs.GetInt("GM", 0) == 1)
        {
            Holder[0].sprite = GoldenSprites[0];
            Holder[1].sprite = GoldenSprites[1];
            Holder[2].sprite = GoldenSprites[1];
        }
    }

    public void WakeUp()
    {
        if (PlayerPrefs.GetInt("GM", 0) == 1)
        {
            Holder[0].sprite = GoldenSprites[2];
        }
        else
        {
            Holder[0].sprite = GoldenSprites[3];
        }
    }

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
