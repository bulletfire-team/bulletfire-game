using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Buy : MonoBehaviour {

    public TMP_Text moneyTxt;
    
	// UI
	public TextMeshProUGUI title;
	public TextMeshProUGUI deg;
	public TextMeshProUGUI speed;
	public TextMeshProUGUI frate;
	public RectTransform degAdd;
	public RectTransform degCur;
	public RectTransform speedAdd;
	public RectTransform speedCur;
	public RectTransform frateAdd;
	public RectTransform frateCur;
	public TextMeshProUGUI buyBut;

    [Header("Weapon Equimpent")]
    public Transform weaponEquipmentContainer;
    public GameObject weaponEquipmentItem;
    private int actualWeap = -1;

    // UI
    public TextMeshProUGUI equipBuyBut;
    public TextMeshProUGUI equipTitle;
    public TextMeshProUGUI res;
    public RectTransform resAdd;
    public RectTransform resCur;

    private int actualEquipment = -1;

    [HideInInspector]public GameObject localPlayer;

    private PlayerAtributes playerAtributes = null;
    private ItemsContainer itemsContainer;

    private Weapon[] primaryWeaps;
    private PlayerEquipment[] playerEquipments;

    private void Start()
    {
        itemsContainer = GameObject.Find("Items").GetComponent<ItemsContainer>();
        primaryWeaps = itemsContainer.weapons;
        playerEquipments = itemsContainer.playerEquipments;
    }

    private void OnEnable()
    {
        if (playerAtributes == null) return;
        moneyTxt.text = playerAtributes.money.ToString();
    }

    #region Weap
    public void showWeap (int index) {
        if(playerAtributes == null)
        {
            playerAtributes = localPlayer.GetComponent<PlayerAtributes>();
        }
        moneyTxt.text = playerAtributes.money.ToString();
        actualWeap = index;
		Weapon cur = primaryWeaps[index];
		int lvl = playerAtributes.weaponManager.GetLvl(cur.index);
        print(lvl);
        string txt = " nv. " + lvl;
		if(lvl == 0){
			txt = " (non obtenu)";
		}
        buyBut.transform.parent.GetComponent<Button>().interactable = true;
        title.text = cur.weapName + txt;
        if(lvl == 0)
        {
            deg.text = "Dégats : " + cur.dams[lvl];
            speed.text = "Vitesse de la balle : " + cur.bspeeds[lvl];
            frate.text = "Temps entre chaque tir : " + cur.frates[lvl];
            degCur.sizeDelta = new Vector2((cur.dams[lvl] * 200) / cur.dams[cur.dams.Length - 1], 20);
            degAdd.sizeDelta = new Vector2((cur.dams[lvl + 1] * 200) / cur.dams[cur.dams.Length - 1], 20);
            speedCur.sizeDelta = new Vector2((cur.bspeeds[lvl] * 200) / cur.bspeeds[cur.bspeeds.Length - 1], 20);
            speedAdd.sizeDelta = new Vector2((cur.bspeeds[lvl + 1] * 200) / cur.bspeeds[cur.bspeeds.Length - 1], 20);
            frateCur.sizeDelta = new Vector2((cur.frates[lvl] * 200) / cur.frates[cur.frates.Length - 1] - 200, 20);
            frateAdd.sizeDelta = new Vector2((cur.frates[lvl + 1] * 200) / cur.frates[cur.frates.Length - 1] - 200, 20);
            
            txt = "Obtenir (" + cur.prices[0] + ")";
        }
        else if(lvl < cur.prices.Length)
        {
            txt = "Amélioration nv. " + (lvl + 1) + " (" + cur.prices[lvl] + ")";
            lvl--;
            deg.text = "Dégats : " + cur.dams[lvl] + " + " + (cur.dams[lvl + 1] - cur.dams[lvl]);
            speed.text = "Vitesse de la balle : " + cur.bspeeds[lvl] + " + " + (cur.bspeeds[lvl + 1] - cur.bspeeds[lvl]);
            frate.text = "Temps entre chaque tir : " + cur.frates[lvl] + " - " + (Mathf.Floor((cur.frates[lvl] - cur.frates[lvl + 1]) * 100 + 0.5f) / 100);
            degCur.sizeDelta = new Vector2((cur.dams[lvl] * 200) / cur.dams[cur.dams.Length - 1], 20);
            degAdd.sizeDelta = new Vector2((cur.dams[lvl + 1] * 200) / cur.dams[cur.dams.Length - 1], 20);
            speedCur.sizeDelta = new Vector2((cur.bspeeds[lvl] * 200) / cur.bspeeds[cur.bspeeds.Length - 1], 20);
            speedAdd.sizeDelta = new Vector2((cur.bspeeds[lvl + 1] * 200) / cur.bspeeds[cur.bspeeds.Length - 1], 20);
            frateCur.sizeDelta = new Vector2((cur.frates[lvl] * 200) / cur.frates[cur.frates.Length - 1] - 200, 20);
            frateAdd.sizeDelta = new Vector2((cur.frates[lvl + 1] * 200) / cur.frates[cur.frates.Length - 1] - 200, 20);
        }
        else
        {
            txt = "Nv MAX";
            buyBut.transform.parent.GetComponent<Button>().interactable = false;
            lvl--;
            deg.text = "Dégats : " + cur.dams[lvl];
            speed.text = "Vitesse de la balle : " + cur.bspeeds[lvl];
            frate.text = "Temps entre chaque tir : " + cur.frates[lvl];
            degCur.sizeDelta = new Vector2((cur.dams[lvl] * 200) / cur.dams[cur.dams.Length - 1], 20);
            degAdd.sizeDelta = new Vector2(0, 20);
            speedCur.sizeDelta = new Vector2((cur.bspeeds[lvl] * 200) / cur.bspeeds[cur.bspeeds.Length - 1], 20);
            speedAdd.sizeDelta = new Vector2(0, 20);
            frateCur.sizeDelta = new Vector2((cur.frates[lvl] * 200) / cur.frates[cur.frates.Length - 1] - 200, 20);
            frateAdd.sizeDelta = new Vector2(0, 20);
        }
				
		buyBut.text = txt;
        foreach (Transform t in weaponEquipmentContainer)
        {
            Destroy(t.gameObject);
        }
        lvl = playerAtributes.weaponManager.GetLvl(cur.index);
        if (lvl > 0)
        {
            int i = 0;
            foreach (EquipmentWeapon equipmentWeapon in cur.equipments)
            {
                GameObject obj = (GameObject)Instantiate(weaponEquipmentItem, weaponEquipmentContainer);
                bool obtain = playerAtributes.weaponManager.CheckObtainEquipment(cur.index, i);
                obj.GetComponent<EquipmentWeaponItem>().Init(equipmentWeapon, this, obtain, i);
                i++;
            }   
        }
    }

	public void improve () {
		if (actualWeap != -1) {
			Weapon cur = primaryWeaps[actualWeap];
            int lvl = playerAtributes.weaponManager.GetLvl(cur.index);
			// Si on a assez d'argent pour acheter
            if(lvl <= cur.prices.Length - 1)
            {
                if (cur.prices[lvl] <= playerAtributes.money)
                {
                    playerAtributes.money -= cur.prices[lvl];
                    if (!localPlayer.GetComponent<NetworkIdentity>().isServer) {
                        playerAtributes.weaponManager.ImproveWeapon(cur.index);
                    }
                    
                    localPlayer.GetComponent<NetworkPlayer>().CmdImproveWeapon(cur.index);
                    showWeap(actualWeap);
                }
            }
			
		}
	}

    public void equip (bool isFirst)
    {
        Weapon cur = primaryWeaps[actualWeap];
        int lvl = playerAtributes.weaponManager.GetLvl(cur.index);
        if(lvl > 0)
        {
            localPlayer.GetComponent<NetworkPlayer>().EquipWeapon(actualWeap, isFirst);
            if (isFirst)
            {
                GameObject.Find("PlayerUI").GetComponent<PlayerUI>().firstName.text = cur.name + " |";
                GameObject.Find("PlayerUI").GetComponent<PlayerUI>().firstAmmo.text = cur.maxAmmo + " - " + cur.maxLoader;
                GameObject.Find("PlayerUI").GetComponent<PlayerUI>().firstIcon.sprite = cur.image;
            }
            else
            {
                GameObject.Find("PlayerUI").GetComponent<PlayerUI>().secondName.text = cur.name + " |";
                GameObject.Find("PlayerUI").GetComponent<PlayerUI>().secondAmmo.text = cur.maxAmmo + " - " + cur.maxLoader;
                GameObject.Find("PlayerUI").GetComponent<PlayerUI>().secondIcon.sprite = cur.image;
            }
        }
        else
        {
            // Error 
        }
    }

    public void buyWeaponEquipment(int index)
    {
        if(actualWeap != -1)
        {
            Weapon cur = primaryWeaps[actualWeap];
            EquipmentWeapon equip = cur.equipments[index];
            if(equip.equipment.price <= playerAtributes.money)
            {
                playerAtributes.money -= equip.equipment.price;
                if (!localPlayer.GetComponent<NetworkIdentity>().isServer)
                {
                    playerAtributes.weaponManager.BuyEquipment(cur.index, index);
                }

                localPlayer.GetComponent<NetworkPlayer>().CmdBuyWeaponEquipment(cur.index, index);
                showWeap(actualWeap);
            }
        }
    }

    public void equipWeaponEquipment (int index)
    {
        if(actualWeap != -1)
        {
            Weapon cur = primaryWeaps[actualWeap];
            bool obtain = playerAtributes.weaponManager.CheckObtainEquipment(cur.index, index);
            if (obtain)
            {
                localPlayer.GetComponent<NetworkPlayer>().EquipWeaponEquipment(cur.index, index);
                showWeap(actualWeap);
            }
            else
            {
                // Error
            }
        }
    }
    #endregion

    #region Player Equipment
    public void showEquipment (int index)
    {
        if (playerAtributes == null)
        {
            playerAtributes = localPlayer.GetComponent<PlayerAtributes>();
        }

        actualEquipment = index;

        PlayerEquipment cur = playerEquipments[actualEquipment];
        int lvl = playerAtributes.equipmentManager.GetLevel(actualEquipment);
        string txt = " nv. " + lvl;
        if (lvl == 0) txt = " (non obtenu)";
        equipBuyBut.transform.parent.GetComponent<Button>().interactable = true;
        equipTitle.text = cur.name + txt;
        if(lvl == 0)
        {
            txt = "Obtenir (" + cur.prices[0] + ")";
            res.text = "Résistance : " + cur.resistance[lvl];
            resCur.sizeDelta = new Vector2(0, 20);
            resAdd.sizeDelta = new Vector2((cur.resistance[lvl] * 200) / cur.resistance[cur.resistance.Length - 1], 20);
        }
        else if (lvl < 20)
        {
            txt = "Amélioration nv. " + (lvl + 1) + " (" + cur.prices[lvl] + ")";
            lvl--;
            res.text = "Résistance : " + cur.resistance[lvl] + " + " + (cur.resistance[lvl + 1] - cur.resistance[lvl]);
            resCur.sizeDelta = new Vector2((cur.resistance[lvl] * 200) / cur.resistance[cur.resistance.Length - 1], 20);
            resAdd.sizeDelta = new Vector2((cur.resistance[lvl + 1] * 200) / cur.resistance[cur.resistance.Length - 1], 20);
        }
        else
        {
            txt = "Nv MAX";
            equipBuyBut.transform.parent.GetComponent<Button>().interactable = false;
            lvl--;
            res.text = "Résistance : " + cur.resistance[lvl];
            resCur.sizeDelta = new Vector2((cur.resistance[lvl] * 200) / cur.resistance[cur.resistance.Length - 1], 20);
            resAdd.sizeDelta = new Vector2(0, 20);
        }
        equipBuyBut.text = txt;
    }

    public void improveEquip()
    {
        if(actualEquipment != -1)
        {
            PlayerEquipment cur = playerEquipments[actualEquipment];
            int lvl = playerAtributes.equipmentManager.GetLevel(actualEquipment);
            if(lvl < 20)
            {
                if(cur.prices[lvl] <= playerAtributes.money)
                {
                    playerAtributes.money -= cur.prices[lvl];
                    if (!localPlayer.GetComponent<NetworkIdentity>().isServer)
                    {
                        playerAtributes.equipmentManager.ImproveEquipment(actualEquipment);
                    }
                    localPlayer.GetComponent<NetworkPlayer>().CmdImproveEquipment(actualEquipment);
                    showEquipment(actualEquipment);
                }
            }
        }
    }

    public void equipEquipment()
    {
        PlayerEquipment cur = playerEquipments[actualEquipment];
        int lvl = playerAtributes.equipmentManager.GetLevel(actualEquipment);
        print("Try to equip lvl : " + lvl);
        if(lvl > 0)
        {
            localPlayer.GetComponent<NetworkPlayer>().EquipEquipment(actualEquipment);
        }
    }
    #endregion
}
