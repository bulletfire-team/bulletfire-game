using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class PlayerAtributes : NetworkBehaviour
{

    // Weapon Manager
    [Header("Weapon Manager")]
    public WeaponManager weaponManager;
    public Weapon[] weapsObj;

    // Equipment Manager
    [Header("Equipment Manager")]
    public EquipmentManager equipmentManager;
    public PlayerEquipment[] equipmentObj;

    // UI
    [Header("UI")]
    public GameObject lifeBar;
    public GameObject pseudo;
    public Image[] mapPointer;
    public TMP_Text pseudoTxt;
    
    [HideInInspector] public ItemsContainer itemsContainer;

    [Header("Player Scripts")]
    public Player player;
    public PlayerChat playerChat;
    public PlayerEmote playerEmote;
    public PlayerInput playerInput;
    public PlayerMovement playerMovement;
    public PlayerWeapon playerWeapon;

    public PlayerUI playerUI;

    public GameObject mapCam;
    public int money = 1000;

    public Animator playerAnimator;
    public GameObject weapons;
    public GameObject skinBody;
    public GameObject skinArms;
    public Animator globalAnimator;
    public Animator weapAnimator;
    public IK ik;
    public GameObject emoteCamera;
    public GameObject mainCamera;

    private void Start()
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
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
