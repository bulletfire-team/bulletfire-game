using UnityEngine;

public class ShopEmote : MonoBehaviour
{
    public ShopManager shopManager;

    private void OnEnable()
    {
        shopManager.playerForEmoteStuff.SetActive(true);
    }

    private void OnDisable()
    {
        shopManager.playerForEmoteStuff.SetActive(false);

    }
}
