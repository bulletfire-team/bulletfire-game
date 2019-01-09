using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ArsenalMenu : MonoBehaviour
{
    [Header("Prestiges et haut faits")]
    [Header("Player Stats")]
    public TMP_Text winRateTxt;
    public Slider winRateImg;
    public TMP_Text accuracyTxt;
    public Slider accuracyImg;
    public TMP_Text killTxt;
    public Slider killImg;
    public TMP_Text deathTxt;
    public Slider deathImg;
    public TMP_Text assistTxt;
    public Slider assistImg;
    public TMP_Text headshotTxt;
    public TMP_Text headshotnbTxt;
    public Slider headshotImg;
    public TMP_Text timeTxt;

    [Header("Encyclopédie")]
    [Header("Armes")]
    private Weapon[] weaps;
    public Transform weapContain;
    public GameObject weapItem;
    public GameObject weapsLayer;
    [Header("Arme")]
    public Image weapIcon;
    public TMP_Text weapName;
    public Transform leftEquipmentContainer;
    public Transform rightEquipmentContainer;
    public GameObject equipmentItem;
    public GameObject skinItem;
    public TMP_Text damTxt;
    public Slider damImg;
    public TMP_Text rateTxt;
    public Slider rateImg;
    public TMP_Text priceTxt;
    public TMP_Text levelTxt;
    public GameObject weapLayer;
    private int lvl = 1;
    private Weapon w;
    [Header("Equipements")]
    private PlayerEquipment[] equips;
    public Transform equipContain;
    public GameObject equipItem;
    public GameObject equipsLayer;
    [Header("Equipment")]
    public Image equipIcon;
    public TMP_Text equipName;
    public TMP_Text resTxt;
    public Slider resImg;
    public TMP_Text equipPriceTxt;
    public TMP_Text equipLevelTxt;
    public GameObject equipLayer;
    private int equipLvl = 1;
    private PlayerEquipment p;
    private ItemsContainer itemsContainer;

    private void Start()
    {
        PlayerEntity playerEntity = GameObject.Find("Server").GetComponent<Server>().player;
        itemsContainer = GameObject.Find("Items").GetComponent<ItemsContainer>();
        weaps = itemsContainer.weapons;
        equips = itemsContainer.playerEquipments;
        ShowPlayerStats(playerEntity);
        ShowWeaps();
        ShowEquipments();
    }

    #region Prestiges et haut faits
    private void ShowPlayerStats (PlayerEntity playerEntity)
    {
        // Win rate
        winRateTxt.text = playerEntity.winrate.ToString() + "%";
        winRateImg.value = playerEntity.winrate / 100;
        // Accuracy
        accuracyTxt.text = playerEntity.accuracy.ToString() + "%";
        accuracyImg.value = playerEntity.accuracy / 100;
        // KDA
        killTxt.text = playerEntity.killnb.ToString();
        deathTxt.text = playerEntity.nbdeath.ToString();
        assistTxt.text = playerEntity.nbassist.ToString();
        int max = 0;
        if (playerEntity.killnb > playerEntity.nbdeath && playerEntity.killnb > playerEntity.nbassist)
        {
            max = playerEntity.killnb;
        }
        else if (playerEntity.nbdeath > playerEntity.nbassist)
        {
            max = playerEntity.nbdeath;
        }
        else
        {
            max = playerEntity.nbassist;
        }
        if (max == 0) max = 1;
        killImg.value = playerEntity.killnb / max;
        deathImg.value = playerEntity.nbdeath / max;
        assistImg.value = playerEntity.nbassist / max;
        // Headshot
        headshotTxt.text = playerEntity.headshotrate.ToString() + "%";
        headshotImg.value = playerEntity.headshotrate / 100;
        // Time
        timeTxt.text = playerEntity.playtime;
    }

    #endregion

    #region Encyclopédie
    #region Weapons
    private void ShowWeaps ()
    {
        foreach (Weapon w in weaps)
        {
            GameObject obj = Instantiate(weapItem, weapContain);
            obj.GetComponent<WeapItem>().Init(w, this);
        }
    }

    public void ShowWeap (int index)
    {
        weapsLayer.SetActive(false);
        weapLayer.SetActive(true);
        lvl = 1;
        w = weaps[index];
        weapIcon.sprite = w.image;
        weapName.text = w.name;
        int i = 0;
        foreach (Transform t in leftEquipmentContainer)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in rightEquipmentContainer)
        {
            Destroy(t.gameObject);
        }
        foreach (EquipmentWeapon e in w.equipments)
        {
            GameObject obj = Instantiate(equipmentItem, leftEquipmentContainer);
            obj.GetComponent<WeapEquipmentItem>().Init(e);
            i++;
        }

        GameObject o = Instantiate(skinItem, rightEquipmentContainer);
        int skin = GameObject.Find("ItemManager").GetComponent<ItemManager>().GetSkin(index);
        o.GetComponent<WeapSkinItem>().Init(null, this, -1, skin == -1);

        Server server = GameObject.Find("Server").GetComponent<Server>();
        List<UnlockWeapSkin> unlock = server.player.unlockweapskin.FindAll(u => u.Weapon_ID == index);
        foreach (UnlockWeapSkin item in unlock)
        {
            GameObject ob = Instantiate(skinItem, rightEquipmentContainer);
            ob.GetComponent<WeapSkinItem>().Init(w.GetWeaponSkinByIndex(item.Skin_ID).tex, this, item.Skin_ID, skin == item.Skin_ID);
        }
        ShowWeapLvlDependInfo();
    }

    public void ChangeWeapLvl (int how)
    {
        if ((lvl + how) < 1 || (lvl + how) > 20) return;
        lvl += how;
        ShowWeapLvlDependInfo();
    }

    private void ShowWeapLvlDependInfo ()
    {
        levelTxt.text = "Niveau " + lvl;
        damTxt.text = w.dams[lvl - 1].ToString();
        damImg.value = (float)w.dams[lvl - 1] / (float)w.dams[19];
        rateTxt.text = w.frates[lvl - 1].ToString();
        rateImg.value = w.frates[19] / w.frates[lvl - 1];
        priceTxt.text = w.prices[lvl - 1].ToString();
    }

    public void ChangeSkin (int index)
    {
        foreach (Transform item in rightEquipmentContainer)
        {
            item.GetComponent<WeapSkinItem>().Deselect();
        }
        ItemManager it = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        it.SetSkin(w.index, index);
    }
    #endregion

    #region Equipments
    public void ShowEquipments ()
    {
        foreach (PlayerEquipment w in equips)
        {
            GameObject obj = Instantiate(equipItem, equipContain);
            obj.GetComponent<EquipItem>().Init(w, this);
        }
    }

    public void ShowEquipment (PlayerEquipment playerEquipment)
    {
        equipsLayer.SetActive(false);
        equipLayer.SetActive(true);
        equipLvl = 1;
        p = equips[playerEquipment.index];
        equipIcon.sprite = p.icon;
        equipName.text = p.name;
        ShowEquipLvlDependInfo();
    }

    public void ChangeEquipLvl (int how)
    {
        if ((equipLvl + how) < 1 || (equipLvl + how) > 20) return;
        equipLvl += how;
        ShowEquipLvlDependInfo();
    }

    private void ShowEquipLvlDependInfo ()
    {
        equipLevelTxt.text = "Niveau " + equipLvl;
        resTxt.text = p.resistance[equipLvl - 1].ToString();
        resImg.value = (float)p.resistance[equipLvl - 1] / (float)p.resistance[19];
        equipPriceTxt.text = p.prices[equipLvl - 1].ToString();
    }
    #endregion
    #endregion


}
