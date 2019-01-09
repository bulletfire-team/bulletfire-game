using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopPlayerEquipmentItem : MonoBehaviour
{

    private PlayerEquipment pe;
    private ShopManager shopManager;

    [Header("UI")]
    public TMP_Text nameTxt;
    public Image iconImg;

    public void Init(PlayerEquipment pe, ShopManager shopManager)
    {
        this.pe = pe;
        this.shopManager = shopManager;
        nameTxt.text = pe.name;
        iconImg.sprite = pe.icon;
    }

    public void Select()
    {
        shopManager.SelectPlayerEquipment(pe);
    }
}
