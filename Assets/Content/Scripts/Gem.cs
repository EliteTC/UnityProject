using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Collectable {

    public static int count = 0;
    public int id;

    void Start()
    {
        id = count;
        ++count;

    }

    public static void setCountZero()
    {
        count = 0;
    }


    public int getId() { return id; }
    protected override void OnRabbitHit(HeroRabbit rabit)
    {
        LevelController.current.addGem(this);
        this.CollectedHide();
    }
}
