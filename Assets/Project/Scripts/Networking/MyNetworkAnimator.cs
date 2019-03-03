using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkAnimator : NetworkBehaviour
{
    public Animator anim;

    #region Client RPC
    [ClientRpc]
    public void RpcSetTrigger (string name)
    {
        anim.SetTrigger(name);
    }

    [ClientRpc]
    public void RpcSetBool (string name, bool value)
    {
        anim.SetBool(name, value);
    }

    [ClientRpc]
    public void RpcSetInt(string name, int value)
    {
        anim.SetInteger(name, value);
    }

    [ClientRpc]
    public void RpcSetFloat(string name, float value)
    {
        anim.SetFloat(name, value);
    }
    #endregion

    #region Command
    [Command]
    public void CmdSetTrigger(string name)
    {
        RpcSetTrigger(name);
    }

    [Command]
    public void CmdSetBool(string name, bool value)
    {
        RpcSetBool(name, value);
    }

    [Command]
    public void CmdSetInt(string name, int value)
    {
        RpcSetInt(name, value);
    }

    [Command]
    public void CmdSetFloat(string name, float value)
    {
        RpcSetFloat(name, value);
    }
    #endregion

}
