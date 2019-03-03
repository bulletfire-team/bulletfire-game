using UnityEngine;
using TMPro;

public class RequestItem : MonoBehaviour
{

    private FriendMenu friendMenu;
    [HideInInspector]
    public Friend friend;

    [Header("UI")]
    public TMP_Text pseudo;

    public void Init(FriendMenu friendMenu, Friend friend)
    {
        this.friendMenu = friendMenu;
        this.friend = friend;
        pseudo.text = friend.nickname;
    }

    public void Accept ()
    {
        friendMenu.AcceptFriendRequest(friend.nickname);
    }

    public void Refuse ()
    {
        friendMenu.RefuseFriendRequest(friend.nickname);
    }
}
