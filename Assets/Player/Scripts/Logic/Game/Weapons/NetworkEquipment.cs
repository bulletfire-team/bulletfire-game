using UnityEngine;
using UnityEngine.Networking;

public class NetworkEquipment : NetworkBehaviour
{
    [SyncVar]
    public Vector3 startPos;

    [SyncVar]
    public Vector3 startRot;

    [SyncVar]
    public Vector3 startScale;

    [SyncVar]
    public NetworkInstanceId parentNetId;

    [SyncVar]
    public bool isFirst = false;


    public override void OnStartClient()
    {
        GameObject parentObj = ClientScene.FindLocalObject(parentNetId);
        transform.SetParent(parentObj.transform);
        transform.localPosition = startPos;
        transform.localRotation = Quaternion.Euler(startRot);
        transform.localScale = startScale;
        if (isFirst) transform.SetAsFirstSibling();
        if (!isFirst) transform.SetAsLastSibling();
    }
}
