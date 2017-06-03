using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc2 : MonoBehaviour
{
    public float speed = 1.5f;

    Rigidbody2D myBody = null;
    bool isDead = false;
    Transform heroParent = null;
    Vector3 pointA;
    Vector3 pointB;
    public GameObject carrot;
    float last_carrot = 0;
    public float patrolDistance = 4;
    Mode mode;
    Mode prevMode;
    public BoxCollider2D headCollider;
    public BoxCollider2D bodyCollider;

    public void die()
    {
        isDead = true;
        this.mode = Mode.Dead;
        StartCoroutine(this.orcdie(3));
    }
    public bool isOrcDead()
    {
        return this.isDead;
    }
    public IEnumerator orcdie(float duration)
    {
        //Perform action ...
        //Wait
        foreach (BoxCollider2D collider in this.GetComponents<BoxCollider2D>())
        {
            collider.enabled = false;
        }
        Destroy(this.myBody);
        GetComponent<Animator>().SetTrigger("die");

        yield return new WaitForSeconds(duration);

        Destroy(this.gameObject);

        // LevelController.current.onRabbitDeath(this);

        //Continue excution in few seconds
        //Other actions...
    }

    // Use this for initialization
    void Start()
    {
        mode = Mode.GoToB;
        pointA = this.transform.position;
        pointB = pointA;
        if (patrolDistance < 0)
        {
            pointA.x += patrolDistance;
        }
        else
        {
            pointB.x += patrolDistance;
        }


        // GetComponent<Animator>().SetBool("dead", false);
        myBody = this.GetComponent<Rigidbody2D>();
        this.heroParent = this.transform.parent;
        LevelController.current.setStartPosition(transform.position);
    }
    public enum Mode
    {
        GoToA,
        GoToB,
        CarrotLeft,
        CarrotRight,
        Dead

    }

    float getDirection()
    {
        Vector3 my_pos = this.transform.position;
        Vector3 rabit_pos = HeroRabbit.lastRabit.transform.position;


        if (this.mode == Mode.Dead)
        {
            return 0;
        }
        if(this.mode == Mode.CarrotLeft)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            return 0;
        } else if(this.mode == Mode.CarrotRight)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            return 0;
        }
        if (this.mode == Mode.GoToB)
        {

            if (my_pos.x >= pointB.x)
            {
                this.mode = Mode.GoToA;
            }
        }
        else if (this.mode == Mode.GoToA)
        {
            if (my_pos.x <= pointA.x)
            {
                this.mode = Mode.GoToB;
            }
        }

        if (this.mode == Mode.GoToB)
        {
            if (my_pos.x <= pointB.x)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }
        else if (this.mode == Mode.GoToA)
        {
            if (my_pos.x >= pointA.x)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        return 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {


        Animator animator = GetComponent<Animator>();
        lauchCarrot();

        //[-1, 1]
        float value = this.getDirection();

        if (Mathf.Abs(value) > 0)
        {
            animator.SetBool("run", true);
        }
        else
        {
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
            sr.flipX = false;
        }
        else if (value > 0)
        {
            sr.flipX = true;
        }
    }

    void lauchCarrot()
    {
        if (!this.isDead)
        {
            Vector3 my_pos = this.transform.position;
            Vector3 rabit_pos = HeroRabbit.lastRabit.transform.position;
            if(Mathf.Abs(my_pos.x - rabit_pos.x) <= 8.0f)
            {
                if (rabit_pos.x < my_pos.x)
                {
                    this.mode = Mode.CarrotLeft;
                }
                else
                {
                    this.mode = Mode.CarrotRight;
                }

                if (Time.time - this.last_carrot > 4.0f)
                {
                    this.last_carrot = Time.time;
                    GameObject obj = GameObject.Instantiate(this.carrot);
                    obj.transform.position = my_pos + Vector3.up * 2f;
                    Carrot carrot = obj.GetComponent<Carrot>();
                    this.GetComponent<Animator>().SetTrigger("attack");
                    if (rabit_pos.x < my_pos.x)
                    {
                        carrot.launch(-1);
                    } else
                    {
                        carrot.launch(1);
                    }
                }
            }
            return;
        }
        if(this.mode == Mode.CarrotLeft || this.mode == Mode.CarrotRight)
        this.mode = Mode.GoToA;
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
}
