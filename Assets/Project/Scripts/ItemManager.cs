using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public List<WeaponSkinItem> weaponSkinItems = new List<WeaponSkinItem>();

    public int characterSkin = 0;

    [Header("Emote")]
    public int[] emotes;

    [Header("Quote")]
    public int[] quotes;

    [Header("Tag")]
    public int[] tags;
    

    void Start () {
        GetWeaponSkin();
        GetCharacterSkin();
        GetEmotes();
        GetQuotes();
        GetTags();
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

    #region Quotes
    public void GetQuotes ()
    {
        if (PlayerPrefs.HasKey("Quote1"))
        {
            quotes[0] = PlayerPrefs.GetInt("Quote1");
        }

        if (PlayerPrefs.HasKey("Quote2"))
        {
            quotes[1] = PlayerPrefs.GetInt("Quote2");
        }

        if (PlayerPrefs.HasKey("Quote3"))
        {
            quotes[2] = PlayerPrefs.GetInt("Quote3");
        }

        if (PlayerPrefs.HasKey("Quote4"))
        {
            quotes[3] = PlayerPrefs.GetInt("Quote4");
        }
    }

    public void SaveQuotes ()
    {
        PlayerPrefs.SetInt("Quote1", quotes[0]);
        PlayerPrefs.SetInt("Quote2", quotes[1]);
        PlayerPrefs.SetInt("Quote3", quotes[2]);
        PlayerPrefs.SetInt("Quote4", quotes[3]);
    }
    #endregion

    #region Tags
    private void GetTags ()
    {
        if (PlayerPrefs.HasKey("Tag1"))
        {
            tags[0] = PlayerPrefs.GetInt("Tag1");
        }
        if (PlayerPrefs.HasKey("Tag2"))
        {
            tags[1] = PlayerPrefs.GetInt("Tag2");
        }
        if (PlayerPrefs.HasKey("Tag3"))
        {
            tags[2] = PlayerPrefs.GetInt("Tag3");
        }
        if (PlayerPrefs.HasKey("Tag4"))
        {
            tags[3] = PlayerPrefs.GetInt("Tag4");
        }
    }

    public void SaveTags ()
    {
        PlayerPrefs.SetInt("Tag1", tags[0]);
        PlayerPrefs.SetInt("Tag2", tags[1]);
        PlayerPrefs.SetInt("Tag3", tags[2]);
        PlayerPrefs.SetInt("Tag4", tags[3]);
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