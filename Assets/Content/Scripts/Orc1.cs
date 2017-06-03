using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc1 : MonoBehaviour
{
    public float speed = 1.5f;
    
    Rigidbody2D myBody = null;
    bool isDead = false;
    Transform heroParent = null;
    Vector3 pointA;
    Vector3 pointB;

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
        foreach(BoxCollider2D collider in this.GetComponents<BoxCollider2D>())
        {
            collider.enabled = false;
        }
       // Destroy(this.myBody);
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
        if(patrolDistance < 0)
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
        Attack,
        Dead
        
    }

    float getDirection()
    {
        if(mode != Mode.Attack)
        prevMode = mode;
        Vector3 rabit_pos = HeroRabbit.lastRabit.transform.position;
        Vector3 my_pos = this.transform.position;
        if (rabit_pos.x > Mathf.Min(pointA.x, pointB.x)
        && rabit_pos.x < Mathf.Max(pointA.x, pointB.x))
        {
          
            mode = Mode.Attack;
        } else
        {
            mode = prevMode;
        }
        if (mode == Mode.Attack)
        {
            //Move towards rabit
            if (my_pos.x < rabit_pos.x)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        if (this.mode == Mode.Dead)
        {
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
            
        } else if(this.mode == Mode.GoToA)
        {
            if(my_pos.x >= pointA.x)
            {
                return -1;
            } else
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
