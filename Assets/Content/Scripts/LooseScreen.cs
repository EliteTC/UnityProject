﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LooseScreen : MonoBehaviour {

    public MyButton background;
    public MyButton close;
    public MyButton replay;
    public MyButton menu;
  
    public UI2DSprite gemlabel;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 0;
        background.signalOnClick.AddListener(this.onClosePlay);
        close.signalOnClick.AddListener(this.onClosePlay);
        replay.signalOnClick.AddListener(this.onReplayPlay);
        menu.signalOnClick.AddListener(this.onClosePlay);
        setsFilds();
    }

    private void setsFilds()
    {
        UI2DSprite[] crystals = gemlabel.transform.GetComponentsInChildren<UI2DSprite>();
   
        SpriteRenderer[] crystalsFromGame = LevelController.current.getCrystals();
        for (int i = 0; i < crystalsFromGame.Length; ++i)
        {
            crystals[i + 1].sprite2D = crystalsFromGame[i].sprite;
        }

       
    }

    private void onReplayPlay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(LevelController.current.currentLevelName);
    }

    private void onClosePlay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

}
