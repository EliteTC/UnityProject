using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    protected virtual void OnRabbitHit(HeroRabbit rabit)
    {
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
       // if (!this.hideAnimation)
        {
            HeroRabbit rabit = collider.GetComponent<HeroRabbit>();
            if (rabit != null)
            {
                this.OnRabbitHit(rabit);
            }
        }
    }
    public void CollectedHide()
    {
        Destroy(this.gameObject);
    }
}

