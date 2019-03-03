using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopEmoteItem : MonoBehaviour
{

    private Emote emote;
    private ShopManager shopManager;

    [Header("UI")]
    public Image image;
    public TMP_Text txt;

    public void Init (Emote emote, ShopManager shopManager)
    {
        this.emote = emote;
        this.shopManager = shopManager;
        image.sprite = emote.icon;
        txt.text = emote.name;
    }

    public void Select ()
    {
        this.shopManager.SelectEmote(this.emote);
    }
}
