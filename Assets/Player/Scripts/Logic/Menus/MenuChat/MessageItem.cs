using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageItem : MonoBehaviour
{

    private Msg msg;

    [Header("UI")]
    public TMP_Text pseudTxt;
    public TMP_Text msgTxt;
    public Image iconImg;


    public void Init (Msg msg, Server server)
    {
        this.msg = msg;
        pseudTxt.text = msg.Sender;
        msgTxt.text = msg.Message;
        ItemsContainer container = GameObject.Find("Items").GetComponent<ItemsContainer>();
        iconImg.sprite = container.GetAvatarByIndex(msg.Icon).icon;
    }
}
