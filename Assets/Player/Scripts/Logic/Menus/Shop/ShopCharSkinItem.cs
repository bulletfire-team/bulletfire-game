using UnityEngine;
using UnityEngine.UI;

public class ShopCharSkinItem : MonoBehaviour
{

    private int index;
    private ShopManager shop;

    [Header("UI")]
    public RawImage img;
    public GameObject equipObj;

    public void Init (Texture tex, int index, ShopManager shop, bool equip)
    {
        this.index = index;
        this.shop = shop;
        img.texture = tex;
        equipObj.SetActive(equip);
    }

    public void Select ()
    {
        shop.SelectCharSkin(index);
    }

    public void Unequip (int index)
    {
        if(this.index == index)
        {
            equipObj.SetActive(true);
        }
        else
        {
            equipObj.SetActive(false);
        }
    }
}
