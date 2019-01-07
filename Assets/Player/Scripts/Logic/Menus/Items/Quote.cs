using UnityEngine;

[CreateAssetMenu(fileName ="New Quote", menuName ="New Quote")]
public class Quote : ScriptableObject
{
    public new string name;
    public AudioClip clip;
    public int index;
    public bool buyable = false;
    public int price;
    public Sprite icon;
}