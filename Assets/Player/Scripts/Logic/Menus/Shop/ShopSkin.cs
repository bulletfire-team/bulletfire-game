using UnityEngine;
using System.Collections;

public class ShopSkin : MonoBehaviour
{

    public ShopManager shopManager;

    private void OnEnable()
    {
        shopManager.skinStuff.SetActive(true);
    }

    private void OnDisable()
    {
        shopManager.skinStuff.SetActive(false);
    }
}
