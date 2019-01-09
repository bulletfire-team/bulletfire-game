using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon")]
public class PlayerEquipment : ScriptableObject
{

    public new string name;

    public int index = 0;

    public GameObject prefab;

    public int[] resistance = new int[20] { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100};

    public int[] prices = new int[20] { 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900, 2000, 2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800, 3000 };

    public Sprite icon;

    public EquipmentSkin[] skins;

    public EquipmentSkin GetEquipmentSkinByIndex (int index)
    {
        foreach (EquipmentSkin item in skins)
        {
            if(item.index == index)
            {
                return item;
            }
        }
        return null;
    }
}