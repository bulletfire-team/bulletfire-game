using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text nameTxt;
    public TMP_Text scoreTxt;
    public TMP_Text posTxt;

    public void Init (PlayerEntry friend, int pos)
    {
        nameTxt.text = friend.nickname;
        scoreTxt.text = friend.rank.ToString();
        posTxt.text = pos.ToString();
    }
}
