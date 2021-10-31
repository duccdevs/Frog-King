using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class GlobalMenu : MonoBehaviour
{
    public bool CanClickButton = true;

    public Animator QuitAnim;
    public Animator StartAnim;
    public Animator CreditAnim;

    public AudioClip ScrollSFX;
    public AudioClip ClickSFX;

    public Text scoreTimeText;
    public GameObject Crown;
    public GameObject SettingsMenu;
    public GameObject[] SetActives;
    float timetime;

    //Settings
    public int Fullscreen = 0;
    public bool inSettings = false;
    public Text muteText;
    public Text SkipText;
    public Slider sliderVol;

    bool MuteMute = true;
    bool SkipSkip = true;

    bool inCredits = false;

    bool Shifting = false;

    void Start()
    {
        Time.timeScale = 1.0F;
        Application.targetFrameRate = 60;

        //Settings
        Fullscreen = PlayerPrefs.GetInt("Fullscreen", 0);
        sliderVol.value = PlayerPrefs.GetFloat("Sound", 0.75F);

        ApplySettings();

        timetime = PlayerPrefs.GetFloat("Time", 0);
        if (timetime == 0)
        {
            scoreTimeText.text = "--:--:--";
        }
        else
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timetime);
            scoreTimeText.text = timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
            Crown.SetActive(true);
        }
    }

    void ApplySettings()
    {
        AudioListener.volume = sliderVol.value;

        if (Fullscreen == 0)
        {
            Screen.SetResolution(1280, 720, false);
        }
        else
        {
            Screen.SetResolution(1920, 1080, true);
        }

        if (PlayerPrefs.GetFloat("Music", 1) == 0)
        {
            muteText.text = "MUTED MUSIC";
            MuteMute = false;
        }
        else
        {
            muteText.text = "MUTE MUSIC";
            MuteMute = true;
        }

        if (PlayerPrefs.GetInt("Skip", 0) == 0)
        {
            SkipText.text = "SKIP INTRO";
            SkipSkip = true;
        }
        else
        {
            SkipText.text = "SKIPPING INTRO";
            SkipSkip = false;
        }

        //Save
        PlayerPrefs.SetInt("Fullscreen", Fullscreen);
        PlayerPrefs.SetFloat("Sound", sliderVol.value);
    }

    public void ClickedButton(int i)
    {
        CanClickButton = false;

        if (i == 0)
        {
            //StartGame
            print("StartGame");
            StartAnim.SetTrigger("Start");
            Invoke("StartGameScene", 6.35F);
            Invoke("CanClickAgain", 30F);
        }

        if (i == 1)
        {
            //Settings
            print("Settings");
            foreach (GameObject go in SetActives)
            {
                go.SetActive(false);
            }
            inSettings = true;
            SettingsMenu.SetActive(true);
            Invoke("CanClickAgain", 0.01F);
        }

        if (i == 2)
        {
            //Credits
            print("Credits");
            Invoke("CanClickAgain", 27F);
            CreditAnim.SetTrigger("Roll");
            StartAnim.SetTrigger("Fade");
            inCredits = true;
            return;
        }

        if (i == 3)
        {
            //Quit
            print("Quit");
            QuitAnim.SetTrigger("Exit");
            Invoke("QuitGame", 2);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Shifting = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Shifting = false;
        }
        if (Input.GetKeyDown(KeyCode.P) && Shifting)
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene(0);
        }

        if (inCredits && Input.anyKey)
        {
            CreditAnim.SetTrigger("Skip");
            StartAnim.SetTrigger("Fade2");
            Invoke("CanClickAgain", 0.25F);
            inCredits = false;
        }

        if (inSettings)
        {
            AudioListener.volume = sliderVol.value;
        }
    }
    public void SetMuted()
    {
        Invoke("CanClickAgain", 0.01F);

        if (MuteMute)
        {
            PlayerPrefs.SetFloat("Music", 0);
            muteText.text = "MUTED MUSIC";
            ApplySettings();
            MuteMute = false;
            return;
        }
        else
        {
            PlayerPrefs.SetFloat("Music", 1);
            muteText.text = "MUTE MUSIC";
            ApplySettings();
            MuteMute = true;
            return;
        }
    }
    public void SetSkip()
    {
        Invoke("CanClickAgain", 0.01F);

        if (SkipSkip)
        {
            PlayerPrefs.SetInt("Skip", 1);
            SkipText.text = "SKIPPING INTRO";
            SkipSkip = false;
            return;
        }
        else
        {
            PlayerPrefs.SetInt("Skip", 0);
            SkipText.text = "SKIP INTRO";
            SkipSkip = true;
            ApplySettings();
            return;
        }
    }
    public void SetFullscreen()
    {
        Invoke("CanClickAgain", 0.25F);

        if (Fullscreen == 0)
        {
            Fullscreen = 1;
            ApplySettings();
            return;
        }
        else
        {
            Fullscreen = 0;
            ApplySettings();
            return;
        }
    }

    public void StopFade()
    {
        StartAnim.SetTrigger("Fade2");
        inCredits = false;
    }

    public void BackFromSettings()
    {
        SettingsMenu.SetActive(false);
        foreach(GameObject go in SetActives)
        {
            go.SetActive(true);
        }
        inSettings = false;
        Invoke("CanClickAgain", 0.01F);

        //SaveSettings
        PlayerPrefs.SetInt("Fullscreen", Fullscreen);
        PlayerPrefs.SetFloat("Sound", sliderVol.value);
    }

    void StartGameScene()
    {
        SceneManager.LoadScene(1);
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void CanClickAgain()
    {
        CanClickButton = true;
    }
}
