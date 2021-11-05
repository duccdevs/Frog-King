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
    public GameObject CamHolder;
    public Text PauseTimeText;
    float PauseTime = 1;
    public bool DevMode = false;

    public Text TimeText;
    public Text ProgressText;
    Color TimeTextColorStart;
    public Color TimeTextColorFade;

    public int FrogAmount = 0;
    public int GotHit = -1;

    public float timer = 0.0F;

    float MaxHeight = 400;
    float CurrentHeight;

    public bool BeatGame = false;
    bool once = true;
    bool Pausin = false;
    public bool SkipIntros = false;

    void Start()
    {
        Application.targetFrameRate = 60;

        TimeTextColorStart = TimeText.color;

        //Saving
        Player = GameObject.Find("Player");

        if (PlayerPrefs.GetInt("Skip", 0) == 1)
        {
            SkipIntros = true;
        }
        else
        {
            SkipIntros = false;
        }
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
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Pausin = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Pausin = false;
        }
        if (Input.GetKeyUp(KeyCode.R) && Pausin)
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyUp(KeyCode.N) && DevMode)
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Esc"))
        {
            GetComponent<AudioSource>().Play();

            PauseTime = 1;

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

        musicMan.volume = PlayerPrefs.GetFloat("Music", 1);

        if (PauseTime <= 0)
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Escape) || Input.GetButton("Esc"))
        {
            if (Time.timeScale == 0)
            {
                PauseTime -= Time.unscaledDeltaTime;
                PauseTimeText.text = "HOLD - " + PauseTime.ToString("F1");
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

        //Stuff
        if (Player.transform.position.x < -6.75F && Player.transform.position.y > CamHolder.transform.position.y + 3.45F)
        {
            TimeText.color = TimeTextColorFade;
            ProgressText.color = TimeTextColorFade;
        }
        else
        {
            TimeText.color = TimeTextColorStart;
            ProgressText.color = TimeTextColorStart;
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
            if(GotHit == 0)
            {
                PlayerPrefs.SetInt("God", 1);
            }
            if (FrogAmount >= 21)
            {
                print("Unlocked Frog Pet");
                PlayerPrefs.SetInt("Frog", 1);
            }
            once = false;
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);

        TimeText.text = timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");

        float ProgressValue = ((Mathf.Round(Player.transform.position.y / 10) * 10) / MaxHeight) * 100;
        ProgressText.text = ProgressValue.ToString("F2") + "%";
    }
}
