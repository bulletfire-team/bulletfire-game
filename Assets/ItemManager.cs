﻿using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public List<WeaponSkinItem> weaponSkinItems = new List<WeaponSkinItem>();

    public int characterSkin = 0;

    [Header("Emote")]
    public int[] emotes;
    

    void Start () {
        GetWeaponSkin();
        GetCharacterSkin();
        GetEmotes();
	}

    #region Character Skin
    private void GetCharacterSkin ()
    {
        if (PlayerPrefs.HasKey("CharacterSkin"))
        {
            characterSkin = PlayerPrefs.GetInt("CharacterSkin");
        }
    }

    public void SaveCharacterSkin (int skin)
    {
        characterSkin = skin;
        PlayerPrefs.SetInt("CharacterSkin", characterSkin);
    }
    #endregion

    #region Weapon skin
    private void GetWeaponSkin ()
    {
        if (PlayerPrefs.HasKey("WeaponSkin"))
        {
            weaponSkinItems = JsonUtility.FromJson<WeaponSkinItems>(PlayerPrefs.GetString("WeaponSkin")).weaponSkinItems;
        }
    }

    public void SaveWeaponSkin ()
    {
        WeaponSkinItems i = new WeaponSkinItems();
        i.weaponSkinItems = weaponSkinItems;
        PlayerPrefs.SetString("WeaponSkin", JsonUtility.ToJson(i));
    }

    public int GetSkin (int index)
    {
        WeaponSkinItem w = weaponSkinItems.Find(u => u.Weapon == index);
        if (w == null) return -1;
        return w.Skin;
    }

    public void SetSkin (int weap, int skin)
    {
        WeaponSkinItem w = weaponSkinItems.Find(u => u.Weapon == weap);
        if(w != null)
        {
            w.Skin = skin;
        }
        else
        {
            WeaponSkinItem i = new WeaponSkinItem();
            i.Weapon = weap;
            i.Skin = skin;
            weaponSkinItems.Add(i);
        }
        SaveWeaponSkin();
    }
    #endregion

    #region Emotes
    private void GetEmotes ()
    {
        if (PlayerPrefs.HasKey("Emote1"))
        {
            emotes[0] = PlayerPrefs.GetInt("Emote1");
        }
        if (PlayerPrefs.HasKey("Emote2"))
        {
            emotes[1] = PlayerPrefs.GetInt("Emote2");
        }
        if (PlayerPrefs.HasKey("Emote3"))
        {
            emotes[2] = PlayerPrefs.GetInt("Emote3");
        }
        if (PlayerPrefs.HasKey("Emote4"))
        {
            emotes[3] = PlayerPrefs.GetInt("Emote4");
        }
    }

    public void SaveEmotes ()
    {
        PlayerPrefs.SetInt("Emote1", emotes[0]);
        PlayerPrefs.SetInt("Emote2", emotes[1]);
        PlayerPrefs.SetInt("Emote3", emotes[2]);
        PlayerPrefs.SetInt("Emote4", emotes[3]);
    }
    #endregion

}

# region WeaponSkin
[System.Serializable]
public class WeaponSkinItems
{
    public List<WeaponSkinItem> weaponSkinItems = new List<WeaponSkinItem>();
}

[System.Serializable]
public class WeaponSkinItem
{
    public int Weapon;
    public int Skin;
}
#endregion