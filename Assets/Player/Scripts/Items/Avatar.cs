using UnityEngine;

[CreateAssetMenu(fileName ="New Avatar", menuName ="New Avatar")]
public class Avatar : ScriptableObject
{

    public new string name;

    public int index;

    public Sprite icon;

    public bool buyable;
    public int price;

    public bool defaultPossessed;
}