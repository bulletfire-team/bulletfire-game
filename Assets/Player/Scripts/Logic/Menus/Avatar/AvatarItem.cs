using UnityEngine;
using UnityEngine.UI;

public class AvatarItem : MonoBehaviour
{

    private AvatarManager avatarManager;

    public Image icon;
    public GameObject check;

    private int index;


    public void Init(Sprite sprite, bool isSelected, int index, AvatarManager avatarManager)
    {
        this.index = index;
        this.avatarManager = avatarManager;
        icon.sprite = sprite;
        check.SetActive(isSelected);
    }

    public void Deselect ()
    {
        check.SetActive(false);
    }

    public void Select ()
    {
        check.SetActive(true);
    }

    public void Change ()
    {
        avatarManager.ChangeAvatar(index);
    }
}
