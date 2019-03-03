using System.Collections.Generic;

public class WeaponManager
{
    public List<Weap> weaps = new List<Weap>();

    public void InitWeaponManager (Weapon[] w)
    {
        foreach (Weapon item in w)
        {
            int lvl = 0;
            if(item.index == 0)
            {
                lvl = 1;
            }
            Weap weap = new Weap(item, lvl);
            weaps.Add(weap);
        }
    }

    public bool ImproveWeapon(int index)
    {
        Weap weap = weaps[index];
        if(weap.lvl < 20)
        {
            weap.lvl++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetFireRate (int index)
    {
        Weap weap = weaps[index];
        float fireRate = weap.weapon.frates[weap.lvl - 1];
        return fireRate;
    }

    public float GetBulletSpeed (int index)
    {
        Weap weap = weaps[index];
        float bulletSpeed = weap.weapon.bspeeds[weap.lvl - 1];
        return bulletSpeed;
    }

    public int GetDammages (int index)
    {
        Weap weap = weaps[index];
        int dammages = weap.weapon.dams[weap.lvl - 1];
        return dammages;
    }

    public int GetPrice (int index)
    {
        Weap weap = weaps[index];
        if (weap.lvl > 19) return -1;
        int price = weap.weapon.prices[weap.lvl];
        return price;
    }

    public int GetLvl (int index)
    {
        Weap weap = weaps[index];
        return weap.lvl;
    }

    public bool CheckObtainEquipment(int weapIndex, int equipIndex)
    {
        Weap weap = weaps[weapIndex];
        return weap.CheckObtainEquipment(equipIndex);
    }

    public bool CheckEquipEquipment (int weapIndex, int equipIndex)
    {
        Weap weap = weaps[weapIndex];
        return weap.CheckEquipEquipment(equipIndex);
    }

    public void BuyEquipment (int weapIndex, int equipIndex)
    {
        Weap weap = weaps[weapIndex];
        weap.BuyEquipment(equipIndex);
    }

    public List<EquipWeap> GetEquipWeapsByCat (int weapIndex, WeaponEquipment.cat cat)
    {
        Weap weap = weaps[weapIndex];
        List<EquipWeap> res = new List<EquipWeap>();
        foreach (EquipWeap item in weap.equipments)
        {
            if(item.equipment.equipment.categorie == cat)
            {
                res.Add(item);
            }
        }

        return res;
    }

    public WeaponEquipment.cat GetEquipCat(int weapIndex, int equipIndex)
    {
        Weap weap = weaps[weapIndex];
        EquipWeap equip = weap.equipments[equipIndex];
        return equip.equipment.equipment.categorie;
    }

    public void EquipWeapon(int index, bool isFirst)
    {
        Weap weap = weaps[index];
        weap.isFirst = isFirst;
        weap.isEquip = true;
    }

    public void UnequipWeapon(int index)
    {
        Weap weap = weaps[index];
        weap.isEquip = false;
    }

    public bool CheckWeaponEquip (int index)
    {
        Weap weap = weaps[index];
        return weap.isEquip;
    }

    public void EquipEquipment (int weapIndex, int equipIndex)
    {
        Weap weap = weaps[weapIndex];
        EquipWeap equip = weap.equipments[equipIndex];
        equip.equip = true;
    }

    public void UnEquipEquipment(int weapIndex, int equipIndex)
    {
        Weap weap = weaps[weapIndex];
        EquipWeap equip = weap.equipments[equipIndex];
        equip.equip = false;
    }
}

public class Weap
{
    public Weapon weapon;
    public int lvl;
    public bool isFirst = false;
    public bool isEquip = false;

    public List<EquipWeap> equipments = new List<EquipWeap>();

    public Weap (Weapon weapon, int lvl)
    {
        this.weapon = weapon;
        this.lvl = lvl;
        int i = 0;
        foreach (EquipmentWeapon item in weapon.equipments)
        {
            EquipWeap equip = new EquipWeap(item, false, false, i);
            equipments.Add(equip);
            i++;
        }
    }

    public bool CheckObtainEquipment (int index)
    {
        EquipWeap equip = equipments[index];
        return equip.obtain;
    }

    public bool CheckEquipEquipment (int index)
    {
        EquipWeap equip = equipments[index];
        return equip.equip;
    }

    public void BuyEquipment (int index)
    {
        EquipWeap equip = equipments[index];
        equip.obtain = true;
    }
}

public class EquipWeap
{
    public EquipmentWeapon equipment;
    public bool obtain;
    public bool equip;
    public int index;

    public EquipWeap(EquipmentWeapon equipment, bool obtain, bool equip, int index)
    {
        this.equipment = equipment;
        this.obtain = obtain;
        this.equip = equip;
        this.index = index;
    }
}
