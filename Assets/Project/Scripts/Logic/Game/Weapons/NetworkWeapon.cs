using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Experimental.Rendering.HDPipeline;

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
            print("Skin ok");
            foreach (Transform item in transform)
            {
                Renderer r = item.GetComponent<Renderer>();
                print(item);
                if(r != null)
                {
                    print("Put skin");
                    r.material.SetTexture("_BaseColorMap", GetComponent<WeaponHolder>().weapon.GetWeaponSkinByIndex(skin).tex);
                }
            }
        }
        
        /*
        parentObj.GetComponent<PlayerWeapon>().OnSwitchWeapon(0);*/
    }
}
