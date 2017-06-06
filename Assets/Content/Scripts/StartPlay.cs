using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartPlay : MonoBehaviour
{

    public MyButton playButton;

    void Start()
    {
        playButton.signalOnClick.AddListener(this.onPlay);
    }
    void onPlay()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
}
