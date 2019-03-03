using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipItem : MonoBehaviour
{

    private PlayerEquipment playerEquipment;
    private ArsenalMenu arsenalMenu;

    [Header("UI")]
    public Image equipImg;
    public TMP_Text nameTxt;

    public void Init (PlayerEquipment playerEquipment, ArsenalMenu arsenalMenu)
    {
        this.playerEquipment = playerEquipment;
        this.arsenalMenu = arsenalMenu;
        equipImg.sprite = playerEquipment.icon;
        nameTxt.text = playerEquipment.name;
    }

    public void ShowEquipment ()
    {
        arsenalMenu.ShowEquipment(playerEquipment); 
    }
}
