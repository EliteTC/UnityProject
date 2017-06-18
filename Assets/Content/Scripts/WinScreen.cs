using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {

    public MyButton background;
    public MyButton close;
    public MyButton replay;
    public MyButton next;
    public UILabel coins;
    public UILabel fruits;
    public UI2DSprite gemlabel;

    // Use this for initialization
    void Start () {
        Time.timeScale = 0;
        background.signalOnClick.AddListener(this.onClosePlay);
        close.signalOnClick.AddListener(this.onClosePlay);
        replay.signalOnClick.AddListener(this.onReplayPlay);
        next.signalOnClick.AddListener(this.onClosePlay);
        setsFilds();
    }

    private void setsFilds()
    {
        UI2DSprite[] crystals = gemlabel.transform.GetComponentsInChildren<UI2DSprite>();
       
        SpriteRenderer[] crystalsFromGame = LevelController.current.getCrystals();
        for(int i = 0; i < crystalsFromGame.Length; ++i)
        {
            crystals[i+1].sprite2D = crystalsFromGame[i].sprite;
        }

        coins.text = "+" + LevelController.current.getCoinsOnThisLevel();
        fruits.text = LevelController.current.getFruitCount().ToString();
    }

    private void onReplayPlay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(LevelController.current.currentLevelName);
    }

    private void onClosePlay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("ChooseLevel");
    }

}
