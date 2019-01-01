using UnityEngine;
using TMPro;

public class ChatItem : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text msgTxt;

    public static string globalColor = "red";
    public static string teamColor = "blue";
    public static string squadColor = "green";
    public static string playerColor = "yellow";


    public void Init (string msg, int type, bool isMe, string pseudo)
    {
        string pseu = isMe ? "you" : pseudo;
        switch (type)
        {
            case 1:
                // Global chat
                msgTxt.text = "<color=\"" + globalColor + "\">[" + pseu + "]</color>" + msg;
                break;
            case 2:
                // Squad chat
                msgTxt.text = "<color=\"" + squadColor + "\">[" + pseu + "]</color>" + msg;
                break;
            case 3:
                // Team chat
                msgTxt.text = "<color=\"" + teamColor + "\">[" + pseu + "]</color>" + msg;
                break;
            case 4:
                // Player chat
                msgTxt.text = "<color=\"" + playerColor + "\">[" + pseu + "]</color>" + msg;
                break;
        }
    }
    
}
