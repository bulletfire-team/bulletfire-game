using UnityEngine;
using System.Collections;

public class ShopWeapon : MonoBehaviour
{

    public ShopManager shopManager;


    private void OnEnable()
    {
        shopManager.weaponStuff.SetActive(true);
    }

    private void OnDisable()
    {
        shopManager.weaponStuff.SetActive(false);
    }
}
