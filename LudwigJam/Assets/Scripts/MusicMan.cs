using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMan : MonoBehaviour
{
    public GameObject B;
    public AudioClip[] Songs;

    bool StartSong = true;
    bool StartSong1 = true;
    bool StartSong2 = true;

    void Update()
    {
        if (B.transform.position.y < 110)
        {
            StartSong1 = true;
            StartSong2 = true;

            //Song1
            if (StartSong)
            {
                GetComponent<AudioSource>().clip = Songs[0];
                GetComponent<AudioSource>().Play();
                StartSong = false;
            }
        }

        if (B.transform.position.y >= 110 && B.transform.position.y < 230)
        {
            StartSong = true;
            StartSong2 = true;

            //Song2
            if (StartSong1)
            {
                GetComponent<AudioSource>().clip = Songs[1];
                GetComponent<AudioSource>().Play();
                StartSong1 = false;
            }
        }

        if (B.transform.position.y >= 230 && B.transform.position.y < 400)
        {
            StartSong = true;
            StartSong1 = true;

            //Song2
            if (StartSong2)
            {
                GetComponent<AudioSource>().clip = Songs[2];
                GetComponent<AudioSource>().Play();
                StartSong2 = false;
            }
        }

        if (B.transform.position.y >= 400)
        {
            StartSong = true;
            StartSong1 = true;
            StartSong2 = true;
            GetComponent<AudioSource>().Stop();
        }
    }
}
