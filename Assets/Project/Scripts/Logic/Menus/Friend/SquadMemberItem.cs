using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SquadMemberItem : MonoBehaviour
{
    private FriendMenu friendMenu;
    [HideInInspector]
    public Friend friend;

    [Header("UI")]
    public TMP_Text pseudo;
    public Image pic;

    public void Init(FriendMenu friendMenu, Friend friend)
    {
        this.friendMenu = friendMenu;
        this.friend = friend;
        pseudo.text = friend.nickname;
    }

    public void KickFromSquad()
    {
        friendMenu.KickUserFromSquad(friend.nickname);
    }
} 
