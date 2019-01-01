using UnityEngine;
using UnityEngine.UI;

public class WeapSkinItem : MonoBehaviour
{
    private ArsenalMenu arsenalMenu;
    private int index;

    public RawImage img;
    public GameObject selected;

    public void Init (Texture tex, ArsenalMenu arsenal, int index, bool isSelected)
    {
        arsenalMenu = arsenal;
        if(tex != null) img.texture = tex;
        this.index = index;
        selected.SetActive(isSelected);
    }
    
    public void Select ()
    {
        arsenalMenu.ChangeSkin(index);
        selected.SetActive(true);
    }

    public void Deselect ()
    {
        selected.SetActive(false);
    }
}
