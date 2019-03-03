using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameLeaderboardEntry : MonoBehaviour
{
    public TMP_Text pseudo;
    public TMP_Text kill;
    public TMP_Text death;
    public TMP_Text assist;

    public Image back;

    public Color redColor;
    public Color blueColor;

    private int kills = 0;
    private int deaths = 0;
    private int assists = 0;

    public void Init (string pseud, bool isRed)
    {
        if(isRed)
        {
            back.color = redColor;
        }
        else
        {
            back.color = blueColor;
        }

        pseudo.text = pseud;
    }

    public void AddKill ()
    {
        kills++;
        kill.text = kills.ToString();
    }

    public void AddDeath ()
    {
        deaths++;
        death.text = deaths.ToString();
    }

    public void AddAssist ()
    {
        assists++;
        assist.text = assists.ToString();
    }
}
