using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkTranform : NetworkBehaviour
{

    public enum Type { Tranform, Rigidbody };

    public Type syncType = Type.Tranform;

    [SyncVar(hook = "OnChangePos")]
    public Vector3 pos;

    [SyncVar(hook = "OnChangeRot")]
    public Quaternion rot;

    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if(syncType == Type.Tranform)
        {
            CmdChangeTransform(transform.position, transform.rotation);
        }else if(syncType == Type.Rigidbody)
        {
            CmdChangeRigidbody(body.position, body.rotation);
        }
       
    }

    [Command]
    private void CmdChangeTransform(Vector3 posi, Quaternion roti)
    {
        pos = posi;
        rot = roti;
    }

    [Command]
    private void CmdChangeRigidbody (Vector3 posi, Quaternion roti)
    {
        pos = posi;
        rot = roti;
    }

    public void OnChangePos(Vector3 newPos)
    {
        if (isLocalPlayer) return;
        if(syncType == Type.Tranform)
        {
            transform.position = newPos;
        }else if (syncType == Type.Rigidbody)
        {
            body.position = newPos;
        }
        
    }

    public void OnChangeRot(Quaternion newRot)
    {
        if (isLocalPlayer) return;
        if(syncType == Type.Tranform)
        {
            transform.rotation = newRot;
        }else if(syncType == Type.Rigidbody)
        {
            body.rotation = newRot;
        }
        
    }
}
