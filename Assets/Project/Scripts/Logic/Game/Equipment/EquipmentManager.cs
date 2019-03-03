using System.Collections.Generic;

public class EquipmentManager
{
    public List<Equipment> equipments = new List<Equipment>();

    public void Init (PlayerEquipment[] e)
    {
        foreach (PlayerEquipment pe in e)
        {
            Equipment equipment = new Equipment(pe, 0, false);
            equipments.Add(equipment);
        }
    }

    public bool ImproveEquipment (int index)
    {
        Equipment e = equipments[index];
        if(e.level < 20)
        {
            e.level++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Equip (int index)
    {
        Equipment e = equipments[index];
        e.used = true;
    }

    public void UnEquip (int index)
    {
        Equipment e = equipments[index];
        e.used = false;
    }

    public bool IsUsed (int index)
    {
        Equipment e = equipments[index];
        return e.used;
    }

    public int GetLevel (int index)
    {
        Equipment e = equipments[index];
        return e.level;
    }

    public int GetResistance ()
    {
        int res = 0;
        foreach(Equipment e in equipments)
        {
            if(e.level > 0 && e.used)
            {
                res += e.GetResistance();
            }
        }

        return res;
    }

}

public class Equipment
{
    public PlayerEquipment equipment;
    public int level;
    public bool used;

    public Equipment(PlayerEquipment equipment, int level, bool used)
    {
        this.equipment = equipment;
        this.level = level;
        this.used = used;
    }

    public int GetResistance()
    {
        return equipment.resistance[level-1];
    }
}
