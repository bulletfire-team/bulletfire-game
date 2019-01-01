using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class TestSlider : MonoBehaviour {

    public LobbyManager manager;
    public Text text;
    public Slider slider;

    public void ChangeSlider ()
    {
        text.text = slider.value.ToString();
        manager.maxPlayers = (int)slider.value;
        manager.minPlayers = (int)slider.value;
    }
}
