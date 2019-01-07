using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopQuoteItem : MonoBehaviour
{

    private Quote quote;
    private ShopManager shopManager;

    [Header("UI")]
    public Image img;
    public TMP_Text txt;

    public void Init (Quote quote, ShopManager shopManager)
    {
        this.quote = quote;
        this.shopManager = shopManager;

        img.sprite = quote.icon;
        txt.text = quote.name;
    }

    public void Select ()
    {
        shopManager.SelectQuote(quote);
    }
}
