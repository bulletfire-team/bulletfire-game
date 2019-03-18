using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerWeapon : NetworkBehaviour
{
    public string ennemyTeam = "RedTeam";

    private MyNetworkAudioManager audioManager;

    [Header("Weapon")]
    public int currentWeapon = -1;  //num de l'arme actuelle (toutes les armes auront un num)
    public int maxWeapons = 0; // nombre d'armes maximum pour un joueur
    public Transform weaponHolder;
    public WeaponHolder curWeapon;
    public MyNetworkAnimator weapManagerAnim;
    public GameObject muzzleObj;
    public Transform cam;
    public GameObject hitEffect;
    public GameObject bloodSplat;
    public IK ik;
    public LayerMask mask;

    [Header("Knife")]
    public int cutDamage = 75;
    private bool canCut = true;
    public GameObject knife;
    public Transform cutLeftIk;
    private Transform curLeftIk;

    [Header("Grenade")]
    public GameObject grenadePrefab;
    public Transform launchPlace;
    public int grenadeForce = 2;
    public int nbGrenade = 3;
    public int nbSmokeGrenade = 3;
    private bool canLaunchGrenade = true;
    public GameObject grenadeSmokePrefab;

    [Header("Shot")]
    public MouseLook weapMouseLook;
    public MouseLook camMouseLook;
    public float recul;

    // Aim
    private bool isAiming = false;

    // Shot
    private bool canShot = true;

    // Reload
    private bool isReloading = false;

    // Grenade

    [HideInInspector] public PlayerAtributes playerAtributes;
    [HideInInspector] public PlayerUI playerUI;

    [SyncVar(hook="OnCut")]
    public bool isCutting = false;

    [SyncVar(hook = "OnThrow")]
    public bool isThrowing = false;

    [SyncVar(hook = "OnThrowSmoke")]
    public bool isThrowingSmoke = false;

    [SyncVar(hook = "OnReload")]
    public bool isReload = false;

    void Start()
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
        playerAtributes = GetComponent<PlayerAtributes>();
        audioManager = GetComponent<MyNetworkAudioManager>();
    }

    #region Shot
    public void Shot ()
    {
        if (canShot)
        {
            if (curWeapon.curLoader > 0)
            {
                curWeapon.curLoader--;
                UIWeap();
                canCut = false;
                canShot = false;
                StartCoroutine(waitFireRate());
                CmdShot();
                camMouseLook.Shot(recul);
                weapMouseLook.Shot(recul);
            }
            else
            {
                StartCoroutine(waitReload());
            }
        }
    }

    [ClientRpc]
    void RpcShot ()
    {
        GameObject muzzle = Instantiate(muzzleObj, curWeapon.shotPlace);
    }

    [ClientRpc]
    void RpcHitPlayer (Vector3 pos, int type)
    {
        GameObject blood = Instantiate(bloodSplat, pos, Quaternion.identity);
        if (isLocalPlayer)
        {
            StopCoroutine(WaitHitMarker());
            playerUI.hitMarker.SetActive(true);
            StartCoroutine(WaitHitMarker());
            switch(type)
            {
                case 1:
                    playerAtributes.money += 100;
                    break;
                case 2:
                    playerAtributes.money += 200;
                    break;
                case 3:
                    playerAtributes.money += 400;
                    break;
            }
            playerAtributes.ShowMoney();
        }
    }

    [ClientRpc]
    void RpcHitObj (Vector3 pos)
    {
        GameObject hitE = Instantiate(hitEffect, pos, Quaternion.identity);
    }

    [Command]
    void CmdShot()
    {
        audioManager.CmdPlay("Shot", curWeapon.weapon.shotSound);
        weapManagerAnim.RpcSetTrigger("Shot");
        RaycastHit hit;
        Debug.DrawRay(cam.position, cam.forward, Color.red);
        if(Physics.Raycast(cam.position, cam.forward, out hit, curWeapon.weapon.range, mask))
        {
            if(hit.collider.tag == "RedTeam" || hit.collider.tag == "BlueTeam")
            {
                int touch = hit.collider.gameObject.GetComponent<Hitbox>().GetDammages(playerAtributes.weaponManager.GetDammages(curWeapon.weapon.index), GetComponent<NetworkIdentity>().netId.ToString(), transform.position, 1);
                if(touch == 1)
                {
                    RpcHitPlayer(hit.point, 1);
                    playerAtributes.money += 100;
                }
                else if(touch == 2)
                {
                    RpcHitPlayer(hit.point, 2);
                    playerAtributes.money += 200;
                }
                else if(touch == 3)
                {
                    RpcHitPlayer(hit.point, 3);
                    playerAtributes.money += 400;
                }
            }
            else
            {
                RpcHitObj(hit.point);
            }
        }
        RpcShot();
    }

    IEnumerator WaitHitMarker ()
    {
        yield return new WaitForSeconds(0.1f);
        playerUI.hitMarker.SetActive(false);
    }

    IEnumerator waitFireRate()
    {
        yield return new WaitForSeconds(playerAtributes.weaponManager.GetFireRate(curWeapon.weapon.index));
        canShot = true;
        canCut = true;
    }
    #endregion

    #region Cut
    public void Cut ()
    {
        if(canCut && (canShot || isReloading))
        {
            if (isReloading)
            {
                StopCoroutine(waitReload());
                EndReload();
            }
            weapManagerAnim.CmdSetTrigger("Cut");
            CmdCut();
            canCut = false;
            canShot = false;
        }
    }

    [Command]
    public void CmdCut ()
    {
        isCutting = !isCutting;
        OnCut(true);
    }

    
    public void OnCut (bool cut)
    {
        curLeftIk = ik.leftHandObj;
        ik.leftHandObj = cutLeftIk;
        StartCoroutine(waitCutRate());
    }

    IEnumerator waitCutRate()
    {
        yield return new WaitForSeconds(0.25f);
        knife.SetActive(true);
        knife.GetComponent<Knife>().ennemyTag = ennemyTeam;
        knife.GetComponent<Knife>().netIdPlayer = GetComponent<NetworkIdentity>().netId.ToString();
        yield return new WaitForSeconds(0.5f);
        knife.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        canShot = true;
        canCut = true;
        ik.leftHandObj = curLeftIk;
    }
    #endregion

    #region Grenade

    #region Normal Grenade
    public void LaunchGrenade ()
    {
        if(canShot && canLaunchGrenade)
        {
            if(nbGrenade > 0)
            {
                nbGrenade--;
                playerUI.grenades[nbGrenade].SetActive(false);
                canLaunchGrenade = false;
                weapManagerAnim.CmdSetTrigger("Grenade");
                CmdLaunchGrenade();
            }
        }
    }

    [Command]
    public void CmdLaunchGrenade()
    {
        isThrowing = !isThrowing;
        //OnThrow(true);
    }

    public void OnThrow(bool throwi)
    {
        curLeftIk = ik.leftHandObj;
        ik.leftHandObj = cutLeftIk;
        StartCoroutine(waitGrenade());
    }

    IEnumerator waitGrenade()
    {
        if (isServer)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject go = Instantiate(grenadePrefab, launchPlace.position, Quaternion.identity);
            go.GetComponent<Rigidbody>().AddForce(launchPlace.TransformDirection(Vector3.forward) * grenadeForce, ForceMode.VelocityChange);
            NetworkServer.Spawn(go);
            go.GetComponent<Grenade>().ennemyTag = ennemyTeam;
            go.GetComponent<Grenade>().netIdPlayer = GetComponent<NetworkIdentity>().netId.ToString();
            yield return new WaitForSeconds(0.5f);
            canLaunchGrenade = true;
            ik.leftHandObj = curLeftIk;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            canLaunchGrenade = true;
            ik.leftHandObj = curLeftIk;
        }
    }
    #endregion

    #region Smoke Grenade
    public void LaunchSmokeGrenade ()
    {
        if(canShot && canLaunchGrenade)
        {
            if(nbSmokeGrenade > 0)
            {
                nbSmokeGrenade--;
                playerUI.smokeGrenades[nbSmokeGrenade].SetActive(false);
                canLaunchGrenade = false;
                weapManagerAnim.CmdSetTrigger("Grenade");
                CmdLaunchSmokeGrenade();
            }
        }
    }

    [Command]
    public void CmdLaunchSmokeGrenade()
    {
        isThrowingSmoke = !isThrowingSmoke;
    }

    public void OnThrowSmoke (bool throwi)
    {
        curLeftIk = ik.leftHandObj;
        ik.leftHandObj = curLeftIk;
        StartCoroutine(waitSmokeGrenade());
    }

    IEnumerator waitSmokeGrenade()
    {
        if (isServer)
        {
            yield return new WaitForSeconds(0.5f);
            GameObject go = Instantiate(grenadeSmokePrefab, launchPlace.position, Quaternion.identity);
            go.GetComponent<Rigidbody>().AddForce(launchPlace.TransformDirection(Vector3.forward) * grenadeForce, ForceMode.VelocityChange);
            NetworkServer.Spawn(go);
            yield return new WaitForSeconds(0.5f);
            canLaunchGrenade = true;
            ik.leftHandObj = curLeftIk;
        }
        else
        {
            yield return new WaitForSeconds(1f);
            canLaunchGrenade = true;
            ik.leftHandObj = curLeftIk;
        }
    }
    #endregion

    #endregion

    #region Switch Weapon
    public void SwitchWeaponWheel(int type)
    {
        if(type == 1)
        {
            if (currentWeapon + 1 <= maxWeapons)
            {
                currentWeapon++; // switch d'armes par le num d'arme
            }
            else
            {
                currentWeapon = 0;
            }
        }
        else
        {
            if (currentWeapon - 1 >= 0)
            {
                currentWeapon--;
            }
            else
            {
                currentWeapon = maxWeapons;
            }
        }

        SelectWeapon(currentWeapon);
    }
    
    public void SwitchWeaponIndex(int index)
    {
        if(maxWeapons >= index)
        {
            currentWeapon = index;
            SelectWeapon(currentWeapon);
        }
    }

    public void SelectWeapon(int index)
    {
        CmdSelectWeapon(index);
    }

    [Command]
    public void CmdSelectWeapon (int index)
    {
        RpcOnSwitchWeapon(index);
    }

    [ClientRpc]
    public void RpcOnSwitchWeapon(int newWeapon)
    {
        currentWeapon = newWeapon;
        for (var i = 0; i < weaponHolder.childCount; i++)
        {
            //Activer l'arme selectionne
            if (i == newWeapon)
            {
                Transform o = weaponHolder.GetChild(i);
                o.gameObject.SetActive(true);
                curWeapon = weaponHolder.GetChild(i).GetComponent<WeaponHolder>();
                ik.leftHandObj = curWeapon.leftIK;
                ik.rightHandObj = curWeapon.rightIK;
            }
            else
            {
                weaponHolder.GetChild(i).gameObject.SetActive(false);
            }
        }

    }
    #endregion

    #region Reload
    public void Reload()
    {
        if(curWeapon.curAmmo > 0)
        {
            if (curWeapon.curLoader < curWeapon.weapon.maxLoader)
            {
                CmdReload();
                StartCoroutine(waitReload());
            }
        }
    }

    public void CmdReload()
    {
        isReload = !isReload;
        OnReload(true);
    }

    public void OnReload (bool re)
    {
        curWeapon.anim.Play();
    }

    void EndReload()
    {
        canShot = true;
        isReloading = false;
        UIWeap();
        curWeapon.anim.Stop();
    }

    void ReloadSuccess ()
    {
        if (curWeapon.curAmmo >= curWeapon.weapon.maxLoader - curWeapon.curLoader)
        {
            curWeapon.curAmmo -= curWeapon.weapon.maxLoader - curWeapon.curLoader;
            curWeapon.curLoader = curWeapon.weapon.maxLoader;
        }
        else
        {
            curWeapon.curLoader += curWeapon.curAmmo;
            curWeapon.curAmmo = 0;
        }
    }

    IEnumerator waitReload()
    {
        isReloading = true;
        canShot = false;
        audioManager.CmdPlay("Reload", curWeapon.weapon.reloadSound);
        yield return new WaitForSeconds(curWeapon.weapon.reloadTime);
        ReloadSuccess();
        EndReload();
    }
    #endregion

    #region Aim
    public void Aim (bool status)
    {
        isAiming = status;
        weapManagerAnim.CmdSetBool("Aim", isAiming);
        playerUI.reticle.SetActive(!isAiming);
        if (curWeapon.weapon.type == Weapon.weapType.Sniper)
        {
            StartCoroutine(waitAim(status));
        }
    }

    IEnumerator waitAim (bool status)
    {
        yield return new WaitForSeconds(0.2f);
        playerUI.sniperScope.SetActive(status);
        curWeapon.gameObject.SetActive(!status);
    }
    #endregion

    void UIWeap()
    {
        if(currentWeapon == 0)
        {
            playerUI.firstAmmo.text = curWeapon.curAmmo.ToString() + " - " + curWeapon.curLoader.ToString();
        }
        else
        {
            playerUI.secondAmmo.text = curWeapon.curAmmo.ToString() + " - " + curWeapon.curLoader.ToString();
        }
        
    }
}
