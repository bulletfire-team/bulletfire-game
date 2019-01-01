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