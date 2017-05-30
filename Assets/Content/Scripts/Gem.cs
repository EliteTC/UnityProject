using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Collectable {

    protected override void OnRabbitHit(HeroRabbit rabbit)
    {
        // Level.current.addGem(1);
        this.CollectedHide();
    }
}
