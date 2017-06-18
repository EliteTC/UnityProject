using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabbit : MonoBehaviour
{
    public float speed = 1.5f;
    public static HeroRabbit lastRabit = null;
    void Awake()
    {
        lastRabit = this;
    }
    Rigidbody2D myBody = null;
    bool isGrounded = false;
    bool JumpActive = false;
   public bool isBig = false;
    float JumpTime = 0f;
    public float MaxJumpTime = 2f;
    public float JumpSpeed = 2f;
    Transform heroParent = null;
    bool isDead = false;

    public AudioClip deathSound = null;
    public AudioClip goSound = null;
    public AudioClip jumpSound = null;
    AudioSource deathSource = null;
    AudioSource goSource = null;
    AudioSource jumpSource = null;
    public int healthLimit = 2;
    int currentHealth = 1;

    public void enterRageMode()
    {
        if (!isBig)
        {
            becomeBig();
        }
    }
    public void die()
    {
        isDead = true;
        
            if (SoundManager.Instance.isSoundOn())
            {
                deathSource.Play();
            }
        
            StartCoroutine(this.rdie(0.5f));
    }
    
   public IEnumerator rdie(float duration)
    {
        //Perform action ...
        //Wait
        GetComponent<Animator>().SetBool("dead", true);
      
        currentHealth = 1;
        yield return new WaitForSeconds(duration);
        GetComponent<Animator>().SetBool("dead", false);
        LevelController.current.onRabbitDeath(this);
      

        //Continue excution in few seconds
        //Other actions...
    }
    public void exitRageMode()
    {
        if (isBig)
            becomeSmall();
    }
    void becomeBig()
    {
        this.transform.localScale *= 2;
        isBig = true;
    }
    void becomeSmall()
    {
        isBig = false;
        this.transform.localScale *= 0.5f;
    }
    // Use this for initialization
    void Start()
    {
        isDead = false;
        LevelController.current.setStartPosition(this.transform.position);
        // GetComponent<Animator>().SetBool("dead", false);
        myBody = this.GetComponent<Rigidbody2D>();
        this.heroParent = this.transform.parent;
        this.deathSource = gameObject.AddComponent<AudioSource>();
        this.deathSource.clip = deathSound;
        this.goSource = gameObject.AddComponent<AudioSource>();
        this.goSource.spatialBlend = 1;
        this.goSource.clip = goSound;
        this.jumpSource = gameObject.AddComponent<AudioSource>();
        this.jumpSource.clip = jumpSound;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 from = transform.position + Vector3.up * 0.3f;
        Vector3 to = transform.position + Vector3.down * 0.1f;
        int layer_id = 1 << LayerMask.NameToLayer("Ground");
        //Перевіряємо чи проходить лінія через Collider з шаром Ground
        RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
        if (hit)
        {
            if (!isGrounded && (SoundManager.Instance.isSoundOn())) jumpSource.Play();
            isGrounded = true;
            //Перевіряємо чи ми опинились на платформі
            if (hit.transform != null && hit.transform.GetComponent<MovingPlatform>() != null)
            {
                //Приліпаємо до платформи
                SetNewParent(this.transform, hit.transform);
            }
        }
        else
        {
            if (isGrounded && SoundManager.Instance.isSoundOn())  jumpSource.Play();
            isGrounded = false;
            //Ми в повітрі відліпаємо під платформи
            SetNewParent(this.transform, this.heroParent);
        }
       
        
        //Намалювати лінію (для розробника)
        Debug.DrawLine(from, to, Color.red);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            this.JumpActive = true;
        }
        if (this.JumpActive)
        {
            //Якщо кнопку ще тримають
            if (Input.GetButton("Jump"))
            {
                this.JumpTime += Time.deltaTime;
                if (this.JumpTime < this.MaxJumpTime)
                {
                    Vector2 vel = myBody.velocity;
                    vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
                    myBody.velocity = vel;
                }
            }
            else
            {
                this.JumpActive = false;
                this.JumpTime = 0;
            }
        }
        Animator animator = GetComponent<Animator>();
        if (this.isGrounded)
        {
            animator.SetBool("jump", false);
        }
        else
        {
            animator.SetBool("jump", true);
        }
        //[-1, 1]
        float value = Input.GetAxis("Horizontal");
       
        if (Mathf.Abs(value) > 0)
        {
            if (SoundManager.Instance.isSoundOn() && !goSource.isPlaying && isGrounded)
            {
                goSource.Play();
            }
            else if (!isGrounded)
            {
                goSource.Stop();
            }
            animator.SetBool("run", true);
        }
        else
        {
            if (SoundManager.Instance.isSoundOn())
            {
                goSource.Stop();
            }
            animator.SetBool("run", false);
        }
        if (Mathf.Abs(value) > 0)
        {
            Vector2 vel = myBody.velocity;
            vel.x = value * speed;
            myBody.velocity = vel;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (value < 0)
        {
            sr.flipX = true;
        }
        else if (value > 0)
        {
            sr.flipX = false;
        }
    }
    static void SetNewParent(Transform obj, Transform new_parent)
    {
        if (obj.transform.parent != new_parent)
        {
            //Засікаємо позицію у Глобальних координатах
            Vector3 pos = obj.transform.position;
            //Встановлюємо нового батька
            obj.transform.parent = new_parent;
            //Після зміни батька координати кролика зміняться
            //Оскільки вони тепер відносно іншого об’єкта
            //повертаємо кролика в ті самі глобальні координати
            obj.transform.position = pos;
        }
    }
    public void addOneHealth()
    {
        if (currentHealth < healthLimit)
        {
            currentHealth++;
        }
        if (currentHealth == healthLimit)
        {
            
        }
    }

    public void removeOneHealth()
    {
            if (currentHealth > 1)
            {
                currentHealth--;
               
            }
            else
            {
            this.die();
            }
        
    }
    /*
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (!this.isDead)
        {
            Orc1 orc = collider.gameObject.GetComponent<Orc1>();
            Orc2 orc2 = collider.gameObject.GetComponent<Orc2>();
            if (orc != null)
            {
                if (!orc.isOrcDead())
                {
                    if(collider == orc.bodyCollider)
                    {
                        orc.GetComponent<Animator>().SetTrigger("attack");
                        this.die();
                    } else if(collider == orc.headCollider)
                    {
                        orc.die();
                    }
                }
            }

            if (orc2 != null)
            {
                if (!orc2.isOrcDead())
                {
                    if (collider == orc2.bodyCollider)
                    {
                       // orc2.GetComponent<Animator>().SetTrigger("attack");
                      //  this.die();
                    }
                    else if (collider == orc2.headCollider)
                    {
                        orc2.die();
                    }
                }
            }
        }
    }
    */
}
