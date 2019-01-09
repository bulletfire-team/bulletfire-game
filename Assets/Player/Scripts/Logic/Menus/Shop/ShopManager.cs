using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    private Server server;

    [Header("UI")]

    [Header("Money")]
    public TMP_Text moneyTxt;

    [Header("Skin System")]
    public Transform skinContainer;
    public GameObject skinItem;
    public Transform character;
    public GameObject skinStuff;
    public GameObject charskinobtain;
    public GameObject charskinequipbut;
    public TMP_Text charSkinBuyBut;
    public GameObject charskinchest;
    private int curSkin = 0;
    private List<ShopCharSkinItem> charSkinItems = new List<ShopCharSkinItem>();

    [Header("Weapon system")]
    public Transform weaponContainer;
    public GameObject weaponItem;
    public Transform weaponPlace;
    public GameObject weaponStuff;
    private GameObject curWeapon = null;
    private Weapon curWeap = null;
    public Transform weaponItemContainer;
    public GameObject weaponItemItem;
    public GameObject buyBut;
    public GameObject weapchest;
    public GameObject unlockTxt;
    private int curweapskin = -1;

    [Header("Player equipment skin system")]
    public Transform peContainer;
    public GameObject peItem;
    public Transform pePlace;
    public GameObject peStuff;
    private GameObject curPe = null;
    private PlayerEquipment curEqui = null;
    public Transform equiItemContainer;
    public GameObject equiItemItem;
    public GameObject peBuyBut;
    public GameObject peChest;
    public GameObject peUnlock;
    private int curpeSkin = -1;
    private PlayerEquipment[] playerEquipments;

    [Header("Emote system")]
    public Transform emoteContainer;
    public GameObject emoteItem;
    public GameObject playerForEmoteStuff;
    public Animator emoteAnimator;
    private Emote selectEmote = null;
    public TMP_Text emoteBuyBut;
    public GameObject emoteObtain;
    public GameObject emoteChest;
    public GameObject emoteEquip;
    public Image emoteImg1;
    public Image emoteImg2;
    public Image emoteImg3;
    public Image emoteImg4;
    public TMP_Text emoteTxt1;
    public TMP_Text emoteTxt2;
    public TMP_Text emoteTxt3;
    public TMP_Text emoteTxt4;

    [Header("Quote system")]
    public Transform quoteContainer;
    public GameObject quoteItem;
    public AudioSource quoteAudio;
    private Quote selectQuote = null;
    public TMP_Text quoteBuyBut;
    public GameObject quoteObtain;
    public GameObject quoteChest;
    public GameObject quoteEquip;
    public Image quoteImg1;
    public Image quoteImg2;
    public Image quoteImg3;
    public Image quoteImg4;
    public TMP_Text quoteTxt1;
    public TMP_Text quoteTxt2;
    public TMP_Text quoteTxt3;
    public TMP_Text quoteTxt4;


    [Header("Weapon")]
    private Weapon[] weapons;

    private ItemsContainer itemsContainer;

    private void OnEnable()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        moneyTxt.text = server.player.money.ToString();
    }

    private void OnDisable()
    {
        weaponStuff.SetActive(false);
        skinStuff.SetActive(false);
        playerForEmoteStuff.SetActive(false);
        peStuff.SetActive(false);
    }

    private void Start()
    {
        itemsContainer = GameObject.Find("Items").GetComponent<ItemsContainer>();
        weapons = itemsContainer.weapons;
        playerEquipments = itemsContainer.playerEquipments;
        server = GameObject.Find("Server").GetComponent<Server>();
        /*  ## Weapon skin ## */
        foreach (Transform item in weaponContainer)
        {
            Destroy(item.gameObject);
        }

        foreach (Weapon item in weapons)
        {
            GameObject o = Instantiate(weaponItem, weaponContainer);
            o.GetComponent<ShopWeaponItem>().Init(item, this);
        }

        /* ### Character equipment skin ### */
        foreach (Transform item in peContainer)
        {
            Destroy(item.gameObject);
        }
        foreach (PlayerEquipment item in playerEquipments)
        {
            GameObject o = Instantiate(peItem, peContainer);
            o.GetComponent<ShopPlayerEquipmentItem>().Init(item, this);
        }

        /* ## Character skin ## */
        foreach (Transform item in skinContainer)
        {
            Destroy(item.gameObject);
        }
        int i = 0;
        charSkinItems.Clear();
        int equiped = GameObject.Find("ItemManager").GetComponent<ItemManager>().characterSkin;
        ItemsContainer container = GameObject.Find("Items").GetComponent<ItemsContainer>();
        foreach (CharacterSkin item in container.characterSkins)
        {
            GameObject o = Instantiate(skinItem, skinContainer);
            o.GetComponent<ShopCharSkinItem>().Init(item.tex, i, this, i == equiped);
            charSkinItems.Add(o.GetComponent<ShopCharSkinItem>());
            i++;
        }
        SelectCharSkin(0);

        /* ### Emote ### */
        Emote[] emotes = GameObject.Find("Items").GetComponent<ItemsContainer>().emotes;
        foreach (Transform item in emoteContainer)
        {
            Destroy(item.gameObject);
        }
        foreach (Emote item in emotes)
        {
            GameObject o = Instantiate(emoteItem, emoteContainer);
            o.GetComponent<ShopEmoteItem>().Init(item, this);
        }

        /* ### Quote ### */
        Quote[] quotes = GameObject.Find("Items").GetComponent<ItemsContainer>().quotes;
        foreach (Transform item in quoteContainer)
        {
            Destroy(item.gameObject);
        }

        foreach (Quote item in quotes)
        {
            GameObject o = Instantiate(quoteItem, quoteContainer);
            o.GetComponent<ShopQuoteItem>().Init(item, this);
        }
        
    }

    #region Weapon Skin
    public void SelectWeapon (Weapon weap)
    {
        Destroy(curWeapon);
        List<UnlockWeapSkin> unlock = server.player.unlockweapskin.FindAll(u => u.Weapon_ID == weap.index);
        curWeapon = Instantiate(weap.weaponPrefab, weaponPlace);
        curWeapon.transform.localPosition = Vector3.zero;
        curWeapon.transform.localRotation = Quaternion.identity;
        curWeap = weap;
        foreach (Transform item in weaponItemContainer)
        {
            Destroy(item.gameObject);
        }
        
        foreach (WeaponSkin item in weap.skins)
        {
            bool isUnlocked = false;
            if(unlock.Find(u => u.Skin_ID == item.index) != null)
            {
                isUnlocked = true;
            }
            GameObject o = Instantiate(weaponItemItem, weaponItemContainer);
            o.GetComponent<ShopWeaponSkin>().Init(item.tex, this, item.index, isUnlocked);
        }
        buyBut.SetActive(false);
        unlockTxt.SetActive(false);
        curweapskin = -1;
    }

    public void SelectWeapSkin (Texture texture, int i)
    {
        foreach (Transform item in curWeapon.transform)
        {
            Renderer re = item.GetComponent<Renderer>();
            if(re != null)
            {
                re.material.mainTexture = texture;
            }
        }
        ItemsContainer container = GameObject.Find("Items").GetComponent<ItemsContainer>();
        List<UnlockWeapSkin> unlock = server.player.unlockweapskin;
        if(unlock.Find(u => u.Skin_ID == i && u.Weapon_ID == curWeap.index) != null)
        {
            buyBut.SetActive(false);
            unlockTxt.SetActive(true);
            weapchest.SetActive(false);
        }
        else if(container.GetWeaponSkinByIndex(i).buyable)
        {
            unlockTxt.SetActive(false);
            buyBut.SetActive(true);
            weapchest.SetActive(false);
            buyBut.GetComponentInChildren<TMPro.TMP_Text>().text = "Acheter (" + curWeap.GetWeaponSkinByIndex(i).price + ")";
        }
        else
        {
            weapchest.SetActive(true);
            unlockTxt.SetActive(false);
            buyBut.SetActive(false);
        }
        curweapskin = i;
        
    }

    public void Buy ()
    {
        if(curWeap != null && curweapskin != -1)
        {
            List<UnlockWeapSkin> unlock = GameObject.Find("Server").GetComponent<Server>().player.unlockweapskin;
            if (unlock.Find(u => u.Skin_ID == curweapskin && u.Weapon_ID == curWeap.index) != null)
            {
                return;
            }
            else
            {
                Server server = GameObject.Find("Server").GetComponent<Server>();
                if(server.player.money >= curWeap.GetWeaponSkinByIndex(curweapskin).price)
                {
                    server.player.money -= curWeap.GetWeaponSkinByIndex(curweapskin).price;
                    server.BuyWeaponSkin(curWeap.index, curweapskin);
                    SelectWeapon(curWeap);
                    moneyTxt.text = GameObject.Find("Server").GetComponent<Server>().player.money.ToString();
                }
                else
                {
                    return;
                }
            }
        }
    }
    #endregion

    #region Player equipment Skin
    public void SelectPlayerEquipment (PlayerEquipment pe)
    {
        Destroy(curPe);
        List<UnlockPlayerEquipmentSkin> unlock = server.player.unlockplayerequipmentskin.FindAll(u => u.PlayerEquipment_ID == pe.index);
        curPe = Instantiate(pe.prefab, pePlace);
        curPe.transform.localPosition = Vector3.zero;
        curPe.transform.localRotation = Quaternion.identity;
        curEqui = pe;
        foreach (Transform item in peContainer)
        {
            Destroy(item.gameObject);
        }
        
        foreach (EquipmentSkin item in pe.skins)
        {
            bool isUnlocked = false;
            if (unlock.Find(u => u.Skin_ID == item.index) != null) isUnlocked = true;
            GameObject o = Instantiate(equiItemItem, equiItemContainer);
            o.GetComponent<ShopPlayerEquipmentSkin>().Init(item.icon, this, item.index, isUnlocked);
        }
        peBuyBut.SetActive(false);
        peUnlock.SetActive(false);
        curpeSkin = -1;
    }

    public void SelectPlayerEquipmentSkin (int i)
    {
        EquipmentSkin skin = curEqui.GetEquipmentSkinByIndex(i);
        Destroy(curPe);
        curPe = Instantiate(skin.prefab, pePlace);
        curPe.transform.localPosition = Vector3.zero;
        curPe.transform.localRotation = Quaternion.identity;

        List<UnlockPlayerEquipmentSkin> unlock = server.player.unlockplayerequipmentskin;
        if(unlock.Find(u => u.Skin_ID == i && u.PlayerEquipment_ID == curEqui.index) != null)
        {
            peBuyBut.SetActive(false);
            peUnlock.SetActive(true);
            peChest.SetActive(false);
        }
        else if (skin.buyable)
        {
            peUnlock.SetActive(false);
            peBuyBut.SetActive(true);
            peChest.SetActive(false);
            peBuyBut.GetComponentInChildren<TMP_Text>().text = "Acheter (" + skin.price + ")";
        }
        else
        {
            peChest.SetActive(true);
            peBuyBut.SetActive(false);
            peUnlock.SetActive(false);
        }

        curpeSkin = i;
    }

    public void BuyEquipmentSkin ()
    {
        if(curEqui != null && curpeSkin != -1)
        {
            List<UnlockPlayerEquipmentSkin> unlock = server.player.unlockplayerequipmentskin;
            if(unlock.Find(u => u.Skin_ID == curpeSkin && u.PlayerEquipment_ID == curEqui.index) != null)
            {
                return;
            }
            else
            {
                if(server.player.money >= curEqui.GetEquipmentSkinByIndex(curpeSkin).price)
                {
                    server.player.money -= curEqui.GetEquipmentSkinByIndex(curpeSkin).price;
                    server.BuyPlayerEquipmentSkin(curEqui.index, curpeSkin);
                    SelectPlayerEquipment(curEqui);
                    moneyTxt.text = server.player.money.ToString();
                }
                else
                {
                    return;
                }
            }
        }
    }
    #endregion

    #region Character Skin
    public void SelectCharSkin (int i)
    {
        ItemsContainer container = GameObject.Find("Items").GetComponent<ItemsContainer>();
        Material mat = container.GetCharacterSkinByIndex(i).mat;
        foreach (Transform item in character)
        {
            Renderer rend = item.GetComponent<Renderer>();
            if(rend != null)
            {
                rend.material = mat;
            }
        }

        curSkin = i;
        if(curSkin == 0)
        {
            charSkinBuyBut.transform.parent.gameObject.SetActive(false);
            charskinobtain.SetActive(true);
        }
        else
        {
            if (server.player.GetUnlockCharSkin().Contains(curSkin))
            {
                charSkinBuyBut.transform.parent.gameObject.SetActive(false);
                charskinobtain.SetActive(true);
                charskinchest.SetActive(false);
            }
            else if(container.GetCharacterSkinByIndex(curSkin).buyable)
            {
                charSkinBuyBut.transform.parent.gameObject.SetActive(true);
                charskinobtain.SetActive(false);
                charskinchest.SetActive(false);
                charSkinBuyBut.text = "Acheter (" + container.GetCharacterSkinByIndex(curSkin).price + ")";
            }
            else
            {
                charskinchest.SetActive(true);
                charskinobtain.SetActive(false);
                charSkinBuyBut.transform.parent.gameObject.SetActive(false);
            }
        }

        if(curSkin == GameObject.Find("ItemManager").GetComponent<ItemManager>().characterSkin)
        {
            charskinequipbut.SetActive(false);
        }
        else if(server.player.GetUnlockCharSkin().Contains(curSkin) || curSkin == 0)
        {
            charskinequipbut.SetActive(true);
        }
        else
        {
            charskinequipbut.SetActive(false);
        }
    }

    public void BuyCharSkin ()
    {
        ItemsContainer container = GameObject.Find("Items").GetComponent<ItemsContainer>();
        if (curSkin != 0)
        {
            if (server.player.GetUnlockCharSkin().Contains(curSkin)) return;
            if(server.player.money >= container.GetCharacterSkinByIndex(curSkin).price)
            {
                server.player.money -= container.GetCharacterSkinByIndex(curSkin).price;
                server.BuyChararcterSkin(curSkin);
                SelectCharSkin(curSkin);
                moneyTxt.text = server.player.money.ToString();
            }
            else
            {
                return;
            }
        }
    }

    public void EquipCharacterSkin ()
    {
        GameObject.Find("ItemManager").GetComponent<ItemManager>().SaveCharacterSkin(curSkin);
        foreach (ShopCharSkinItem item in charSkinItems)
        {
            item.Unequip(curSkin);
        }
        SelectCharSkin(curSkin);
    }
    #endregion

    #region Emote
    public void SelectEmote (Emote emote)
    {
        emoteAnimator.Play(emote.clip.name);
        selectEmote = emote;

        if (server.player.GetUnlockEmotes().Contains(selectEmote.index))
        {
            emoteBuyBut.transform.parent.gameObject.SetActive(false);
            emoteObtain.SetActive(true);
            emoteChest.SetActive(false);
            emoteEquip.SetActive(true);
        }
        else if (selectEmote.buyable)
        {
            emoteBuyBut.transform.parent.gameObject.SetActive(true);
            emoteObtain.SetActive(false);
            emoteChest.SetActive(false);
            emoteBuyBut.text = "Acheter (" + selectEmote.price + ")";
            emoteEquip.SetActive(false);
        }
        else
        {
            emoteBuyBut.transform.parent.gameObject.SetActive(false);
            emoteObtain.SetActive(false);
            emoteChest.SetActive(true);
            emoteEquip.SetActive(false);
        }
    }

    public void BuyEmote ()
    {
        if(selectEmote != null)
        {
            if (server.player.GetUnlockEmotes().Contains(selectEmote.index)) return;
            if (!selectEmote.buyable) return;
            if(server.player.money >= selectEmote.price)
            {
                server.player.money -= selectEmote.price;
                server.BuyEmote(selectEmote.index);
                SelectEmote(selectEmote);
                moneyTxt.text = server.player.money.ToString();
            }
            else
            {
                return;
            }
            
        }
    }

    public void EquipEmote (int where)
    {
        if (where > 3) return;  
        if(selectEmote != null)
        {
            if(server.player.GetUnlockEmotes().Contains(selectEmote.index))
            {
                GameObject.Find("ItemManager").GetComponent<ItemManager>().emotes[where] = selectEmote.index;
                GameObject.Find("ItemManager").GetComponent<ItemManager>().SaveEmotes();
            }
        }
    }

    public void OpenEquipEmoteMenu ()
    {
        int[] emotes = GameObject.Find("ItemManager").GetComponent<ItemManager>().emotes;
        ItemsContainer cont = GameObject.Find("Items").GetComponent<ItemsContainer>();
        if(cont.GetEmoteByIndex(emotes[0]) != null)
        {
            emoteImg1.sprite = cont.GetEmoteByIndex(emotes[0]).icon;
            emoteTxt1.text = cont.GetEmoteByIndex(emotes[0]).name;
        }

        if (cont.GetEmoteByIndex(emotes[1]) != null) {
            emoteImg2.sprite = cont.GetEmoteByIndex(emotes[1]).icon;
            emoteTxt2.text = cont.GetEmoteByIndex(emotes[1]).name;
        }

        if (cont.GetEmoteByIndex(emotes[2]) != null) {
            emoteImg3.sprite = cont.GetEmoteByIndex(emotes[2]).icon;
            emoteTxt3.text = cont.GetEmoteByIndex(emotes[2]).name;
        }

        if (cont.GetEmoteByIndex(emotes[3]) != null)
        {
            emoteImg4.sprite = cont.GetEmoteByIndex(emotes[3]).icon;
            emoteTxt4.text = cont.GetEmoteByIndex(emotes[3]).name;
        }
    }
    #endregion

    #region Quote
    public void SelectQuote(Quote quote)
    {
        quoteAudio.clip = quote.clip;
        quoteAudio.Play();
        selectQuote = quote;

        if (server.player.GetUnlockQuotes().Contains(selectQuote.index))
        {
            quoteBuyBut.transform.parent.gameObject.SetActive(false);
            quoteObtain.SetActive(true);
            quoteChest.SetActive(false);
            quoteEquip.SetActive(true);
        }
        else if (selectQuote.buyable)
        {
            quoteBuyBut.transform.parent.gameObject.SetActive(true);
            quoteObtain.SetActive(false);
            quoteChest.SetActive(false);
            quoteBuyBut.text = "Acheter (" + selectQuote.price + ")";
            quoteEquip.SetActive(false);
        }
        else
        {
            quoteBuyBut.transform.parent.gameObject.SetActive(false);
            quoteObtain.SetActive(false);
            quoteChest.SetActive(true);
            quoteEquip.SetActive(false);
        }
    }

    public void BuyQuote()
    {
        if (selectQuote != null)
        {
            if (server.player.GetUnlockQuotes().Contains(selectQuote.index)) return;
            if (!selectQuote.buyable) return;
            if (server.player.money >= selectQuote.price)
            {
                server.player.money -= selectQuote.price;
                server.BuyQuote(selectQuote.index);
                SelectQuote(selectQuote);
                moneyTxt.text = server.player.money.ToString();
            }
            else
            {
                return;
            }

        }
    }

    public void EquipQuote(int where)
    {
        if (where > 3) return;
        if (selectQuote != null)
        {
            if (server.player.GetUnlockQuotes().Contains(selectQuote.index))
            {
                GameObject.Find("ItemManager").GetComponent<ItemManager>().quotes[where] = selectQuote.index;
                GameObject.Find("ItemManager").GetComponent<ItemManager>().SaveQuotes();
            }
        }
    }

    public void OpenEquipQuoteMenu()
    {
        int[] quotes = GameObject.Find("ItemManager").GetComponent<ItemManager>().quotes;
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
    #endregion

    public void BuyCredits ()
    {
        Application.OpenURL("https://iutlyon1-ptut-robotscompagnons.alwaysdata.net/shop");
    }

}
