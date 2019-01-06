using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsContainer : MonoBehaviour
{
    public Emote[] emotes;

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
}
