using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Vector3 MoveBy;
    Vector3 pointA;
    Vector3 pointB;
    public Vector3 speed;
    public float pause;
    private float time_to_wait;


    private bool going_to_a = true;

    // Use this for initialization
    void Start()
    {
        this.pointA = this.transform.position;
        this.pointB = this.pointA + MoveBy;

    }


    bool isArrived(Vector3 pos, Vector3 target)
    {
        pos.z = 0;
        target.z = 0;
        return Vector3.Distance(pos, target) < 0.2f;
    }




    void Update()
    {

        if (time_to_wait > 0)
            time_to_wait -= Time.deltaTime;
        if (time_to_wait <= 0)
        {


            Vector3 my_pos = this.transform.position;
            Vector3 target;

            if (going_to_a)
            {
                target = this.pointB;
            }
            else
            {
                target = this.pointA;
            }
            Vector3 destination = target - my_pos;
            destination.z = 0;

            if (isArrived(my_pos, target))
            {
                going_to_a = !going_to_a;
                time_to_wait = pause;
            }
            else if (going_to_a)
                this.transform.position += speed * Time.deltaTime;
            else
                this.transform.position -= speed * Time.deltaTime;


        }


    }


}
