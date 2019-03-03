using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon")]
public class Weapon : ScriptableObject {

    public enum weapType { Sniper, Other};

	public string weapName;
    public Sprite image;
    public int index = 0;
    public GameObject weaponPrefab;

    [Header("General informations")]
    public weapType type = weapType.Other;
    public int dammages;
	public Rigidbody bullet;
	public float bulletSpeed;
	public float fireRate;
    public float range = 300;
    public int maxAmmo;
    public int maxLoader;
    public float reloadTime = 2f;
    public int shotSound;
    public int reloadSound;

    [Header("Stats by level")]
	public int[] prices = new int[20] {1000,1100,1200,1300,1400,1500,1600,1700,1800,1900,2000,2100,2200,2300,2400,2500,2600,2700,2800,3000};
	public int[] dams = new int[20] {20,25,30,35,40,45,50,55,60,65,70,75,80,85,90,95,100,110,115,120};
	public float[] bspeeds = new float[20] {100,110,120,130,140,150,160,170,180,190,200,210,220,230,240,250,260,270,280,290};
	public float[] frates = new float[20] {0.5f,0.49f,0.48f,0.47f,0.46f,0.45f,0.44f,0.43f,0.42f,0.41f,0.4f,0.39f,0.38f,0.37f,0.36f,0.35f,0.34f,0.33f,0.32f,0.31f};

    
    [Header("Skin and equipment")]
    [SerializeField]public EquipmentWeapon[] equipments;
    [SerializeField]public WeaponSkin[] skins;

    public WeaponSkin GetWeaponSkinByIndex(int index)
    {
        foreach (WeaponSkin item in skins)
        {
            if (item.index == index)
            {
                return item;
            }
        }
        return null;
    }

}
