using UnityEngine;
using UnityEngine.Networking;

public class NetworkWeapon : MonoBehaviour
{
    public Vector3 startPos;

    public Vector3 startRot;

    //[SyncVar]
    //public NetworkInstanceId parentNetId;

    //[SyncVar]
    public bool isFirst = false;

    //[SyncVar]
    public int skin = -1;


    public void DoIt ()
    {
        /*GameObject parentObj = ClientScene.FindLocalObject(parentNetId);
        Transform weapPlace = parentObj.GetComponent<NetworkPlayer>().weaponHolder;
        transform.SetParent(weapPlace);
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.Euler(startRot);
        */
        if(skin != -1)
        {
            foreach (Transform item in transform)
            {
                Renderer r = item.GetComponent<Renderer>();
                if(r != null)
                {
                    r.material.mainTexture = GetComponent<WeaponHolder>().weapon.GetWeaponSkinByIndex(skin).tex;
                }
            }
        }
        
        /*
        parentObj.GetComponent<PlayerWeapon>().OnSwitchWeapon(0);*/
    }
}
