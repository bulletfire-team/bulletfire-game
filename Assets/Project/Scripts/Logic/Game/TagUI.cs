using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TagUI : MonoBehaviour
{

    private int selectedTag = 0;
    private PlayerTag playerTag;
    private int[] tags;

    [Header("UI")]
    public Image TagImg1;
    public Image TagImg2;
    public Image TagImg3;
    public Image TagImg4;
    public TMP_Text TagTxt1;
    public TMP_Text TagTxt2;
    public TMP_Text TagTxt3;
    public TMP_Text TagTxt4;

    private void Start()
    {
        tags = GameObject.Find("ItemManager").GetComponent<ItemManager>().tags;
        ItemsContainer cont = GameObject.Find("Items").GetComponent<ItemsContainer>();
        if (cont.GetTagByIndex(tags[0]) != null)
        {
            TagImg1.sprite = cont.GetTagByIndex(tags[0]).icon;
            TagTxt1.text = cont.GetTagByIndex(tags[0]).name;
        }

        if (cont.GetTagByIndex(tags[1]) != null)
        {
            TagImg2.sprite = cont.GetTagByIndex(tags[1]).icon;
            TagTxt2.text = cont.GetTagByIndex(tags[1]).name;
        }

        if (cont.GetTagByIndex(tags[2]) != null)
        {
            TagImg3.sprite = cont.GetTagByIndex(tags[2]).icon;
            TagTxt3.text = cont.GetTagByIndex(tags[2]).name;
        }

        if (cont.GetTagByIndex(tags[3]) != null)
        {
            TagImg4.sprite = cont.GetTagByIndex(tags[3]).icon;
            TagTxt4.text = cont.GetTagByIndex(tags[3]).name;
        }
    }

    public void SetPlayerTag(PlayerTag playerTag)
    {
        this.playerTag = playerTag;
        gameObject.SetActive(false);
    }

    public void SelectTag(int index)
    {
        selectedTag = index;
    }

    public void DeselectTag(int index)
    {
        if (selectedTag == index)
        {
            selectedTag = 0;
        }
    }

    private void OnEnable()
    {
        selectedTag = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        if (selectedTag != 0 && tags[selectedTag - 1] != 0)
        {
            playerTag.PrintTag(tags[selectedTag - 1]);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
