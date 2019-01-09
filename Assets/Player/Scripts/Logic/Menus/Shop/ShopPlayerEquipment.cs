using UnityEngine;

public class ShopPlayerEquipment : MonoBehaviour
{

    public ShopManager shopManager;


    private void OnEnable()
    {
        shopManager.peStuff.SetActive(true);
    }

    private void OnDisable()
    {
        shopManager.peStuff.SetActive(false);
    }
}
