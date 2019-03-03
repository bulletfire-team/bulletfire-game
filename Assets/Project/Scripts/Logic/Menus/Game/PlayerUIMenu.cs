using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerUIMenu : MonoBehaviour {

    private bool isBuyMenuOpen = false;

    public GameObject buyMenu;

    public bool canOpen = true;

    public GameObject settingsMenu;
    public bool isSettingsMenuOpen = false;

    [Header("Chat System")]
    public TMP_InputField chatIF;
    public RectTransform chat;
    public ScrollRect chatScroll;
    public RectTransform scrollbar;
    public GameObject chatBut;
    public Color disableColor;
    public Color enableColor;
    private bool chatOpen = false;
    public Transform msgContainer;
    public GameObject msgItem;
    public string gameColor;
    public string squadColor;
    public string teamColor;
    public string playerColor;
    public TMP_Text groupTxt;
    private int currentGroup = 1;

    [Header("Leaderboard System")]
    public GameObject leaderboard;
    public Transform leaderboardContainer;
    public GameObject leaderboardEntry;

    public PlayerChat chatSystem;

    public Dictionary<string, GameLeaderboardEntry> players = new Dictionary<string, GameLeaderboardEntry>();

    #region Leaderboard
    public void SetPlayers(string[] red, string[] blue, string me)
    {
        foreach (Transform item in leaderboardContainer)
        {
            Destroy(item.gameObject);
        }
        players.Clear();

        foreach (string item in red)
        {
            GameObject o = Instantiate(leaderboardEntry, leaderboardContainer);
            o.GetComponent<GameLeaderboardEntry>().Init(item, true);
            players.Add(item, o.GetComponent<GameLeaderboardEntry>());
        }

        foreach (string item in blue)
        {
            GameObject o = Instantiate(leaderboardEntry, leaderboardContainer);
            o.GetComponent<GameLeaderboardEntry>().Init(item, false);
            players.Add(item, o.GetComponent<GameLeaderboardEntry>());
        }
    }

    public void AddAssist (string pseudo)
    {
        GameLeaderboardEntry entry = players[pseudo];
        entry.AddAssist();
    }

    public void AddKill (string pseudo)
    {
        GameLeaderboardEntry entry = players[pseudo];
        entry.AddKill();
    }

    public void AddDeath (string pseudo)
    {
        GameLeaderboardEntry entry = players[pseudo];
        entry.AddDeath();
    }
    #endregion

    void Update () {
        if (Input.GetKeyDown(KeyCode.M) && canOpen && !isSettingsMenuOpen)
        {
            
            isBuyMenuOpen = !isBuyMenuOpen;
            
            buyMenu.SetActive(isBuyMenuOpen);
            Cursor.visible = isBuyMenuOpen;
            if (isBuyMenuOpen)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && !isSettingsMenuOpen)
        {
            if(chatOpen)
            {
                if (chatIF.text == "")
                {
                    chatOpen = false;
                    // Close chat
                    chatSystem.GetComponent<PlayerInput>().canMove = true;
                    chat.sizeDelta = new Vector2(250, 150);
                    chatScroll.vertical = false;
                    scrollbar.sizeDelta = new Vector2(0, scrollbar.sizeDelta.y);
                    chatBut.SetActive(true);
                    chatIF.gameObject.SetActive(false);
                    chat.GetComponent<Image>().color = disableColor;
                    groupTxt.gameObject.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    // Send message
                    SendMsg();
                }
            }
            else
            {
                chatOpen = true;
                // Open chat
                chatSystem.GetComponent<PlayerInput>().canMove = false;
                chat.sizeDelta = new Vector2(250, 300);
                chatScroll.vertical = true;
                scrollbar.sizeDelta = new Vector2(5, scrollbar.sizeDelta.y);
                chatBut.SetActive(false);
                chatIF.gameObject.SetActive(true);
                groupTxt.gameObject.SetActive(true);
                chat.GetComponent<Image>().color = enableColor;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                chatIF.Select();
            }
            
        }

        if(Input.GetKeyDown(KeyCode.Tab) && chatOpen && !isSettingsMenuOpen)
        {
            currentGroup++;
            if (currentGroup > 4) currentGroup = 1;
            switch (currentGroup)
            {
                case 1:
                    groupTxt.text = "<color=\"" + gameColor + "\">[Partie]</color>";
                    break;
                case 2:
                    groupTxt.text = "<color=\"" + squadColor + "\">[Escouade]</color>";
                    break;
                case 3:
                    groupTxt.text = "<color=\"" + teamColor + "\">[Equipe]</color>";
                    break;
                case 4:
                    groupTxt.text = "<color=\"" + playerColor + "\">[Joueur]</color>";
                    break;

            }
        }

        if(Input.GetKeyDown(KeyCode.Tab) && !chatOpen && !isSettingsMenuOpen)
        {
            leaderboard.SetActive(true);
        }

        if(Input.GetKeyUp(KeyCode.Tab))
        {
            leaderboard.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSettingsMenuOpen = !isSettingsMenuOpen;
            settingsMenu.SetActive(isSettingsMenuOpen);
            chatSystem.GetComponent<PlayerInput>().canMove = !isSettingsMenuOpen;
            Cursor.visible = isSettingsMenuOpen;
            Cursor.lockState = isSettingsMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
	}

    public void closeMenu ()
    {
        isBuyMenuOpen = false;
    }

    public void CloseSettingsMenu ()
    {
        isSettingsMenuOpen = false;
        chatSystem.GetComponent<PlayerInput>().canMove = !isSettingsMenuOpen;
        Cursor.visible = isSettingsMenuOpen;
        Cursor.lockState = isSettingsMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    #region ChatSystem
    public void ReceiveMsg(string msg, int type, bool isMe, string pseudo)
    {
        GameObject obj = Instantiate(msgItem, msgContainer);
        obj.GetComponent<ChatItem>().Init(msg, type, isMe, pseudo);
    }

    private void SendMsg ()
    {
        switch (currentGroup)
        {
            case 1:
                // Send to all
                chatSystem.CmdSendAll(chatIF.text);
                break;
            case 2:
                // Send to squad
                chatSystem.CmdSendAll(chatIF.text);
                break;
            case 3:
                // Send to team
                chatSystem.CmdSendTeam(chatIF.text);
                break;
            case 4:
                // Send to player
                chatSystem.CmdSendAll(chatIF.text);
                break;

        }
        chatIF.text = "";
        chatIF.Select();
    }
    #endregion
}
