using System.Collections;
using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine.SceneManagement;
using TMPro;


public class Server : MonoBehaviour
{
    public PlayerEntity player = null;

    public Socket socket;
    public string url;

    public bool keepCo = false;

    public Sprite[] playerIcons;
    public Material[] characterSkins;
    public Texture[] characterSkinsTex;
    public int[] skinPrice;

    [Header("Version control")]
    public string vcURL = "http://51.254.95.174/checkversion.php";
    public string versionIdentifier;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        UnityThread.initUnityThread();
    }

    private IEnumerator Start()
    {
        WWWForm form = new WWWForm();
        form.AddField("version", versionIdentifier);
        WWW www = new WWW(vcURL, form);

        yield return www;
        
        if(www.error != null)
        {
            Debug.Log(www.error);
            TMP_Text logs = GameObject.Find("Status").GetComponent<TMP_Text>();
            logs.text = "Cannot reach server";
        }
        else
        {
            if(www.text == "true")
            {
                ConnectToServer();
            }
            else
            {
                TMP_Text logs = GameObject.Find("Status").GetComponent<TMP_Text>();
                logs.text = "A new version is available open the launcher to download it";
            }
        }
    }

    private void ConnectToServer ()
    {
        socket = IO.Socket(url);
        CallBacks();
    }

    private void CallBacks ()
    {
        TMP_Text logs = null;
        if (SceneManager.GetActiveScene().name == "First")
        {
            logs = GameObject.Find("Status").GetComponent<TMP_Text>();
            logs.text = "Try to connect to server";

        }
        socket.On(Socket.EVENT_CONNECT, (evt) =>
        {

            UnityThread.executeInUpdate(() =>
            {
                logs.text = "Connected to server";
                if (SceneManager.GetActiveScene().name == "First")
                {
                    if (PlayerPrefs.HasKey("mail") && PlayerPrefs.HasKey("pass"))
                    {
                        logs.text = "Saved user";
                        PlayerEntity user = new PlayerEntity(PlayerPrefs.GetString("mail"), PlayerPrefs.GetString("pass"));
                        ConnectFromHash(user);
                    }
                    else
                    {
                        logs.text = "New user";
                        SceneManager.LoadSceneAsync("Log");
                    }
                }
            });
        });

        socket.On("SuccesCon", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                logs.text = "User connection success";
                PlayerEntity user = JsonUtility.FromJson<PlayerEntity>(evt.ToString());
                player = user;
                if (keepCo)
                {
                    PlayerPrefs.SetString("mail", user.mail);
                    PlayerPrefs.SetString("pass", user.pwd);
                }
                SceneManager.LoadSceneAsync("Menu");
            });
        });

        socket.On("ErrorCon", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                logs.text = "User connection fail";
                if (SceneManager.GetActiveScene().name == "First")
                {
                    SceneManager.LoadSceneAsync("Log");
                }
            });
        });
    }

    public void TryToConnect(PlayerEntity player, bool keepconnected)
    {
        keepCo = keepconnected;
        socket.Emit("connexion", JsonUtility.ToJson(player));
    }

    public void ConnectFromHash (PlayerEntity player)
    {
        socket.Emit("hashconnexion", JsonUtility.ToJson(player));
    }

    public void UpdateUserInfos (PlayerEntity player)
    {
        socket.Emit("update", JsonUtility.ToJson(player));

        socket.On("SuccesUpdate", (evt) =>
        {
            print("Update success");
        });
    }

    public void BuyWeaponSkin (int weap, int skin)
    {
        UnlockWeapSkin unl = new UnlockWeapSkin();
        unl.Skin_ID = skin;
        unl.Weapon_ID = weap;
        player.unlockweapskin.Add(unl);
        socket.Emit("BuyWeapSkin", JsonUtility.ToJson(unl));
    }

    public void BuyChararcterSkin (int skin)
    {
        player.unlockcharskin.Add(new UnlockCharSkin(skin));
        socket.Emit("BuyCharSkin", skin);
    }

    public void BuyEmote (int emote)
    {
        player.unlockemote.Add(new UnlockEmote(emote));
        socket.Emit("BuyEmote", emote);
    }

    public void BuyQuote (int quote)
    {
        player.unlockquote.Add(new UnlockQuote(quote));
        socket.Emit("BuyQuote", quote);
    }

    public void UpdatePlayerStats (PlayerGameStat stats)
    {
        socket.Emit("updatefromhost", JsonUtility.ToJson(stats));
    }

    public void EndGame ()
    {
        socket.Emit("endgame");
    }

    // A remplir par Guigui
    #region Chat
    /**
     Callbacks : 
        Not in a squad
        SendSquadChatSuccess
    */
    public void SendSquadChat (Msg msg)
    {
        socket.Emit("sendsquadmessage", JsonUtility.ToJson(msg));
    }
    /**
     Callbacks : 
        NoUserFound
        SendUserChatSuccess
    */
    public void SendUserChat (Msg msg)
    {
        socket.Emit("sendpersonalmessage", JsonUtility.ToJson(msg));
    }

    public void GetChat ()
    {
        socket.Emit("getchat");
    }
    #endregion

    // A remplir par Michel et Alec
    #region Friend Management
    /**
     Callbacks : 
        NoUserFoundSFR
        AlreadyFriend
        SendFriendRequestSuccess
    */
    public void SendFriendRequest(string pseudo)
    {
        socket.Emit("sendFriendRequest", pseudo);
    }
    /**
     Callbacks : 
        NoRequestFound
        AcceptFriendRequestSuccess
    */
    public void AcceptFriendRequest(string pseudo)
    {
        socket.Emit("acceptFriendRequest", pseudo);
    }
    /**
     Callbacks :
        RefuseFriendRequestSuccess
     */
    public void RefuseFriendRequest(string pseudo)
    {
        socket.Emit("refuseFriendRequest", pseudo);
    }
    /**
     Callbacks :
        OnReceiveFriends
    */
    public void GetFriends()
    {
        socket.Emit("getFriend");
    }
    /**
     Callbacks : 
        NoUserFoundIFIS
        NotAFriendIFIS
        InviteFriendIntoSquadSuccess
    */
    public void InviteFriendIntoSquad(string pseudo)
    {
        socket.Emit("inviteFriendIntoSquad", pseudo);
    }
    /**
     Callbacks : 
        NoSquadFound
        AcceptSquadInvitationSuccess
    */
    public void AcceptSquadInvitation(int squadId)
    {
        socket.Emit("acceptSquadInvitation", squadId.ToString());
    }
    /**
     Callbacks : 
        RefuseSquadRequestSuccess
     */
    public void RefuseSquadRequest(int squadId)
    {
        socket.Emit("refuseSquadRequest", squadId.ToString());
    }
    /**
     Callbacks : 
        NoUserFoundKUFS
        NotSquadAdmin
        KickUserFromSquadSuccess
    */
    public void KickUserFromSquad(string pseudo)
    {
        socket.Emit("kickUserFromSquad", pseudo);
    }
    /**
     Callbacks :
        NotInASquad
        LeaveSquadSuccess
     */
    public void LeaveSquad()
    {
        socket.Emit("leaveSquad");
    }
    /**
     Callbacks : 
        DeleteFriendSuccess
     */
    public void DeleteFriend(string pseudo)
    {
        socket.Emit("deleteFriend", pseudo);
    }
    /**
     Callbacks :
        OnReceiveFriendRequests
     */
    public void GetFriendRequests()
    {
        socket.Emit("getFriendRequests");
    }
    #endregion

    // Ne pas remplir
    #region Matchmaking
    public void StartMatchmaking(int type)
    {
        socket.Emit("startmatchmaking", type);
    }

    public void StopMatchmaking ()
    {
        socket.Emit("stopmatchmaking");
    }

    public void HostReady (ulong gameNetId)
    {
        socket.Emit("hostready", gameNetId);
    }

    // Only for tests
    public void StartTestMM (string gamestruct)
    {
        socket.Emit("startmatchmakingtest", gamestruct);
    }

    #endregion

    #region Leaderboard
    public void GetLeaderBoard (int page)
    {
        socket.Emit("getleaderboard", page.ToString());
    }
    #endregion

    private void OnApplicationQuit()
    {
        socket.Close();
        socket.Disconnect();
    }
}