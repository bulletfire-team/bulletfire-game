using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyEquipItem : MonoBehaviour
{
    private PlayerEquipment equip;
    private Buy buy;

    [Header("UI")]
    public TMP_Text nameTxt;
    public Image iconImg;

    public void Init (PlayerEquipment equip, Buy buy)
    {
        this.equip = equip;
        this.buy = buy;

        nameTxt.text = equip.name;
        iconImg.sprite = equip.icon;
    }

    public void Select ()
    {
        buy.showEquipment(equip);
    }
}
