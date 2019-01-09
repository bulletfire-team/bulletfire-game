using UnityEngine;

public class ItemsContainer : MonoBehaviour
{

    [Header("Weapon and equipments")]
    public Weapon[] weapons;
    public PlayerEquipment[] playerEquipments;

    [Header("Skins")]
    public WeaponSkin[] weaponSkins;
    public CharacterSkin[] characterSkins;
    public EquipmentSkin[] equipmentSkins;

    [Header("Playable")]
    public Emote[] emotes;
    public Quote[] quotes;

    [Header("Avatar")]
    public Avatar[] avatars;


    #region Weapon and Equipment
    // Weapon
    public Weapon GetWeaponByIndex(int index)
    {
        foreach (Weapon item in weapons)
        {
            if (item.index == index)
            {
                return item;
            }
        }

        return null;
    }

    // Player equipment
    public PlayerEquipment GetPlayerEquipmentByIndex (int index)
    {
        foreach (PlayerEquipment item in playerEquipments)
        {
            if(item.index == index)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region Skin
    // Weapon Skin
    public WeaponSkin GetWeaponSkinByIndex (int index)
    {
        foreach (WeaponSkin item in weaponSkins)
        {
            if(item.index == index)
            {
                return item;
            }
        }
        return null;
    }

    // Character Skin
    public CharacterSkin GetCharacterSkinByIndex (int index)
    {
        foreach (CharacterSkin item in characterSkins)
        {
            if(item.index == index)
            {
                return item;
            }
        }

        return null;
    }

    // Equipment Skin
    public EquipmentSkin GetEquipmentSkinByIndex (int index)
    {
        foreach (EquipmentSkin item in equipmentSkins)
        {
            if(item.index == index)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region Playable
    // Emote
    public Emote GetEmoteByIndex (int index)
    {
        foreach (Emote item in emotes)
        {
            if(item.index == index)
            {
                return item;
            }
        }
        return null;
    }

    // Quote
    public Quote GetQuoteByIndex (int index)
    {
        foreach(Quote item in quotes)
        {
            if(item.index == index)
            {
                return item;
            }
        }
        return null;
    }
    #endregion

    #region Avatar
    // Avatar
    public Avatar GetAvatarByIndex (int index)
    {
        foreach (Avatar item in avatars)
        {
            if(item.index == index)
            {
                return item;
            }
        }

        return null;
    }
    #endregion
}
