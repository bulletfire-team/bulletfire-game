using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkTranform : NetworkBehaviour
{
    [SyncVar(hook = "OnChangePos")]
    private Vector3 pos;

    [SyncVar(hook = "OnChangeRot")]
    private Quaternion rot;


    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        CmdChangeTransform(transform.position, transform.rotation);
    }

    [Command]
    private void CmdChangeTransform(Vector3 posi, Quaternion roti)
    {
        pos = posi;
        rot = roti;
    }

    public void OnChangePos(Vector3 newPos)
    {
        if (isLocalPlayer) return;
        transform.position = newPos;
    }

    public void OnChangeRot(Quaternion newRot)
    {
        if (isLocalPlayer) return;
        transform.rotation = newRot;
    }
}
