using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeapEquipmentItem : MonoBehaviour
{

    private EquipmentWeapon equipmentWeapon;

    [Header("UI")]
    public Image iconImg;
    public TMP_Text nameTxt;
    
    public void Init (EquipmentWeapon equipmentWeapon)
    {
        this.equipmentWeapon = equipmentWeapon;
        iconImg.sprite = equipmentWeapon.equipment.icon;
        nameTxt.text = equipmentWeapon.equipment.name;
    }
}
