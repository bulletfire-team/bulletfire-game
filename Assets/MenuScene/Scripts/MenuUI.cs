using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour {

	public RectTransform rightMenuSlider;
	public RectTransform rightMenuSelect;

	private int rightPosX = -125;

    [Header("Player UI")]
    public TMP_Text pseudoTxt;
    public Image iconImg;

    private DiscordRPManager rpmanager;

    private void Start()
    {
        Server server = GameObject.Find("Server").GetComponent<Server>();
        pseudoTxt.text = server.player.nickname;
        iconImg.sprite = server.playerIcons[server.player.icon];
        rpmanager = GameObject.Find("DiscordManager").GetComponent<DiscordRPManager>();
        rpmanager.StartMenu();
    }

    public void slideRightMenu(int where){
		if(rightPosX != where){
			StartCoroutine(slideRightM(where));
		}
	}

	IEnumerator slideRightM (int where){
		int pos = 0;
		if(where < rightPosX){
			pos = -10;
		}else{
			pos = 10;
		}
		Vector3 menuPos = rightMenuSlider.anchoredPosition;
		menuPos.x += pos*4.16f;
		rightMenuSlider.anchoredPosition = menuPos;

		Vector3 selePos = rightMenuSelect.anchoredPosition;
		selePos.x += -pos;
		rightMenuSelect.anchoredPosition = selePos;

		yield return new WaitForSeconds(0.1f);
		if(menuPos.x >= where - 2 && menuPos.x <= where + 2){
			rightPosX = where;
			menuPos.x = where;
			rightMenuSlider.anchoredPosition = menuPos;
		}else{
			StartCoroutine(slideRightM(where));
		}
	}

	public void changeMenu(){
		
	}

    public void Quit ()
    {
        Application.Quit();
    }

    public void StartSearch (int type)
    {
        if(type == 1)
        {
            rpmanager.StartSearchingQuick();
        }
        else
        {
            rpmanager.StartSearchingRank();
        }
    }
    
}
