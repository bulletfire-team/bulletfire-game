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
        int i = 0;
        foreach (Sprite item in server.playerIcons)
        {
            GameObject o = Instantiate(avatarItem, avatarContainer);
            o.GetComponent<AvatarItem>().Init(item, server.player.icon == i, i, this);
            i++;
            items.Add(o.GetComponent<AvatarItem>());
        }
    }

    public void ChangeAvatar (int index)
    {
        if(index < server.playerIcons.Length)
        {
            items[server.player.icon].Deselect();
            items[index].Select();
            server.player.icon = index;
            server.UpdateUserInfos(server.player);
            avatarIcon.sprite = server.playerIcons[index];
        }
    }
}
