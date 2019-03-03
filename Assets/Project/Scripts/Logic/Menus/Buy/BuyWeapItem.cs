using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyWeapItem : MonoBehaviour
{

    private Weapon weap;
    private Buy buy;

    [Header("UI")]
    public TMP_Text nameTxt;
    public Image iconImg;
    
    public void Init (Weapon weap, Buy buy)
    {
        this.weap = weap;
        this.buy = buy;
        nameTxt.text = weap.name;
        iconImg.sprite = weap.image;
    }

    public void Select ()
    {
        buy.showWeap(weap);
    }
}
