using UnityEngine;

[CreateAssetMenu(fileName ="New Emote", menuName ="New Emote")]
public class Emote : ScriptableObject
{
    public AnimationClip clip;
    public Sprite icon;
    public new string name;
}