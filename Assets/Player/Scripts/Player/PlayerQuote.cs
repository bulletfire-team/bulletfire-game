using UnityEngine;
using UnityEngine.Networking;

public class PlayerQuote : NetworkBehaviour
{
    public PlayerAtributes attr;
    public AudioSource audio;

    private void Start()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("QuoteMenu").GetComponent<QuoteUI>().SetPlayerQuote(this);
        }
    }

    #region Public void
    public void PlayQuote(int index)
    {
        CmdPlayQuote(index);
    }
    #endregion

    #region Network
    #region Play
    public void CmdPlayQuote(int index)
    {
        RpcPlayQuote(index);
    }

    public void RpcPlayQuote(int index)
    {
        AudioClip clip = attr.itemsContainer.GetQuoteByIndex(index).clip;
        audio.PlayOneShot(clip);
    }
    #endregion
    #endregion
}
