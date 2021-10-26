using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public AudioSource musicMan;
    GameObject Player;
    public GameObject PauseMenu;
    public Text PauseTimeText;
    float PauseTime = 4;
    public bool DevMode = false;

    public Text TimeText;
    public Text ProgressText;

    float timer = 0.0F;

    float MaxHeight = 400;
    float CurrentHeight;

    public bool BeatGame = false;
    bool once = true;

    void Start()
    {
        Application.targetFrameRate = 60;

        //Saving
        Player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && DevMode)
        {
            Time.timeScale = 10.0F;
        }
        if (Input.GetKeyUp(KeyCode.L) && DevMode)
        {
            Time.timeScale = 1.0F;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyUp(KeyCode.N) && DevMode)
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<AudioSource>().Play();

            PauseTime = 4;

            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                PauseMenu.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.M) && DevMode)
        {
            if (musicMan.volume == 1)
            {
                musicMan.volume = 0;
            }
            else
            {
                musicMan.volume = 1;
            }
        }

        if (PauseTime <= 0)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                PauseTime -= Time.unscaledDeltaTime;
                PauseTimeText.text = PauseTime.ToString("F1");
            }
        }

        if (Input.GetKeyDown(KeyCode.K) && DevMode)
        {
            Player.transform.position = new Vector2(5, 225);
        }

        if (Input.GetKeyDown(KeyCode.J) && DevMode)
        {
            Player.transform.position = new Vector2(3, 106);
        }
    
        //Progress

        if (!BeatGame)
        {
            timer = Mathf.Max(0, timer + Time.deltaTime);
        }
        else if (once)
        {
            //New score
            if (timer < PlayerPrefs.GetFloat("Time", 0) || PlayerPrefs.GetFloat("Time", 0) == 0)
            {
                PlayerPrefs.SetFloat("Time", timer);
            }
            once = false;
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);

        TimeText.text = timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");

        float ProgressValue = ((Mathf.Round(Player.transform.position.y / 10) * 10) / MaxHeight) * 100;
        ProgressText.text = ProgressValue.ToString("F2") + "%";
    }
}
