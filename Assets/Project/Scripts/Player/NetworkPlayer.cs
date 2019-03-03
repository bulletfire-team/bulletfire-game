using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour {

	public GameObject came;

    [SyncVar(hook="OnChangeHealth")]
    public int health = 100;

    [SyncVar(hook = "OnDie")]
    public int life = 3;

    public RectTransform lifebar;

    private Vector3 firstPos;
    private Quaternion firstRot;

    public Transform weaponHolder;
    public Transform equipmentHolder;

    private bool isAlive = true;

    public PlayerUI playerUI;

    public Transform skin1;
    public Transform skin2;

    public PlayerAtributes playerAtributes;


    IEnumerator Start () {
        if(isServer && GameManager.instance == null)
        {
            GameManager gm = gameObject.AddComponent<GameManager>();
            gm.SetInstance(gm);
        }
        if(isServer) GameManager.instance.RegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<Player>());
        if (isLocalPlayer){
            GetComponent<AudioListener>().enabled = true;
            came.SetActive(true);
            skin1.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            int skin = GameObject.Find("ItemManager").GetComponent<ItemManager>().GetSkin(0);
            CmdSpawnWeap(0, true, skin);
            GameObject.Find("BuyMenu").GetComponent<Buy>().SetLocalPlayer(gameObject);
            GameObject.Find("BuyMenu").SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            CmdChangeSkin(GameObject.Find("ItemManager").GetComponent<ItemManager>().characterSkin);
            foreach (Image item in GetComponent<PlayerAtributes>().mapPointer)
            {
                item.gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(1f);
        if (isLocalPlayer) {
            if(GetComponent<Player>().team == "red")
            {
                GameObject.Find("PlayerUI").GetComponent<PlayerUI>().team.color = Color.red;
            }
            else
            {
                GameObject.Find("PlayerUI").GetComponent<PlayerUI>().team.color = Color.blue;
            }
            playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        } 
    }

    public void SetFirstTransform()
    {
        firstPos = GetComponent<Rigidbody>().position;
        firstRot = GetComponent<Rigidbody>().rotation;
    }

	public int GetDammages(int dam, string killerNetId, Vector3 pos, int type){
		if(!isServer) return 0;
        if (health <= 0) return 0;
        if (killerNetId == GetComponent<NetworkIdentity>().netId.ToString() && type == 1) return 0;
        health -= dam;
        if(health <= 0){
            if (isAlive)
            {
                isAlive = false;
                life--;
                if (life > 0)
                {
                    RpcRespawn();
                    health = 100;
                    GameManager.instance.PlayerGone(GetComponent<NetworkIdentity>().netId.ToString(), killerNetId);
                    return 3;
                }
                else
                {
                    GetComponent<Player>().isDead = true;
                    RpcRealDead();
                    health = 100;
                    GameManager.instance.PlayerDied(GetComponent<NetworkIdentity>().netId.ToString(), killerNetId);
                    return 2;
                }
            }
            else
            {
                health = 100;
            }
            return 0;
        }
        else
        {
            TargetGetHit(connectionToClient, pos);
            return 1;
        }
    }

    [TargetRpc]
    void TargetGetHit (NetworkConnection target, Vector3 targetPos)
    {
        GameObject.Find("PlayerUI").GetComponent<PlayerUI>().damageIndicatorScript.SetTarget(targetPos);
    }

    void OnChangeHealth(int li){
    	float newL = (float)li/100f;
    	lifebar.sizeDelta = new Vector2(newL*1.5f, lifebar.sizeDelta.y);
        if (isLocalPlayer)
        {
            Image blood = GameObject.Find("PlayerUI").GetComponent<PlayerUI>().blood;
            Color col = blood.color;
            col.a = 1 - ((float)li / 100f);
            blood.color = col;
        }
    }

    void OnDie (int lif)
    {
        if(isLocalPlayer)
        {
            if (playerUI == null) playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
            playerUI.lifeCount.text = lif + " vies";
        }
    }

    [ClientRpc]
    public void RpcRespawn(){
        Respawn();
    }

    [ClientRpc]
    public void RpcRealDead ()
    {
        if (!isLocalPlayer) return;
        print("Rpc real dead");
        transform.position = new Vector3(0, -100, 0);
        came.SetActive(false);
        GetComponent<Player>().isDead = true;
        GameObject.FindWithTag("DeadCamera").GetComponent<Camera>().enabled = true;
        playerUI.gameObject.SetActive(false);
        GetComponent<Player>().FreezePlayer();
    }

    [Command]
    public void CmdChangeSkin (int skin)
    {
        RpcChangeSkin(skin);
    }

    [ClientRpc]
    public void RpcChangeSkin (int skin)
    {
        //Material mat = GameObject.Find("Server").GetComponent<Server>().characterSkins[skin];
        //this.skin1.GetComponent<Renderer>().material = mat;
        //this.skin2.GetComponent<Renderer>().material = mat;
    }

    public void Respawn (bool isStart = false)
    {
        if (isLocalPlayer)
        {
            if (!isStart)
            {
                transform.position = new Vector3(0, -100, 0);
                came.SetActive(false);
                GameObject.FindWithTag("DeadCamera").GetComponent<Camera>().enabled = true;
                GetComponent<Player>().FreezePlayer();
                TMPro.TMP_Text resTxt = playerUI.respawnTxt;
                playerUI.gameObject.SetActive(false);
                StartCoroutine(WaitRespawn(resTxt));
            }
            else
            {
                GetComponent<Rigidbody>().position = firstPos;
                GetComponent<Rigidbody>().rotation = firstRot;
                transform.position = firstPos;
                transform.rotation = firstRot;
                isAlive = true;
            }
            
        }
    }

    IEnumerator WaitRespawn (TMPro.TMP_Text respawnTxt)
    {
        respawnTxt.text = "Respawn dans 5";
        yield return new WaitForSeconds(1f);
        respawnTxt.text = "Respawn dans 4";
        yield return new WaitForSeconds(1f);
        respawnTxt.text = "Respawn dans 3";
        yield return new WaitForSeconds(1f);
        respawnTxt.text = "Respawn dans 2";
        yield return new WaitForSeconds(1f);
        respawnTxt.text = "Respawn dans 1";
        yield return new WaitForSeconds(1f);
        respawnTxt.text = "";
        GetComponent<Rigidbody>().position = firstPos;
        GetComponent<Rigidbody>().rotation = firstRot;
        transform.position = firstPos;
        transform.rotation = firstRot;
        isAlive = true;
        GameObject.FindWithTag("DeadCamera").GetComponent<Camera>().enabled = false;
        playerUI.gameObject.SetActive(true);
        came.SetActive(true);
        GetComponent<Player>().UnfreezePlayer();
    }

    #region Commands
    [Command]
    public void CmdSpawnWeap(int index, bool isFirst, int skin)
    {
        GetComponent<PlayerAtributes>().weaponManager.EquipWeapon(index, isFirst);
        NetworkWeapon weap = playerAtributes.itemsContainer.GetWeaponByIndex(index).weaponPrefab.GetComponent<NetworkWeapon>();
        GameObject weapObj = (GameObject)Instantiate(playerAtributes.itemsContainer.GetWeaponByIndex(index).weaponPrefab, Vector3.zero, Quaternion.identity);
        weapObj.GetComponent<NetworkWeapon>().parentNetId = this.netId;
        weapObj.GetComponent<NetworkWeapon>().isFirst = isFirst;
        weapObj.GetComponent<NetworkWeapon>().skin = skin;
        weapObj.name = index.ToString();
        NetworkServer.Spawn(weapObj);
        weapObj.transform.parent = weaponHolder;
        weapObj.transform.localPosition = weap.startPos;
        weapObj.transform.localRotation = Quaternion.Euler(weap.startRot);
        if (isFirst) weapObj.transform.SetAsFirstSibling();
        if (!isFirst) weapObj.transform.SetAsLastSibling();
    }

    [Command]
    public void CmdDestroyWeap(bool isFirst)
    {
        int index = isFirst ? 0 : 1;
        if(weaponHolder.childCount > index)
        {
            Transform toDestroy = weaponHolder.GetChild(index);
            int numWeap = -1;
            int.TryParse(toDestroy.name, out numWeap);
            NetworkServer.Destroy(toDestroy.gameObject);
            GetComponent<PlayerAtributes>().weaponManager.UnequipWeapon(numWeap);
        }
        
    }

    [Command]
    public void CmdImproveWeapon (int index)
    {
        GetComponent<PlayerAtributes>().weaponManager.ImproveWeapon(index);
    }

    [Command]
    public void CmdBuyWeaponEquipment (int weapIndex, int equipIndex)
    {
        GetComponent<PlayerAtributes>().weaponManager.BuyEquipment(weapIndex, equipIndex);
    }

    [Command]
    public void CmdEquipWeaponEquipment (int weapIndex, int equipIndex)
    {
        WeaponManager manager = GetComponent<PlayerAtributes>().weaponManager;
        
        bool isEquip = manager.CheckEquipEquipment(weapIndex, equipIndex);
        if (isEquip) return;
        
        WeaponEquipment.cat cat = manager.GetEquipCat(weapIndex, equipIndex); 
        List<EquipWeap> equips = GetComponent<PlayerAtributes>().weaponManager.GetEquipWeapsByCat(weapIndex, cat);
        bool weapEquip = manager.CheckWeaponEquip(weapIndex);
        Transform weapObj = null;
        if (weapEquip)
        {
            weapObj = weaponHolder.Find(weapIndex.ToString());
        }
        foreach (EquipWeap item in equips)
        {
            if (item.equip)
            {
                item.equip = false;
                if (weapEquip)
                {
                    // Destroy
                    Transform toDestroy = weapObj.Find(equipIndex.ToString());
                    NetworkServer.Destroy(toDestroy.gameObject);
                }
            }
        }
        manager.EquipEquipment(weapIndex, equipIndex);
        EquipmentWeapon equipment = manager.weaps[weapIndex].equipments[equipIndex].equipment;
        if (weapEquip)
        {
            GameObject equipObj = (GameObject)Instantiate(equipment.equipment.ObjPrefab, weapObj);
            equipObj.GetComponent<NetworkEquipment>().parentNetId = weapObj.GetComponent<NetworkIdentity>().netId;
            equipObj.GetComponent<NetworkEquipment>().startRot = equipment.rot;
            equipObj.GetComponent<NetworkEquipment>().startPos = equipment.pos;
            equipObj.name = equipIndex.ToString();
            NetworkServer.Spawn(equipObj);
            equipObj.transform.parent = weapObj;
            equipObj.transform.localPosition = equipment.pos;
            equipObj.transform.localRotation = Quaternion.Euler(equipment.rot);
        }
    }

    [Command]
    public void CmdImproveEquipment (int index)
    {
        playerAtributes.equipmentManager.ImproveEquipment(index);
    }

    [Command]
    public void CmdSpawnEquipment(int index)
    {
        playerAtributes.equipmentManager.Equip(index);
        NetworkEquipment weap = playerAtributes.itemsContainer.GetPlayerEquipmentByIndex(index).prefab.GetComponent<NetworkEquipment>();
        print(weap);
        GameObject equipObj = Instantiate(playerAtributes.itemsContainer.GetPlayerEquipmentByIndex(index).prefab, Vector3.zero, Quaternion.identity);
        print(equipObj);
        equipObj.GetComponent<NetworkEquipment>().parentNetId = this.netId;
        equipObj.name = index.ToString();
        NetworkServer.Spawn(equipObj);
        equipObj.transform.parent = equipmentHolder;
        equipObj.transform.localPosition = weap.startPos;
        equipObj.transform.localRotation = Quaternion.Euler(weap.startRot);
    }

    #endregion

    public void EquipEquipment (int index)
    {
        CmdSpawnEquipment(index);
    }

    public void EquipWeapon(int index, bool isFirst)
    {
        CmdDestroyWeap(isFirst);
        int skin = GameObject.Find("ItemManager").GetComponent<ItemManager>().GetSkin(index);
        CmdSpawnWeap(index, isFirst, skin);
        if (isFirst)
        {
            StartCoroutine(waitSpawnWeap());
        }
        else
        {
            GetComponent<PlayerWeapon>().maxWeapons = 1;
        }
    }

    public void EquipWeaponEquipment (int weapIndex, int equipIndex)
    {
        CmdEquipWeaponEquipment(weapIndex, equipIndex);
    }

    IEnumerator waitSpawnWeap()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<PlayerWeapon>().SelectWeapon(0);
    }

}
