using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmoteUI : MonoBehaviour
{

    private int selectedEmote = 0;
    private PlayerEmote playerEmote;
    private int[] emotes;

    [Header("UI")]
    public Image emoteImg1;
    public Image emoteImg2;
    public Image emoteImg3;
    public Image emoteImg4;
    public TMP_Text emoteTxt1;
    public TMP_Text emoteTxt2;
    public TMP_Text emoteTxt3;
    public TMP_Text emoteTxt4;

    private void Start()
    {
        emotes = GameObject.Find("ItemManager").GetComponent<ItemManager>().emotes;
        ItemsContainer cont = GameObject.Find("Items").GetComponent<ItemsContainer>();
        if (cont.GetEmoteByIndex(emotes[0]) != null)
        {
            emoteImg1.sprite = cont.GetEmoteByIndex(emotes[0]).icon;
            emoteTxt1.text = cont.GetEmoteByIndex(emotes[0]).name;
        }

        if (cont.GetEmoteByIndex(emotes[1]) != null)
        {
            emoteImg2.sprite = cont.GetEmoteByIndex(emotes[1]).icon;
            emoteTxt2.text = cont.GetEmoteByIndex(emotes[1]).name;
        }

        if (cont.GetEmoteByIndex(emotes[2]) != null)
        {
            emoteImg3.sprite = cont.GetEmoteByIndex(emotes[2]).icon;
            emoteTxt3.text = cont.GetEmoteByIndex(emotes[2]).name;
        }

        if (cont.GetEmoteByIndex(emotes[3]) != null)
        {
            emoteImg4.sprite = cont.GetEmoteByIndex(emotes[3]).icon;
            emoteTxt4.text = cont.GetEmoteByIndex(emotes[3]).name;
        }
    }

    public void SetPlayerEmote (PlayerEmote playerEmote)
    {
        this.playerEmote = playerEmote;
        gameObject.SetActive(false);
    }

    public void SelectEmote (int index)
    {
        selectedEmote = index;
    }

    public void DeselectEmote (int index)
    {
        if(selectedEmote == index)
        {
            selectedEmote = 0;
        }
    }

    private void OnEnable ()
    {
        selectedEmote = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable ()
    {
        if(selectedEmote != 0 && emotes[selectedEmote-1] != 0)
        {
            playerEmote.PlayEmote(emotes[selectedEmote - 1]);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
