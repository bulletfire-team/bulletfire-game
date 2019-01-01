using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeapItem : MonoBehaviour
{

    private Weapon weapon;
    private ArsenalMenu arsenalMenu;

    [Header("UI")]
    public TMP_Text nameTxt;
    public Image iconImg;

    public void Init (Weapon weapon, ArsenalMenu arsenalMenu)
    {
        this.weapon = weapon;
        this.arsenalMenu = arsenalMenu;
        nameTxt.text = weapon.name;
        iconImg.sprite = weapon.image;
    }

    public void ShowWeap ()
    {
        arsenalMenu.ShowWeap(weapon.index);
    }
}
