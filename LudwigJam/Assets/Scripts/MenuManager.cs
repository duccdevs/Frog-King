using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    GameObject Img;
    Text buttonText;
    GameObject menuMan;

    public int MenuID = 0;

    void Start()
    {
        menuMan = GameObject.Find("MenuManager");
        Img = transform.GetChild(1).gameObject;
        buttonText = transform.GetChild(0).GetComponent<Text>();
    }

    public void Entered()
    {
        if (menuMan.GetComponent<GlobalMenu>().CanClickButton)
        {
            menuMan.GetComponent<AudioSource>().PlayOneShot(menuMan.GetComponent<GlobalMenu>().ScrollSFX);
            if (transform.parent.GetComponent<Button>() != null && !transform.parent.GetComponent<Button>().interactable)
            {
                Img.SetActive(false);
            }
            else
            {
                Img.SetActive(true);
            }
        }
    }
    public void Exited()
    {
        Img.SetActive(false);
        buttonText.color = Color.white;
    }

    public void Clicked()
    {
        if (menuMan.GetComponent<GlobalMenu>().CanClickButton)
        {
            menuMan.GetComponent<AudioSource>().PlayOneShot(menuMan.GetComponent<GlobalMenu>().ClickSFX);
            Img.SetActive(false);
            if (MenuID != -1)
            {
                buttonText.color = Color.yellow;
            }
            menuMan.GetComponent<GlobalMenu>().ClickedButton(MenuID);
        }
    }

    void OnDisable()
    {
        Img.SetActive(false);
        buttonText.color = Color.white;
    }
}
