using UnityEngine;
using UnityEngine.UI;

public class ShopPlayerEquipmentSkin : MonoBehaviour
{

    private Sprite ico;
    private ShopManager shopManager;
    private int index;

    public Image img;
    public GameObject locked;

    public void Init(Sprite icon, ShopManager manager, int i, bool isunlocked)
    {
        index = i;
        ico = icon;
        shopManager = manager;
        img.sprite = icon;
        locked.SetActive(!isunlocked);
    }

    public void Select()
    {
        shopManager.SelectPlayerEquipmentSkin(index);
    }


}
