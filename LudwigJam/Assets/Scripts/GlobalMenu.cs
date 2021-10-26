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
    public Slider sliderVol;

    bool inCredits = false;

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
            Invoke("CanClickAgain", 0.25F);
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
            if (sliderVol.value != 0)
            {
                muteText.text = "MUTE";
                muteText.transform.parent.GetComponent<Button>().interactable = true;
            }
            else
            {
                muteText.text = "MUTED";
                muteText.transform.parent.GetComponent<Button>().interactable = false;
            }
        }
    }
    public void SetMuted()
    {
        sliderVol.value = 0;
        AudioListener.volume = 0;
        muteText.text = "MUTED";
        muteText.transform.parent.GetComponent<Button>().interactable = false;
        PlayerPrefs.SetFloat("Sound", sliderVol.value);
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
        Invoke("CanClickAgain", 0.25F);

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
