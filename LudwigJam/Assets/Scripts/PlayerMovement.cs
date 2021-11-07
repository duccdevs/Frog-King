using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    private float moveSpeedDefault = 10f;
    public Vector2 direction;
    public bool facingRight = true;
    public bool facingRightOld;
    public bool CanMove = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    public bool DoubleJump = false;
    public bool LeapAbility = false;
    public bool FloatAbility = false;
    public int DoubleJumpAmount = 2;
    bool CanDoubleJump = false;
    bool CanLeap = false;
    float FloatAbilityTime = 1.375F;
    bool Floating = false;

    [Header("QOL")]
    private float jumpTimer;
    [SerializeField]
    public float fJumpPressedRemember = 0.0F;
    [SerializeField]
    public float fJumpPressedRememberTime = 0.1F;
    [SerializeField]
    public float fGroundedRemember = 0.0F;
    [SerializeField]
    public float fGroundedRememberTime = 0.2F;

    private float fWJumpPressedRemember = 0.0F;
    private float fWJumpPressedRememberTime = 0.5F;

    [Header("Components")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    [Header("Physics")]
    private float maxSpeed = 4.5F;
    private float maxSpeedNormal = 4.5F;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public bool onGroundDelay = false;
    bool EnteredGround = false;
    public float groundLength = 0.6F;
    public float wallLenght = 0.6F;
    public Vector3 colliderOffset;
    bool onGroundOnce = true;

    [Header("Misc")]
    public bool Ragdolled = false;
    public bool DJPlat = false;
    public bool FloatPlat = false;
    public bool LeapPlat = false;
    public PhysicsMaterial2D normalPhysics;
    public PhysicsMaterial2D bouncePhysics;
    public GameObject CamHolder;
    public Animator animatorWalk;
    public Animator animatorBody;
    public Animator animatorHands;
    public GameObject DJEffOne;
    public GameObject DJEffTwo;
    public GameObject Cheese;
    bool djBool = true;
    //public GameObject GM;
    public ParticleSystem JumpDust;
    public ParticleSystem dustLand;
    public ParticleSystem dustSide1;
    public ParticleSystem dustSide2;
    public ParticleSystem dustJump;
    public ParticleSystem dustJumpAir;
    public ParticleSystem GetHit;
    public ParticleSystem GetHitInk;
    public ParticleSystem FloatHold;
    public ParticleSystem FloatParticles;
    public ParticleSystem LeapParticles;
    public ParticleSystem LeapJump;
    public ParticleSystem LeapDust;
    bool Frogged = false;
    GameObject ThisFrog = null;
    bool OnceFloat = true;
    bool Inked = false;
    public GameObject[] HideParts;
    public bool InFrog = false;
    public GameObject Mouth;
    public GameObject Mouth2;
    public GameObject Bag;
    public bool RightFlipped = false;
    public GameObject Crown;
    public GameObject FrogPet;
    public GameObject FrogPetPos;
    public GameObject GodObj;
    public GameObject ShowJumpies;
    bool TutJump = true;
    bool TutSpit = true;

    [Header("Sounds")]
    public AudioSource audioOne;
    public AudioClip GettingPower;

    private void Start()
    {
        colliderOffset.x = 0.401F;
        moveSpeedDefault = moveSpeed;
        facingRightOld = facingRight;
        rb.sharedMaterial = normalPhysics;

        if (PlayerPrefs.GetInt("Won") == 1)
        {
            Crown.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Frog") == 1)
        {
            FrogPet.SetActive(true);
        }

        if (PlayerPrefs.GetInt("God") == 1)
        {
            GodObj.SetActive(true);
        }
    }

    void Update()
    {
        CheckPlats();

        //Collisions
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(
            transform.position + colliderOffset,
            Vector2.down, groundLength, groundLayer)
            || Physics2D.Raycast(transform.position - colliderOffset,
            Vector2.down, groundLength, groundLayer) ||
            Physics2D.Raycast(transform.position,
            Vector2.down, groundLength, groundLayer);

        //Inputs
        //jump fall idle anim
        float checkSpeedY;
        checkSpeedY = rb.velocity.y;

        if (Frogged && direction.y <= -0.5F && ThisFrog != null)
        {
            if (!ThisFrog.GetComponent<AudioSource>().isPlaying)
            {
                ThisFrog.GetComponent<AudioSource>().Play();
                ThisFrog.GetComponent<Animator>().SetTrigger("Note");
            }
        }

        if (CanMove && !Ragdolled)
        {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            direction = Vector2.zero;
        }

        if (onGround)
        {
            if (onGroundOnce)
            {
                fGroundedRemember = fGroundedRememberTime;
                onGroundOnce = false;
            }
        }
        else
        {
            onGroundOnce = true;
            fGroundedRemember -= Time.deltaTime;
        }

        if (fGroundedRemember > 0)
        {
            onGroundDelay = true;
        }
        else
        {
            if (!onGround)
            {
                onGroundDelay = false;
            }
            else
            {
                onGroundDelay = true;
            }
        }

        //GroundJump
        if (jumpTimer > Time.time)
        {
            Jump();
        }

        //Normal Jump
        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
            //print("E");
        }

        if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0))
        {
            fJumpPressedRemember = 0;
            jumpTimer = Time.time + jumpDelay;
        }
        
        //DoubleJump
        if (!onGroundDelay)
        {
            if (CanDoubleJump && !Ragdolled)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    jumpTimer = Time.time + jumpDelay;
                }
            }
        }

        if (!Ragdolled)
        {
            if (!onGroundDelay && !onGround && FloatAbilityTime > 0 && FloatAbility)
            {
                if (Input.GetButton("Jump") && rb.velocity.y <= 1F)
                {
                    if (OnceFloat)
                    {
                        FloatParticles.Play();
                        OnceFloat = false;
                    }
                    Floating = true;
                }
            }

            if (Floating)
            {
                linearDrag = 1;
                rb.drag = linearDrag;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                gravity = 0;


                FloatAbilityTime -= Time.deltaTime;

                if (Input.GetButtonUp("Jump") || FloatAbilityTime <= 0.0F)
                {
                    linearDrag = 16;
                    rb.drag = linearDrag;
                    gravity = 2.5F;
                    FloatParticles.Stop();

                    Floating = false;
                    OnceFloat = true;
                }
            }

            if (FloatAbility)
            {
                FloatHold.emissionRate = FloatAbilityTime * 50;
            }
            else
            {
                FloatHold.emissionRate = 0;
            }

            if (!onGroundDelay && LeapAbility && CanLeap && Input.GetButtonDown("Jump"))
            {
                DoLeap();
                CanLeap = false;
                return;
            }
        }

        if (!LeapAbility)
        {
            LeapParticles.emissionRate = 0;
        }

        if (Ragdolled && rb.velocity.magnitude <= 0.35F && onGround && !Inked && !InFrog)
        {
            Ragdolled = false;
            UnRagdoll();
        }

        if (Ragdolled && rb.velocity == Vector2.zero && !Inked && !InFrog)
        {
            Ragdolled = false;
            UnRagdoll();
        }

        if (DoubleJump || LeapAbility || FloatAbility)
        {
            animatorHands.SetBool("Hold", true);
        }
        else
        {
            animatorHands.SetBool("Hold", false);
        }

        if (Ragdolled && rb.velocity.y > 0)
        {
            Cheese.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            Cheese.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (DoubleJump)
        {
            if (!DJEffOne.GetComponent<ParticleSystem>().loop && djBool)
            {
                Invoke("DJStuff", 0.65F);
                djBool = false;
            }

            if (DoubleJumpAmount == 2)
            {
                DJEffOne.GetComponent<ParticleSystem>().emissionRate = 50;
                DJEffOne.GetComponent<ParticleSystem>().startLifetime = 0.5F;
            }

            if (DoubleJumpAmount == 1)
            {
                DJEffOne.GetComponent<ParticleSystem>().emissionRate = 25;
                DJEffOne.GetComponent<ParticleSystem>().startLifetime = 0.25F;
            }
        }
        else
        {
            DJEffOne.GetComponent<ParticleSystem>().loop = false;
            DJEffOne.GetComponent<ParticleSystem>().Stop();
            DJEffTwo.GetComponent<ParticleSystem>().loop = false;
            DJEffTwo.GetComponent<ParticleSystem>().Stop();

            djBool = true;
        }
    }

    void DJStuff()
    {
        if (!DJEffOne.GetComponent<ParticleSystem>().loop)
        {
            DJEffOne.GetComponent<ParticleSystem>().loop = true;
            DJEffOne.GetComponent<ParticleSystem>().Play();
        }

        if (!DJEffTwo.GetComponent<ParticleSystem>().loop)
        {
            DJEffTwo.GetComponent<ParticleSystem>().loop = true;
            DJEffTwo.GetComponent<ParticleSystem>().Play();
        }
    }

    void DoLeap()
    {
        CanMove = false;
        linearDrag = 0;
        rb.drag = linearDrag;
        rb.velocity = Vector2.zero;
        CamHolder.GetComponent<CameraMovement>().ShakeCam(0.115F);
        LeapJump.Play();
        LeapDust.Play();
        LeapParticles.emissionRate = 0;

        rb.AddForce(Vector2.up * 5.77F, ForceMode2D.Impulse);

        if (facingRight)
        {
            rb.AddForce(Vector2.right * 3.175F, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.left * 3.175F, ForceMode2D.Impulse);
        }
    }

    public void RagDoll()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GotHit++;
        Ragdolled = true;
        rb.sharedMaterial = bouncePhysics;
        CanMove = false;
        linearDrag = 1.35F;
        rb.drag = linearDrag;
        foreach(GameObject go in HideParts)
        {
            go.transform.rotation = Quaternion.Euler(0, go.transform.rotation.y, 90);
        }
    }

    void UnRagdoll()
    {
        CamHolder.GetComponent<CameraMovement>().FallAmount = 0;
        transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y * 2) / 2);
        rb.velocity = Vector2.zero;
        rb.sharedMaterial = normalPhysics;
        CanMove = true;
        linearDrag = 16;
        rb.drag = linearDrag;

        foreach(GameObject go in HideParts)
        {
            if (facingRight)
            {
                go.transform.rotation = Quaternion.identity;
            }
            else
            {
                go.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void FixedUpdate()
    {
        moveCharacter(direction.x);
        modifyPhysics();
    }

    void moveCharacter(float horizontal)
    {
        if (CanMove && !Ragdolled)
        {
            rb.AddForce(Vector2.right * horizontal * moveSpeed);

            if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
            {
                Flip();
            }
            if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
            }
        }

        if (onGround && rb.velocity.magnitude >= 0.25F)
        {
            animatorWalk.SetBool("Walk", true);
        }
        else
        {
            animatorWalk.SetBool("Walk", false);
        }
    }

    void Jump()
    {
        if (CanMove)
        {
            CreateDustJmp();
            colliderOffset.x = 0.0F;
            Invoke("ResetOffset", 0.21F);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

            if (CanDoubleJump && fGroundedRemember < 0 && DoubleJumpAmount > 0 && !Ragdolled)
            {
                //DoubleJump
                if (TutJump)
                {
                    ShowJumpies.SetActive(true);
                    Destroy(ShowJumpies, 6.5F);
                    TutJump = false;
                }
                DoubleJumpAmount--;
                CamHolder.GetComponent<CameraMovement>().ShakeCam(0.125F);
                if (DoubleJumpAmount <= 0)
                {
                    DoubleJump = false;
                    CanDoubleJump = false;
                }
                dustJumpAir.Play();
                dustJumpAir.transform.GetComponent<AudioSource>().Play();
                fGroundedRemember = 0;
                jumpTimer = 0;
                return;
            }
            else
            {
                if (!Floating)
                {
                    //Single Jump
                    dustJump.Play();
                    fGroundedRemember = 0;
                    jumpTimer = 0;
                    return;
                }
            }
        }
    }

    void CheckPlats()
    {
        if (DJPlat)
        {
            if (onGround && direction.y <= -0.5F)
            {
                //Get DoubleJump
                StartSummon();
                DoubleJump = true;
                CanDoubleJump = true;
                DoubleJumpAmount = 2;
                FloatAbility = false;
                LeapAbility = false;
            }
        }

        if (FloatPlat)
        {
            if (onGround && direction.y <= -0.5F)
            {
                //Get Float
                StartSummon();
                DoubleJump = false;
                CanDoubleJump = false;
                DoubleJumpAmount = 0;
                LeapAbility = false;
                FloatAbility = true;
                FloatAbilityTime = 1.375F;
            }
        }

        if (LeapPlat)
        {
            if (onGround && direction.y <= -0.5F)
            {
                //Get Leap
                StartSummon();
                DoubleJump = false;
                CanDoubleJump = false;
                DoubleJumpAmount = 0;
                FloatAbility = false;

                LeapAbility = true;
                LeapParticles.emissionRate = 45;
                CanLeap = true;
            }
        }
    }

    void StartSummon()
    {
        direction.y = 0;
        CanMove = false;
        animatorBody.SetTrigger("Yoink");
        animatorHands.SetTrigger("Yoink");
        CamHolder.GetComponent<CameraMovement>().ShakeCam(0.65F);
        audioOne.clip = GettingPower;
        audioOne.Play();

        Invoke("StopSummon", 0.75F);
    }

    void StopSummon()
    {
        CanMove = true;
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.layer == 8 && Ragdolled)
        {
            CamHolder.GetComponent<CameraMovement>().ShakeCam(0.25F);
        }
    }

    void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "DoubleJump" && transform.position.y > (collision2D.transform.position.y + 0.25F))
        {
            DJPlat = true;
        }

        if (collision2D.gameObject.tag == "Float" && transform.position.y > (collision2D.transform.position.y + 0.25F))
        {
            FloatPlat = true;
        }
        if (collision2D.gameObject.tag == "Leap" && transform.position.y > (collision2D.transform.position.y + 0.25F))
        {
            LeapPlat = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "DoubleJump")
        {
            DJPlat = false;
        }
        if (collision2D.gameObject.tag == "Float")
        {
            FloatPlat = false;
        }
        if (collision2D.gameObject.tag == "Leap")
        {
            LeapPlat = false;
        }
    }

    void ResetOffset()
    {
        colliderOffset.x = 0.401F;
    }

    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround)
        {
            rb.drag = linearDrag;
            maxSpeed = maxSpeedNormal;
            rb.gravityScale = 0;

            if (EnteredGround)
            {
                CamHolder.GetComponent<CameraMovement>().FallAmount = 0;

                FrogPetPos.transform.position = transform.position;
                if (FrogPet.GetComponent<FrogPet>().JumpAgain)
                {
                    FrogPet.GetComponent<FrogPet>().SetPos();
                }

                if (CanMove)
                {
                    rb.sharedMaterial = normalPhysics;
                    transform.position = new Vector2(transform.position.x, Mathf.Round(transform.position.y * 2) / 2);
                }

                CreateDustLand();
                if (DoubleJump)
                {
                    CanDoubleJump = true;
                }
                if (LeapAbility)
                {
                    linearDrag = 16;
                    rb.drag = linearDrag;
                    CanMove = true;

                    if (!CanLeap)
                    {
                        LeapAbility = false;
                    }
                }
                EnteredGround = false;
            }
        }
        else
        {
            rb.gravityScale = gravity;
            if (CanMove && !Ragdolled)
            {
                if (rb.velocity.y < 0)
                {
                    rb.gravityScale = gravity * fallMultiplier * 1.25F;
                }
            }

            EnteredGround = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Frogm")
        {
            Frogged = true;
            ThisFrog = collider2D.gameObject;
            ThisFrog.GetComponent<IAmFrogme>().Frogme();
        }

        if (collider2D.tag == "Boss")
        {
            print("Monke Boss Time");
            collider2D.gameObject.SetActive(false);
            FirstBoss();
        }

        if (collider2D.tag == "SquidBoss")
        {
            print("Squid Boss Time");
            collider2D.gameObject.SetActive(false);
            SecondBoss();
        }

        if (collider2D.tag == "FrogBoss")
        {
            print("Frog Boss Time");
            collider2D.gameObject.SetActive(false);
            ThirdBoss();
        }

        if (collider2D.tag == "LFist" && !Ragdolled)
        {
            RagDoll();
            GetHitEffect();
            rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            rb.AddForce(Vector2.right * 8, ForceMode2D.Impulse);
            collider2D.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (collider2D.tag == "RFist" && !Ragdolled)
        {
            RagDoll();
            GetHitEffect();
            rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            rb.AddForce(Vector2.left * 8, ForceMode2D.Impulse);
            collider2D.GetComponent<BoxCollider2D>().enabled = false;
        }

        if (collider2D.tag == "Ink" && !Ragdolled)
        {
            if (!onGround)
            {
                Inked = true;
                Invoke("UnInk", 0.1F);
            }

            GetHitEffectInk();
            Destroy(collider2D.gameObject);
        }

        if (collider2D.tag == "Tongue" && !Ragdolled)
        {
            InsideFrog();
            CamHolder.GetComponent<CameraMovement>().ShakeCam(0.25F);
            GetHit.transform.GetComponent<AudioSource>().Play();
        }
    }

    void UnInk()
    {
        Inked = false;
    }

    void InsideFrog()
    {
        InFrog = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        CanMove = false;

        LeapAbility = false;
        CanLeap = false;
        DoubleJump = false;
        CanDoubleJump = false;
        FloatAbility = false;

        Invoke("InsideFrogTwo", 1F);
        Invoke("SpitOut", 2.875F);
    }
    void InsideFrogTwo()
    {
        transform.position = Mouth.transform.position;
        Bag.SetActive(true);
        CamHolder.GetComponent<CameraMovement>().ShakeCam(5F);

        foreach(GameObject go in HideParts)
        {
            go.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    void SpitOut()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GotHit++;
        Bag.SetActive(false);
        Mouth2.SetActive(true);
        foreach(GameObject go in HideParts)
        {
            go.GetComponent<SpriteRenderer>().enabled = true;
        }
        rb.isKinematic = false;
        InFrog = false;
        RagDoll();
        if (TutSpit)
        {
            rb.AddForce(new Vector2(0, -5), ForceMode2D.Impulse);
            CamHolder.GetComponent<CameraMovement>().ShakeCam(0.2F);
            GetHit.transform.GetComponent<AudioSource>().Play();
            Invoke("CloseMouth", 0.75F);
            TutSpit = false;
            return;
        }
        else
        {
            if (transform.position.x < 0)
            {
                rb.AddForce(new Vector2(Random.Range(0, 50), Random.Range(-30, 30)), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(Random.Range(-50, 0), Random.Range(-30, 30)), ForceMode2D.Impulse);
            }

            CamHolder.GetComponent<CameraMovement>().ShakeCam(0.2F);
            GetHit.transform.GetComponent<AudioSource>().Play();
            Invoke("CloseMouth", 0.75F);
            return;
        }
    }
    void CloseMouth()
    {
        CamHolder.GetComponent<CameraMovement>().ShakeCam(0.1F);
        Mouth2.SetActive(false);
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.tag == "Frogm")
        {
            Frogged = false;
            ThisFrog = null;
        }
    }

    void FirstBoss()
    {
        CanMove = false;
        CamHolder.GetComponent<CameraMovement>().ShakeCam(9.75F);
        CamHolder.GetComponent<CameraMovement>().FirstBossStart();
        CamHolder.GetComponent<CameraMovement>().Cinema(5, 38);
    }

    void SecondBoss()
    {
        CanMove = false;
        CamHolder.GetComponent<CameraMovement>().ShakeCam(15.75F);
        CamHolder.GetComponent<CameraMovement>().SecondBossStart();
        CamHolder.GetComponent<CameraMovement>().Cinema(1.75F, 16);
    }

    void ThirdBoss()
    {
        LeapAbility = false;
        CanLeap = false;
        linearDrag = 16;
        rb.drag = linearDrag;
        CanMove = false;
        CamHolder.GetComponent<CameraMovement>().ThirdBossStart();
        CamHolder.GetComponent<CameraMovement>().Cinema(1.75F, 21.75F);
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);

        if (onGround)
        {
            if (facingRight)
            {
                DustSide1();
            }
            else
            {
                DustSide2();
            }
        }
    }

    void CreateDustJmp()
    {
        JumpDust.Play();
        JumpDust.transform.GetComponent<AudioSource>().Play();

        if (onGroundDelay)
        {
            //print("UR GAY LOL + L");
            //dustJump.Play();
        }
        else
        {
            //print("UR GAY LOL");
            //dustJumpAir.Play();
            //dustJumpAir.transform.GetComponent<AudioSource>().Play();
        }
    }
    void CreateDustLand()
    {
        dustLand.Play();
        dustLand.transform.GetComponent<AudioSource>().Play();
    }
    void DustSide1()
    {
        dustSide1.Play();
        dustSide1.transform.GetComponent<AudioSource>().Play();
    }
    void DustSide2()
    {
        dustSide2.Play();
        dustSide2.transform.GetComponent<AudioSource>().Play();
    }

    void GetHitEffect()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GotHit++;
        GetHit.Play();
        GetHit.transform.GetComponent<AudioSource>().Play();
        CamHolder.GetComponent<CameraMovement>().ShakeCam(0.2F);
    }

    void GetHitEffectInk()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().GotHit++;
        FloatAbilityTime = 0.0F;
        FloatAbility = false;
        FloatHold.emissionRate = 0;
        GetHitInk.Play();
        GetHitInk.transform.GetComponent<AudioSource>().Play();
        CamHolder.GetComponent<CameraMovement>().ShakeCam(0.15F);
    }
}
