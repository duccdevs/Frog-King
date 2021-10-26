using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Crown : MonoBehaviour
{
    bool InThing = false;
    public GameObject Player;
    public GameObject CrownHolder;
    public GameObject CamHolder;
    public GameObject GameManager;
    public bool WonGame = false;
    bool move = true;
    public Animator endCredits;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            InThing = true;
            Player = collider2D.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Player")
        {
            InThing = false;
        }
    }

    void Update()
    {
        if (Player != null && InThing && Player.GetComponent<PlayerMovement>().direction.y == -1 && !WonGame)
        {
            //EndGame
            print("WonGame");
            Player.GetComponent<PlayerMovement>().CanMove = false;
            CamHolder.GetComponent<CameraMovement>().Cinema(1.75F, 99);
            endCredits.SetTrigger("Roll");
            Invoke("EndMovie", 25);
            Invoke("EndCredits", 30);
            PlayerPrefs.SetInt("Won", 1);
            GameManager.GetComponent<GameManager>().BeatGame = true;
            WonGame = true;
        }

        if (WonGame)
        {
            if (move)
            {
                CrownHolder.transform.Translate(Vector2.up * (Time.deltaTime / 35));
            }
            Player.transform.position = new Vector2(transform.position.x, transform.position.y + (CrownHolder.transform.localPosition.y - 1));
        }
    }

    void EndMovie()
    {
        move = false;
    }

    void EndCredits()
    {
        SceneManager.LoadScene(0);
    }
}
