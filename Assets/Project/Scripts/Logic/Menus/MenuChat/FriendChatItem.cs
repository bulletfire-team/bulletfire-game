using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendChatItem : MonoBehaviour
{

    private FriendMessages friend;
    private ChatSystem sys;

    [Header("UI")]
    public TMP_Text nameTxt;

    public void Init(FriendMessages friend, ChatSystem sys)
    {
        this.friend = friend;
        this.sys = sys;
        nameTxt.text = friend.friendName;
    }
    
    public void OpenChat ()
    {
        sys.OpenPersonalChat(friend.friendName);
    }
}
