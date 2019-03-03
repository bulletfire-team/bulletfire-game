using UnityEngine;
using System.Collections;
using TMPro;

public class EquipmentWeaponItem : MonoBehaviour
{
    private Buy buy;
    private bool isObtain = false;
    private int index;

    [Header("UI")]
    public TMP_Text nameTxt;
    public TMP_Text butTxt;

    public void Init (EquipmentWeapon weap, Buy buy, bool obtain, int index)
    {
        this.buy = buy;
        this.isObtain = obtain;
        this.index = index;
        nameTxt.text = weap.equipment.name;
        if (obtain)
        {
            butTxt.text = "Equiper";
        }
        else
        {
            butTxt.text = "Obtenir (" + weap.equipment.price + ")";
        }
    }

    public void BuyOrEquip ()
    {
        if (isObtain)
        {
            // Equip
            buy.equipWeaponEquipment(index);
        }
        else
        {
            // Buy
            buy.buyWeaponEquipment(index);
        }
    }
}
