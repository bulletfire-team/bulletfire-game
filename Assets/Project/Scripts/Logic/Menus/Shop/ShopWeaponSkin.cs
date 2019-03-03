using UnityEngine;
using UnityEngine.UI;

public class ShopWeaponSkin : MonoBehaviour
{

    private Texture texture;
    private ShopManager shopManager;
    private int index;

    public RawImage img;
    public GameObject locked;

    public void Init (Texture tex, ShopManager manager, int i, bool isunlocked)
    {
        index = i;
        texture = tex;
        shopManager = manager;
        img.texture = tex;
        locked.SetActive(!isunlocked);
    }

    public void Select ()
    {
        shopManager.SelectWeapSkin(texture, index);
    }

    
}
