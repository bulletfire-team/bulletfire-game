using UnityEngine;

[CreateAssetMenu(fileName = "New Tag", menuName = "New Tag")]
public class Tag : ScriptableObject
{
    public Material tag;
    public Sprite icon;
    public new string name;
    public int index;
    public bool buyable = false;
    public int price;
}