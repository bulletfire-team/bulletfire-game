using UnityEngine;

[CreateAssetMenu(fileName ="New Character Skin", menuName ="New Character Skin")]
public class CharacterSkin : ScriptableObject
{

    public new string name;

    public int index;

    public Texture tex;
    public Material mat;
    public Sprite icon;

    public bool buyable = false;
    public int price;
}