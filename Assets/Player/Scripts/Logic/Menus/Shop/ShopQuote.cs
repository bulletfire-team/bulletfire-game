using UnityEngine;

public class ShopQuote : MonoBehaviour
{
    public ShopManager shopManager;

    private void OnEnable()
    {
        shopManager.quoteAudio.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        shopManager.quoteAudio.gameObject.SetActive(false);

    }

}
