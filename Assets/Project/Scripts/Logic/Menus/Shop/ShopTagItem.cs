using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopTagItem : MonoBehaviour
{

    private new Tag tag;
    private ShopManager shopManager;

    [Header("UI")]
    public Image image;
    public TMP_Text txt;

    public void Init(Tag tag, ShopManager shopManager)
    {
        this.tag = tag;
        this.shopManager = shopManager;
        image.sprite = tag.icon;
        txt.text = tag.name;
    }

    public void Select()
    {
        this.shopManager.SelectTag(tag);
    }
}
