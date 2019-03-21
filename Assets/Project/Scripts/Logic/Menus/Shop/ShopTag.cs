using UnityEngine;

public class ShopTag : MonoBehaviour
{
    public ShopManager shopManager;

    private void OnEnable()
    {
        shopManager.tagStuff.SetActive(true);
    }

    private void OnDisable()
    {
        shopManager.tagStuff.SetActive(false);

    }
}
