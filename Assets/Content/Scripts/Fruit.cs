using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Collectable {


    protected override void OnRabbitHit(HeroRabbit rabbit)
    {
        // Level.current.addFruit(1);
        this.CollectedHide();
    }
}
