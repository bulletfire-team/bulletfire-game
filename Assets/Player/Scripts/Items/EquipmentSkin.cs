using UnityEngine;

[CreateAssetMenu(fileName ="New Equipment Skin", menuName ="New Equipment Skin")]
public class EquipmentSkin : ScriptableObject
{

    public new string name;

    public int index;

    public Sprite icon;

    public bool buyable = false;
    public int price;

    public GameObject prefab;
}