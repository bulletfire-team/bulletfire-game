using UnityEngine;
using System.Collections.Generic;
using TMPro;

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
    public GameObject unlockTxt;
    private int curweapskin = -1;

    [Header("Weapon")]
    public Weapon[] weapons;

    private void OnEnable()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        moneyTxt.text = server.player.money.ToString();
    }

    private void OnDisable()
    {
        weaponStuff.SetActive(false);
        skinStuff.SetActive(false);
    }

    private void Start()
    {
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
        /* ## Character skin ## */
        foreach (Transform item in skinContainer)
        {
            Destroy(item.gameObject);
        }
        int i = 0;
        charSkinItems.Clear();
        int equiped = GameObject.Find("ItemManager").GetComponent<ItemManager>().characterSkin;
        foreach (Texture item in server.characterSkinsTex)
        {
            GameObject o = Instantiate(skinItem, skinContainer);
            o.GetComponent<ShopCharSkinItem>().Init(item, i, this, i == equiped);
            charSkinItems.Add(o.GetComponent<ShopCharSkinItem>());
            i++;
        }
        SelectCharSkin(0);
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
        int i = 0;
        foreach (Texture item in weap.skins)
        {
            bool isUnlocked = false;
            if(unlock.Find(u => u.Skin_ID == i) != null)
            {
                isUnlocked = true;
            }
            GameObject o = Instantiate(weaponItemItem, weaponItemContainer);
            o.GetComponent<ShopWeaponSkin>().Init(item, this, i, isUnlocked);
            i++;
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
        List<UnlockWeapSkin> unlock = server.player.unlockweapskin;
        if(unlock.Find(u => u.Skin_ID == i && u.Weapon_ID == curWeap.index) != null)
        {
            buyBut.SetActive(false);
            unlockTxt.SetActive(true);
        }
        else
        {
            unlockTxt.SetActive(false);
            buyBut.SetActive(true);
            buyBut.GetComponentInChildren<TMPro.TMP_Text>().text = "Acheter (" + curWeap.skinsPrice[i] + ")";
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
                if(server.player.money >= curWeap.skinsPrice[curweapskin])
                {
                    server.player.money -= curWeap.skinsPrice[curweapskin];
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

    #region Character Skin
    public void SelectCharSkin (int i)
    {
        Material mat = server.characterSkins[i];
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
            }
            else
            {
                charSkinBuyBut.transform.parent.gameObject.SetActive(true);
                charskinobtain.SetActive(false);
                charSkinBuyBut.text = "Acheter (" + server.skinPrice[curSkin] + ")";
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
        if(curSkin != 0)
        {
            if (server.player.GetUnlockCharSkin().Contains(curSkin)) return;
            if(server.player.money >= server.skinPrice[curSkin])
            {
                server.player.money -= server.skinPrice[curSkin];
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

}
