using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;


public class FriendItem : MonoBehaviour
{

    private FriendMenu friendMenu;
    [HideInInspector]
    public Friend friend;

    [Header("UI")]
    public TMP_Text pseudo;
    public Image status;
    public Image icon;

    public void Init (FriendMenu friendMenu, Friend friend)
    {
        this.friendMenu = friendMenu;
        this.friend = friend;
        pseudo.text = friend.nickname;
        status.color = friend.isConnected ? Color.green : Color.red;
        Server server = GameObject.Find("Server").GetComponent<Server>();
        icon.sprite = server.playerIcons[friend.icon];
    }

    public void Disconnect ()
    {
        friend.isConnected = false;
        status.color = Color.red;
    }
    
    public void Connect ()
    {
        friend.isConnected = true;
        status.color = Color.green;
    }

    public void InviteIntoSquad ()
    {
        if(friend.isConnected)
        {
            friendMenu.InviteFriendIntoSquad(friend.nickname);
        }
    }

    public void SeeProfile ()
    {

    }

    public void Delete ()
    {
        friendMenu.DeleteFriend(friend.nickname);
    }

    public void Talk ()
    {
        friendMenu.Talk(friend.nickname);
    }
}
