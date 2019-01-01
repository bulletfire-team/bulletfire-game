using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public string team = "";
    [SyncVar]
    public string pseudo = "";
    public bool isDead = false;

    private int leftTime = 30;

    public Behaviour[] toFreeze;

    public GameObject redKillUI;
    public GameObject blueKillUI;

    public Color friendColor;

    private PlayerUI playerUI;
    private PlayerUIMenu playerUIMenu;

    private void Start()
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        playerUIMenu = GameObject.Find("UI").GetComponent<PlayerUIMenu>();
    }

    public void Init()
    {
        //FreezePlayer();
        
        foreach(Hitbox go in GetComponentsInChildren<Hitbox>())
        {
            go.gameObject.tag = team == "red" ? "RedTeam" : "BlueTeam";
        }
        if(team == "red")
        {
            GetComponent<PlayerWeapon>().ennemyTeam = "BlueTeam";
        }else if(team == "blue")
        {
            GetComponent<PlayerWeapon>().ennemyTeam = "RedTeam";
        }

        if(!isServer && isLocalPlayer) CmdChangeTag(team);
        
    }

    [TargetRpc]
    public void TargetGetMoney(NetworkConnection target, int mo)
    {
        PlayerAtributes playerAtributes = GetComponent<PlayerAtributes>();
        playerAtributes.money += mo;
        playerAtributes.ShowMoney();
    }
    

    [Command]
    public void CmdChangeTag(string t)
    {
        GetComponent<PlayerWeapon>().ennemyTeam = "RedTeam";
        foreach(Hitbox go in GetComponentsInChildren<Hitbox>())
        {
            go.gameObject.tag = "BlueTeam";
        }
    }

    #region TargetRpc
    [TargetRpc] 
    public void TargetGetPlayers (NetworkConnection target, string[] red, string[] blue)
    {
        GameObject.Find("UI").GetComponent<PlayerUIMenu>().SetPlayers(red, blue, pseudo);
    }

    [TargetRpc]
    public void TargetBeginRound (NetworkConnection target, int redScore, int blueScore, int nbRed, int nbBlue)
    {
        if (!isLocalPlayer) return;
        FreezePlayer();
        if (isDead)
        {
            print("Camera back");
            GetComponent<NetworkPlayer>().came.SetActive(true);
            GameObject.FindWithTag("DeadCamera").GetComponent<Camera>().enabled = false;
            playerUI.gameObject.SetActive(true);
        }

        // Reload weapons
        Transform weapContainer = GetComponent<NetworkPlayer>().weaponHolder;
        int a = 0;
        foreach (Transform item in weapContainer)
        {
            item.GetComponent<WeaponHolder>().NewRound(a == 0 ? playerUI.firstAmmo : playerUI.secondAmmo);
            a++;
        }
        // Reload grenades
        GetComponent<PlayerWeapon>().nbGrenade = 3;
        for(int i = 0; i < 3; i++)
        {
            playerUI.grenades[i].SetActive(true);
        }


        GameObject.Find("UI").GetComponent<PlayerUIMenu>().canOpen = true;
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().waitPanel.SetActive(false);

        // Reinit nb players
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().redPlayers.text = nbRed.ToString() + " joueurs en vie";
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().bluePlayers.text = nbBlue.ToString() + " joueurs en vie";

        // Update UI Score
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().redScore.text = redScore.ToString();
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().blueScore.text = blueScore.ToString();

        CmdRetakeLife();

        foreach(OneWayDoor o in FindObjectsOfType<OneWayDoor>())
        {
            o.Reinit();
        }
        
        leftTime = 30;
        isDead = false;
        // Respawn
        NetworkPlayer myNP = GetComponent<NetworkPlayer>();
        myNP.Respawn(true);
        // Start counter
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().timerTxt.gameObject.SetActive(true);
        StartCoroutine(waitStartRound());
    }

    [TargetRpc]
    public void TargetPlayerDied (NetworkConnection target, int playerRed, int playerBlue)
    {
        if (!isLocalPlayer) return;
        playerUI.redPlayers.text = playerRed.ToString() + " joueurs en vie";
        playerUI.bluePlayers.text = playerBlue.ToString() + " joueurs en vie";
        playerUI.nbPlayerLeft.text = playerRed.ToString() + " contre " + playerBlue.ToString();
        playerUI.nbPlayerLeft.gameObject.SetActive(true);
        StopCoroutine(waitUnshowCount());
        StartCoroutine(waitUnshowCount());
    }

    [TargetRpc]
    public void TargetPlayerGone(NetworkConnection target, string[] killer, string[] victim)
    {
        if (!isLocalPlayer) return;
        GameObject go = null;
        if(killer[0] == "red")
        {
            go = (GameObject)Instantiate(redKillUI, playerUI.killFeedContainer);
        }else if(killer[0] == "blue")
        {
            go = (GameObject)Instantiate(blueKillUI, playerUI.killFeedContainer);
        }
        if (go == null) return;
        go.transform.SetAsFirstSibling();
        go.GetComponentInChildren<TextMeshProUGUI>().text = victim[1] + " a été tué par " + killer[1];
        playerUIMenu.AddDeath(victim[1]);
        playerUIMenu.AddKill(killer[1]);
        StartCoroutine(waitClearKillFeed(go));
    }

    [TargetRpc]
    public void TargetNbPlayerChanged (NetworkConnection target, int playerRed, int playerBlue)
    {
        if (!isLocalPlayer) return;
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().redPlayers.text = playerRed.ToString() + " joueurs en vie";
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().bluePlayers.text = playerBlue.ToString() + " joueurs en vie";
    }

    [TargetRpc]
    public void TargetFirstPoint (NetworkConnection target,  Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        GetComponent<NetworkPlayer>().SetFirstTransform();
    }

    [TargetRpc]
    public void TargetPeopleOnMyTeam (NetworkConnection target, NetworkInstanceId[] mates)
    {
        foreach (NetworkInstanceId id in mates)
        {
            GameObject player = ClientScene.FindLocalObject(id);
            player.GetComponent<Player>().ShowPseudo();
        }
    }

    [TargetRpc]
    public void TargetEndGame (NetworkConnection target, bool isRedTeam)
    {
        FreezePlayer();
        if(team == "red" && isRedTeam || team == "blue" && !isRedTeam)
        {
            GameObject.Find("PlayerUI").GetComponent<PlayerUI>().victoryTxt.SetActive(true);
        }
        else
        {
            GameObject.Find("PlayerUI").GetComponent<PlayerUI>().loseTxt.SetActive(true);
        }
    }
    #endregion


    public void FreezePlayer ()
    {
        GetComponent<MyNetworkAudioManager>().CmdReinit();
        playerUI.damageIndicatorScript.indicator.SetActive(false);
        foreach(Behaviour b in toFreeze)
        {
            b.enabled = false;
        }
    }

    public void UnfreezePlayer ()
    {
        foreach(Behaviour b in toFreeze)
        {
            b.enabled = true;
        }
    }

    IEnumerator waitStartRound ()
    {
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().timerTxt.text = leftTime.ToString();
        yield return new WaitForSeconds(1);
        leftTime--;
        if(leftTime >= 0)
        {
            StartCoroutine(waitStartRound());
        }
        else
        {
            GameObject.Find("PlayerUI").GetComponent<PlayerUI>().timerTxt.gameObject.SetActive(false);
            GameObject.Find("PlayerUI").GetComponent<PlayerUI>().buyMenu.SetActive(false);
            GameObject.Find("UI").GetComponent<PlayerUIMenu>().canOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CmdRetakeHealth();
            UnfreezePlayer();
        }
    }

    IEnumerator waitClearKillFeed (GameObject obj)
    {
        yield return new WaitForSeconds(3f);
        Destroy(obj);
    }

    IEnumerator waitUnshowCount ()
    {
        yield return new WaitForSeconds(2f);
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().nbPlayerLeft.gameObject.SetActive(false);
    }

    [Command]
    public void CmdRetakeHealth ()
    {
        NetworkPlayer myNP = GetComponent<NetworkPlayer>();
        myNP.health = GetComponent<PlayerAtributes>().equipmentManager.GetResistance() + 100;
    }

    [Command]
    public void CmdRetakeLife ()
    {
        // Reset player attributes
        NetworkPlayer myNP = GetComponent<NetworkPlayer>();
        myNP.life = 3;
    }

    public void ShowPseudo()
    {
        if (isLocalPlayer) return;
        PlayerAtributes pa = GetComponent<PlayerAtributes>();
        pa.pseudoTxt.transform.parent.gameObject.SetActive(true);
        pa.pseudoTxt.text = pseudo;
        pa.lifeBar.SetActive(true);
        foreach (Image item in pa.mapPointer)
        {
            item.gameObject.SetActive(true);
            item.color = friendColor;
        }
    }
}
