using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon")]
[System.Serializable]
public class WeaponEquipment : ScriptableObject
{

    public enum cat { Viseur, Canon, Chargeur, Crosse, Poignee};

    public cat categorie;
    public new string name;


    public GameObject ObjPrefab;

    public int price;

    public Sprite icon;

}