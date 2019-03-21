using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using UnityEngine.SceneManagement;


public class TestSlider : MonoBehaviour {

    public LobbyManager manager;
    public Text text;
    public Slider slider;
    public Toggle toggle;
    public string demo;
    public string real;

    public void ChangeSlider ()
    {
        text.text = slider.value.ToString();
        manager.maxPlayers = (int)slider.value;
        manager.minPlayers = (int)slider.value;
    }

    public void ChangeToggle ()
    {
        if(toggle.isOn)
        {
            manager.playScene = demo;
        }
        else
        {
            manager.playScene = real;
        }
    }
}
