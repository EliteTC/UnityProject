using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartPlay : MonoBehaviour
{

    public MyButton playButton;
    public GameObject settingsScreen;
    public MyButton settings;

    void Start()
    {
        settings.signalOnClick.AddListener(this.onSettings);
        playButton.signalOnClick.AddListener(this.onPlay);
    }
    void onSettings()
    {
        Time.timeScale = 0;
        GameObject obj = GameObject.Find("UI Root").AddChild(this.settingsScreen);

        obj.transform.position = this.transform.position;
        obj.transform.position += new Vector3(0.0f, 1.0f, 0.0f);

        SettingsPopUp settings = obj.GetComponent<SettingsPopUp>();
        
    }
    void onPlay()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
}
