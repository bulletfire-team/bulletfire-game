using System.Collections.Generic;

[System.Serializable]
public class PlayerEntity {
	
	public string mail;
	public string pwd;
    public string nickname;
    public string kda;
    public int killnb;
    public string playtime;
    public float winrate;
    public float accuracy;
    public float headshotrate;
    public int nbheadshot;
    public int nbassist;
    public int nbdeath;
    public int icon;
    public int money;
    public int NbCoffres;

    public List<UnlockWeapSkin> unlockweapskin = new List<UnlockWeapSkin>();

    public List<UnlockCharSkin> unlockcharskin = new List<UnlockCharSkin>();

    public List<UnlockEmote> unlockemote = new List<UnlockEmote>();

    public List<UnlockAvatar> unlockavatar = new List<UnlockAvatar>();
    
    public PlayerEntity (string mail, string pass)
    {
        this.mail = mail;
        this.pwd = pass;
    }

    public List<int> GetUnlockCharSkin ()
    {
        List<int> li = new List<int>();
        foreach (UnlockCharSkin item in unlockcharskin)
        {
            li.Add(item.Skin_ID);
        }
        return li;
    }

    public List<int> GetUnlockEmotes()
    {
        List<int> li = new List<int>();
        foreach (UnlockEmote item in unlockemote)
        {
            li.Add(item.Emote_ID);
        }
        return li;
    }

}

[System.Serializable]
public class UnlockWeapSkin
{
    public int Weapon_ID;
    public int Skin_ID;
}

[System.Serializable]
public class UnlockCharSkin
{
    public int Skin_ID;

    public UnlockCharSkin(int skin)
    {
        Skin_ID = skin;
    }
}

[System.Serializable]
public class UnlockEmote
{
    public int Emote_ID;

    public UnlockEmote (int emote)
    {
        Emote_ID = emote;
    }
}

[System.Serializable]
public class UnlockAvatar
{
    public int Avatar_ID;

    public UnlockAvatar (int avatar)
    {
        Avatar_ID = avatar;
    }
}