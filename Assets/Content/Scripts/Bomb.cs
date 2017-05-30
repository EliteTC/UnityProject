using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable {

    protected override void OnRabbitHit(HeroRabbit rabbit)
    {
        // Level.current.addBombs(1);
        if (rabbit.isBig)
        {

            rabbit.exitRageMode();
        }
        else rabbit.die();
        this.CollectedHide();
    }
}
