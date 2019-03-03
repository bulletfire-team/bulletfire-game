using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuoteUI : MonoBehaviour
{

    private int selectedQuote = 0;
    private PlayerQuote playerQuote;
    private int[] quotes;

    [Header("UI")]
    public Image quoteImg1;
    public Image quoteImg2;
    public Image quoteImg3;
    public Image quoteImg4;
    public TMP_Text quoteTxt1;
    public TMP_Text quoteTxt2;
    public TMP_Text quoteTxt3;
    public TMP_Text quoteTxt4;

    private void Start()
    {
        quotes = GameObject.Find("ItemManager").GetComponent<ItemManager>().quotes;
        ItemsContainer cont = GameObject.Find("Items").GetComponent<ItemsContainer>();
        if (cont.GetQuoteByIndex(quotes[0]) != null)
        {
            quoteImg1.sprite = cont.GetQuoteByIndex(quotes[0]).icon;
            quoteTxt1.text = cont.GetQuoteByIndex(quotes[0]).name;
        }

        if (cont.GetQuoteByIndex(quotes[1]) != null)
        {
            quoteImg2.sprite = cont.GetQuoteByIndex(quotes[1]).icon;
            quoteTxt2.text = cont.GetQuoteByIndex(quotes[1]).name;
        }

        if (cont.GetQuoteByIndex(quotes[2]) != null)
        {
            quoteImg3.sprite = cont.GetQuoteByIndex(quotes[2]).icon;
            quoteTxt3.text = cont.GetQuoteByIndex(quotes[2]).name;
        }

        if (cont.GetQuoteByIndex(quotes[3]) != null)
        {
            quoteImg4.sprite = cont.GetQuoteByIndex(quotes[3]).icon;
            quoteTxt4.text = cont.GetQuoteByIndex(quotes[3]).name;
        }
    }

    public void SetPlayerQuote(PlayerQuote playerquote)
    {
        this.playerQuote = playerquote;
        gameObject.SetActive(false);
    }

    public void Selectquote(int index)
    {
        selectedQuote = index;
    }

    public void Deselectquote(int index)
    {
        if (selectedQuote == index)
        {
            selectedQuote = 0;
        }
    }

    private void OnEnable()
    {
        selectedQuote = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        if (selectedQuote != 0 && quotes[selectedQuote - 1] != 0)
        {
            playerQuote.PlayQuote(quotes[selectedQuote - 1]);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
