using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Buy : MonoBehaviour {

    public TMP_Text moneyTxt;
    
	[Header("UI")]

    [Header("Weapon")]
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
    public Transform weapContainer;
    public GameObject weapItem;
    private Weapon actualWeap = null;

    [Header("Weapon Equimpent")]
    public Transform weaponEquipmentContainer;
    public GameObject weaponEquipmentItem;

    [Header("Player Equipment")]
    public TextMeshProUGUI equipBuyBut;
    public TextMeshProUGUI equipTitle;
    public TextMeshProUGUI res;
    public RectTransform resAdd;
    public RectTransform resCur;
    public Transform equipContainer;
    public GameObject equipItem;
    private PlayerEquipment actualEquipment = null;

    [Header("Private")]
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

        foreach (Weapon item in primaryWeaps)
        {
            GameObject o = Instantiate(weapItem, weapContainer);
            o.GetComponent<BuyWeapItem>().Init(item, this);
        }

        foreach (PlayerEquipment item in playerEquipments)
        {
            GameObject o = Instantiate(equipItem, equipContainer);
            o.GetComponent<BuyEquipItem>().Init(item, this);
        }
    }

    public void SetLocalPlayer (GameObject ob)
    {
        localPlayer = ob;
        showWeap(primaryWeaps[0]);
        showEquipment(playerEquipments[0]);
    }

    private void OnEnable()
    {
        if (playerAtributes == null) return;
        moneyTxt.text = playerAtributes.money.ToString();
        showWeap(primaryWeaps[0]);
        showEquipment(playerEquipments[0]);
    }

    #region Weap
    public void showWeap (Weapon weap) {
        if(playerAtributes == null)
        {
            playerAtributes = localPlayer.GetComponent<PlayerAtributes>();
        }
        moneyTxt.text = playerAtributes.money.ToString();
        actualWeap = weap;
		Weapon cur = weap;
		int lvl = playerAtributes.weaponManager.GetLvl(cur.index);
        print(lvl);
        string txt = " nv. " + lvl;
		if(lvl == 0){
			txt = " (non obtenu)";
		}
        buyBut.transform.parent.GetComponent<Button>().interactable = true;
        title.text = cur.weapName + txt;
        float maxFrate = cur.frates[0];
        float minFrate = cur.frates[cur.frates.Length - 1];
        float hundredPercentFrate = maxFrate - minFrate;
        if(lvl == 0)
        {
            deg.text = "Dégats : " + cur.dams[lvl];
            speed.text = "Vitesse de la balle : " + cur.bspeeds[lvl];
            frate.text = "Temps entre chaque tir : " + cur.frates[lvl];
            degCur.sizeDelta = new Vector2((cur.dams[lvl] * 200) / cur.dams[cur.dams.Length - 1], 20);
            degAdd.sizeDelta = new Vector2((cur.dams[lvl + 1] * 200) / cur.dams[cur.dams.Length - 1], 20);
            speedCur.sizeDelta = new Vector2((cur.bspeeds[lvl] * 200) / cur.bspeeds[cur.bspeeds.Length - 1], 20);
            speedAdd.sizeDelta = new Vector2((cur.bspeeds[lvl + 1] * 200) / cur.bspeeds[cur.bspeeds.Length - 1], 20);
            frateCur.sizeDelta = new Vector2((1- (cur.frates[lvl]-minFrate) / hundredPercentFrate) * 200, 20);
            frateAdd.sizeDelta = new Vector2((1-(cur.frates[lvl + 1] - minFrate) / hundredPercentFrate) * 200, 20);
            
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
            frateCur.sizeDelta = new Vector2((1 - (cur.frates[lvl] - minFrate) / hundredPercentFrate) * 200, 20);
            frateAdd.sizeDelta = new Vector2((1 - (cur.frates[lvl + 1] - minFrate) / hundredPercentFrate) * 200, 20);
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
		if (actualWeap != null) {
			Weapon cur = actualWeap;
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
        Weapon cur = actualWeap;
        int lvl = playerAtributes.weaponManager.GetLvl(cur.index);
        if(lvl > 0)
        {
            localPlayer.GetComponent<NetworkPlayer>().EquipWeapon(actualWeap.index, isFirst);
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
        if(actualWeap != null)
        {
            Weapon cur = actualWeap;
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
        if(actualWeap != null)
        {
            Weapon cur = actualWeap;
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
    public void showEquipment (PlayerEquipment equip)
    {
        if (playerAtributes == null)
        {
            playerAtributes = localPlayer.GetComponent<PlayerAtributes>();
        }

        actualEquipment = equip;

        PlayerEquipment cur = equip;
        int lvl = playerAtributes.equipmentManager.GetLevel(actualEquipment.index);
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
        if(actualEquipment != null)
        {
            PlayerEquipment cur = actualEquipment;
            int lvl = playerAtributes.equipmentManager.GetLevel(actualEquipment.index);
            if(lvl < 20)
            {
                if(cur.prices[lvl] <= playerAtributes.money)
                {
                    playerAtributes.money -= cur.prices[lvl];
                    if (!localPlayer.GetComponent<NetworkIdentity>().isServer)
                    {
                        playerAtributes.equipmentManager.ImproveEquipment(actualEquipment.index);
                    }
                    localPlayer.GetComponent<NetworkPlayer>().CmdImproveEquipment(actualEquipment.index);
                    showEquipment(actualEquipment);
                }
            }
        }
    }

    public void equipEquipment()
    {
        PlayerEquipment cur = actualEquipment;
        int lvl = playerAtributes.equipmentManager.GetLevel(actualEquipment.index);
        print("Try to equip lvl : " + lvl);
        if(lvl > 0)
        {
            localPlayer.GetComponent<NetworkPlayer>().EquipEquipment(actualEquipment.index);
        }
    }
    #endregion
}
