using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class AvatarManager : MonoBehaviour
{

    private Server server;
    
    private List<AvatarItem> items = new List<AvatarItem>();

    public Transform avatarContainer;
    public GameObject avatarItem;

    public Image avatarIcon;

    private void Start()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        ItemsContainer container = GameObject.Find("Items").GetComponent<ItemsContainer>();
        int i = 0;
        foreach (Avatar item in container.avatars)
        {
            GameObject o = Instantiate(avatarItem, avatarContainer);
            o.GetComponent<AvatarItem>().Init(item.icon, server.player.icon == i, i, this);
            i++;
            items.Add(o.GetComponent<AvatarItem>());
        }
    }

    public void ChangeAvatar (int index)
    {
        ItemsContainer container = GameObject.Find("Items").GetComponent<ItemsContainer>();
        if (index < container.avatars.Length)
        {
            items[server.player.icon].Deselect();
            items[index].Select();
            server.player.icon = index;
            server.UpdateUserInfos(server.player);
            avatarIcon.sprite = container.GetAvatarByIndex(index).icon;
        }
    }
}
