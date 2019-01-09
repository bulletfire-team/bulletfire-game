using UnityEngine;

[CreateAssetMenu(fileName ="New Weapon Skin", menuName ="New Weapon Skin")]
public class WeaponSkin : ScriptableObject
{
    public new string name;

    public int index;

    public Texture tex;
    public Material mat;
    public Sprite icon;

    public bool buyable = false;
    public int price;
}