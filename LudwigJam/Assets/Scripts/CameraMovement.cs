using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject Player;

    Vector2 currentPos;
    Vector2 lastPos;

    public bool FirstBoss = false;
    public bool FirstBossB = false;
    public bool FirstBoss2 = false;
    public int CamPos = 0;
    bool once = true;
    public GameObject FirstBossGO;
    public GameObject RealFirstBoss;
    public GameObject FallObj;
    public GameObject Fade;
    public GameObject StageOne;

    public GameObject SecondBossParticle;
    public GameObject SecondBossGO;
    public GameObject SecondBoss1GO;
    public GameObject SecondBoss2GO;

    public GameObject ThirdBossGo;
    public GameObject ThirdBoss1Go;
    public GameObject FakePad;

    [Header("Sections")]
    public Color AColor;
    public Color BColor;
    public Color CColor;
    public GameObject S1Particles;
    public GameObject S2Particles;
    public GameObject S3Particles;

    public int FallAmount = 0;

    bool Entertwo = true;
    bool Enterthree = true;

    void Start()
    {
        GameObject.Find("NamePlate").GetComponent<NamePlate>().ShowName(0);
    }

    void Update()
    {
        //Colors
        if (transform.position.y < 110)
        {
            Camera.main.GetComponent<Camera>().backgroundColor = AColor;
            S1Particles.SetActive(true);
            S2Particles.SetActive(false);
            S3Particles.SetActive(false);
        }

        if (transform.position.y < 230 && transform.position.y > 110)
        {
            Camera.main.GetComponent<Camera>().backgroundColor = BColor;
            S1Particles.SetActive(false);
            S2Particles.SetActive(true);
            S3Particles.SetActive(false);
        }

        if (transform.position.y == 110 && Player.GetComponent<PlayerMovement>().onGround)
        {
            if (Entertwo)
            {
                GameObject.Find("NamePlate").GetComponent<NamePlate>().ShowName(2);
                Entertwo = false;
            }
        }

        if (transform.position.y >= 230)
        {
            Camera.main.GetComponent<Camera>().backgroundColor = CColor;
            S1Particles.SetActive(false);
            S2Particles.SetActive(false);
            S3Particles.SetActive(true);

            if (Enterthree && Player.GetComponent<PlayerMovement>().onGround)
            {
                GameObject.Find("NamePlate").GetComponent<NamePlate>().ShowName(4);
                Enterthree = false;
            }
        }

        //Normal Cam
        if (!FirstBoss)
        {
            transform.position = new Vector2(0, Mathf.Round(Player.transform.position.y / 10) * 10);
        }

        if (lastPos != currentPos && !FirstBossB)
        {
            if (lastPos.y > currentPos.y)
            {
                if (!Player.GetComponent<PlayerMovement>().Ragdolled)
                {
                    print("Fall");
                    FallAmount++;
                    if (FallAmount == 2)
                    {
                        Player.GetComponent<PlayerMovement>().RagDoll();
                    }
                }
                lastPos = currentPos;
            }

            if (Player.GetComponent<PlayerMovement>().onGround)
            {
                lastPos = currentPos;
            }
        }

        currentPos = transform.position;

        //First Boss Encounter
        if (FirstBoss)
        {
            if (!FirstBoss2)
            {
                transform.Translate(Vector2.up * Time.deltaTime);
            }
        }

        if (CamPos != 0 && CamPos != -1)
        {
            if (CamPos == 1)
            {
                //Up
                FirstBoss = true;
                FirstBossB = true;
                FirstBoss2 = true;
                transform.Translate(Vector2.up * Time.deltaTime);
                if (once)
                {
                    Invoke("CamMove", 1.5F);
                    once = false;
                }
            }
            if (CamPos == 2)
            {
                //Stop
                if (once)
                {
                    Invoke("CamMove", 15.75F);
                    once = false;
                }
            }
            if (CamPos == 3)
            {
                //Down
                transform.Translate(Vector2.down * Time.deltaTime);
                if (once)
                {
                    Invoke("CamMove", 1.5F);
                    once = false;
                }
            }
            if (CamPos == 4)
            {
                //Exit
                FirstBoss = false;
                FirstBoss2 = false;
                Invoke("SetFall", 1);
                CamPos = 0;
            }
        }
    }

    void SetFall()
    {
        FirstBossB = false;
    }

    public void Cinema(float Delay, float Timer)
    {
        GetComponent<Cinmen>().EnterCinema(Delay, Timer);
    }

    public void FirstBossStart()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().SkipIntros)
        {
            Time.timeScale = 10.0F;
        }
        FirstBossGO.GetComponent<AudioSource>().Play();
        FallObj.GetComponent<ParticleSystem>().Play();
        Invoke("FirstDelay", 3.5F);
        Invoke("OnBoss", 9.75F);
    }

    public void StartFight()
    {
        Fade.GetComponent<Animator>().SetTrigger("Fade");
        Invoke("EnableAll", 0.25F);
        Time.timeScale = 1.0F;
    }
    void EnableAll()
    {
        Player.transform.position = new Vector2(0, -3.5F);
        Player.GetComponent<PlayerMovement>().CanMove = true;
        RealFirstBoss.SetActive(true);
        FirstBossGO.SetActive(false);
        StageOne.SetActive(true);
        FirstBoss = false;
        FirstBoss2 = false;
    }

    void FirstDelay()
    {
        FirstBoss = true;
    }

    void OnBoss()
    {
        FirstBoss2 = true;

        FirstBossGO.GetComponent<Animator>().SetTrigger("Wake");
    }

    //Squid Boss
    public void SecondBossStart()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().SkipIntros)
        {
            Time.timeScale = 10.0F;
        }
        SecondBossParticle.SetActive(true);
        Invoke("SpawnSquid", 8);
        Invoke("ActivateSquid", 15);
    }
    void SpawnSquid()
    {
        SecondBossGO.SetActive(true);
    }
    void ActivateSquid()
    {
        SecondBoss1GO.GetComponent<FollowThing>().enabled = true;
        SecondBoss2GO.GetComponent<Schmovin>().enabled = true;

        Player.GetComponent<PlayerMovement>().CanMove = true;

        Time.timeScale = 1.0F;
    }

    //Frog Boss
    public void ThirdBossStart()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().SkipIntros)
        {
            Time.timeScale = 10.0F;
        }
        Invoke("CamMove", 1.5F);
        ThirdBossGo.SetActive(true);
        FakePad.SetActive(false);
        ThirdBossGo.GetComponent<Animator>().SetTrigger("Start");
        Invoke("TBG", 21.5F);
        Invoke("CrazyBoi", 18.25F);
    }

    void CamMove()
    {
        CamPos++;
        once = true;
    }

    void CrazyBoi()
    {
        var no = S3Particles.GetComponent<ParticleSystem>().noise;
        no.enabled = true;
        no.strength = 5.15F;
        no.frequency = 2.35F;
        Invoke("CalmBoi", 2.9F);
    }
    void CalmBoi()
    {
        var no = S3Particles.GetComponent<ParticleSystem>().noise;
        no.enabled = true;
        no.strength = 0.2F;
        no.frequency = 0.5F;
    }

    void TBG()
    {
        ThirdBossGo.SetActive(false);
        ThirdBoss1Go.SetActive(true);
        Time.timeScale = 1.0F;
    }

    public void ShakeCam(float time)
    {
        Camera.main.GetComponent<Animator>().SetBool("Shake", true);
        Invoke("StopShake", time);
    }

    void StopShake()
    {
        Camera.main.GetComponent<Animator>().SetBool("Shake", false);
    }
}
