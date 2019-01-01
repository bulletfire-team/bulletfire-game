using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioManager))]
public class MyNetworkAudioManager : NetworkBehaviour
{

    public AudioManager audioManager;

    #region Rpc Client
    [ClientRpc]
    public void RpcPlay (string name, int clip)
    {
        audioManager.Play(name, clip);
    }

    [ClientRpc]
    public void RpcPause (string name)
    {
        audioManager.Pause(name);
    }

    [ClientRpc]
    public void RpcStop (string name)
    {
        audioManager.Stop(name);
    }

    [ClientRpc]
    public void RpcReinit ()
    {
        audioManager.Reinit();
    }
    #endregion

    #region Command
    [Command]
    public void CmdPlay (string name, int clip)
    {
        RpcPlay(name, clip);
    }

    [Command]
    public void CmdPause (string name)
    {
        RpcPause(name);
    }

    [Command]
    public void CmdStop (string name)
    {
        RpcStop(name);
    }

    [Command]
    public void CmdReinit ()
    {
        RpcReinit();
    }
    #endregion
}
