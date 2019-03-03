using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoxMenu : MonoBehaviour
{

    private Server server;
    
    public GameObject boxStuff;
    public Animator anim;
    public TMP_Text nbBoxTxt;

    public GameObject boxContent;

    [Header("UI")]
    public Image boxContentImg;
    public TMP_Text boxContentTxt;

    private ItemsContainer container;


    public void OpenBox ()
    {
        if(server.player.NbCoffres > 0)
        {
            server.player.NbCoffres--;
            anim.SetTrigger("open");
            nbBoxTxt.text = server.player.NbCoffres + " coffres disponibles";
            server.OpenChest();

            server.socket.On("openchest", (data) =>
            {
                UnityThread.executeInUpdate(() =>
                {
                    Chest chest = JsonUtility.FromJson<Chest>(data.ToString());
                    Sprite contentSprite = null;
                    string contentTxt = "";
                    switch (chest.chest[0])
                    {
                        case -1:
                            // Money
                            contentTxt = "Gold +1000";
                            break;
                        case 1:
                            // Weapon skin
                            Weapon weap = container.GetWeaponByIndex(chest.chest[1]);
                            WeaponSkin weapskin = weap.GetWeaponSkinByIndex(chest.chest[2]);
                            contentSprite = weapskin.icon;
                            contentTxt = "Nouveau skin d'arme pour " + weap.name + " : " + weapskin.name;
                            break;
                        case 2:
                            // Character skin
                            CharacterSkin charskin = container.GetCharacterSkinByIndex(chest.chest[1]);
                            contentSprite = charskin.icon;
                            contentTxt = "Nouveau skin de personnage : " + charskin.name;
                            break;
                        case 3:
                            // Player avatar
                            Avatar avatar = container.GetAvatarByIndex(chest.chest[1]);
                            contentSprite = avatar.icon;
                            contentTxt = "Nouvel avatar : " + avatar.name;
                            break;
                        case 4:
                            // Emote
                            Emote emote = container.GetEmoteByIndex(chest.chest[1]);
                            contentSprite = emote.icon;
                            contentTxt = "Nouvelle emote : " + emote.name;
                            break;
                        case 5:
                            //Quote
                            Quote quote = container.GetQuoteByIndex(chest.chest[1]);
                            contentSprite = quote.icon;
                            contentTxt = "Nouvelle réplique : " + quote.name;
                            break;
                        case 6:
                            // Equipment skin
                            
                            contentTxt = "Nouveau skin d'équipement";
                            break;
                        case 7:
                            // Tag
                            contentTxt = "Nouveau tag";
                            break;
                    }

                    boxContentImg.sprite = contentSprite;
                    boxContentTxt.text = contentTxt;
                    boxContent.SetActive(true);
                });
            });
        }
    }

    private void Start()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        nbBoxTxt.text = server.player.NbCoffres + " coffres disponibles";
        container = GameObject.Find("Items").GetComponent<ItemsContainer>();
    }

    private void OnEnable()
    {
        boxStuff.SetActive(true);
    }

    private void OnDisable()
    {
        boxStuff.SetActive(false);
    }
}
