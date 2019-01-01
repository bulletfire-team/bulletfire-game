using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopWeaponItem : MonoBehaviour
{

    private Weapon weap;
    private ShopManager shopManager;

    [Header("UI")]
    public TMP_Text nameTxt;
    public Image iconImg;
    
    public void Init (Weapon weap, ShopManager shopManager)
    {
        this.weap = weap;
        this.shopManager = shopManager;
        nameTxt.text = weap.name;
        iconImg.sprite = weap.image;
    }

    public void Select ()
    {
        shopManager.SelectWeapon(weap);
    }
}
