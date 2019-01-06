using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour {

    public TextMeshProUGUI lifeCount;

    public TextMeshProUGUI redPlayers;
    public TextMeshProUGUI bluePlayers;

    public TextMeshProUGUI redScore;
    public TextMeshProUGUI blueScore;

    public TextMeshProUGUI nbPlayerLeft;

    public Transform killFeedContainer;

    public TextMeshProUGUI timerTxt;

    public GameObject reticle;

    public GameObject sniperScope;

    public Image blood;

    public GameObject buyMenu;

    public GameObject victoryTxt;
    public GameObject loseTxt;

    public GameObject hitMarker;

    public DamageIndicator damageIndicatorScript;

    public GameObject killfeed;
    public GameObject chat;

    public GameObject waitPanel;

    public TMP_Text respawnTxt;

    public Image team;

    public GameObject[] grenades;
    public GameObject[] smokeGrenades;

    public GameObject emoteMenu;

    [Header("Weapons")]
    public TextMeshProUGUI firstName;
    public TextMeshProUGUI secondName;
    public TextMeshProUGUI firstAmmo;
    public TextMeshProUGUI secondAmmo;
    public Image firstIcon;
    public Image secondIcon;

    private void Start()
    {
        GameplaySettings settings = GameObject.Find("GameplayManager").GetComponent<GameplaySettings>();
        killfeed.SetActive(settings.settings.showKillfeed);
        chat.SetActive(settings.settings.showChat);
    }

    public void ModifGameplaySettings ()
    {
        GameplaySettings settings = GameObject.Find("GameplayManager").GetComponent<GameplaySettings>();
        killfeed.SetActive(settings.settings.showKillfeed);
        chat.SetActive(settings.settings.showChat);
    }
}
