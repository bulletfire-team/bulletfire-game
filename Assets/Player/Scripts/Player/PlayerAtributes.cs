using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class PlayerAtributes : NetworkBehaviour
{

    // Weapon Manager
    public WeaponManager weaponManager;
    public Weapon[] weapsObj;

    // Equipment Manager
    public EquipmentManager equipmentManager;
    public PlayerEquipment[] equipmentObj;

    public GameObject lifeBar;
    public GameObject pseudo;

    public Image[] mapPointer;

    public GameObject mapCam;

    public int money = 1000;

    [Header("UI")]
    public TMP_Text pseudoTxt;

    public ItemsContainer itemsContainer;

    public Animator playerAnimator;
    public GameObject weapons;
    public GameObject skinBody;
    public GameObject skinArms;
    public Animator globalAnimator;
    public Animator weapAnimator;
    public IK ik;
    public GameObject emoteCamera;

    private void Start()
    {
        itemsContainer = GameObject.Find("Items").GetComponent<ItemsContainer>();
        if(isLocalPlayer)
        {
            lifeBar.SetActive(false);
            pseudo.SetActive(false);
            mapCam.SetActive(true);
        }
        if (isLocalPlayer || isServer)
        {
            weaponManager = new WeaponManager();
            weaponManager.InitWeaponManager(weapsObj);

            equipmentManager = new EquipmentManager();
            equipmentManager.Init(equipmentObj);
        }
    }

    public void ShowMoney ()
    {
        GameObject.Find("UI").GetComponent<PlayerUIMenu>().buyMenu.GetComponent<Buy>().moneyTxt.text = money.ToString();
    }
}
