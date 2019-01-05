using UnityEngine;
using UnityEngine.Networking;

public class NetworkWeapon : NetworkBehaviour
{
    [SyncVar]
    public Vector3 startPos;

    [SyncVar]
    public Vector3 startRot;

    [SyncVar]
    public NetworkInstanceId parentNetId;

    [SyncVar]
    public bool isFirst = false;

    [SyncVar]
    public int skin = -1;


    public void Start ()
    {
        GameObject parentObj = ClientScene.FindLocalObject(parentNetId);
        Transform weapPlace = parentObj.GetComponent<NetworkPlayer>().weaponHolder;
        transform.SetParent(weapPlace);
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.Euler(startRot);
        if(skin != -1)
        {
            foreach (Transform item in transform)
            {
                Renderer r = item.GetComponent<Renderer>();
                if(r != null)
                {
                    r.material.mainTexture = GetComponent<WeaponHolder>().weapon.skins[skin];
                }
            }
        }
        if (isFirst) transform.SetAsFirstSibling();
        if (!isFirst) transform.SetAsLastSibling();
        if (!isFirst) gameObject.SetActive(false);
        parentObj.GetComponent<PlayerWeapon>().OnSwitchWeapon(0);
    }
}
