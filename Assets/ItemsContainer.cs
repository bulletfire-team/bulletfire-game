using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsContainer : MonoBehaviour
{
    public Emote[] emotes;
    public Quote[] quotes;

    public Emote GetEmoteByIndex (int index)
    {
        foreach (Emote item in emotes)
        {
            if(item.index == index)
            {
                return item;
            }
        }
        return null;
    }

    public Quote GetQuoteByIndex (int index)
    {
        foreach(Quote item in quotes)
        {
            if(item.index == index)
            {
                return item;
            }
        }
        return null;
    }
}
