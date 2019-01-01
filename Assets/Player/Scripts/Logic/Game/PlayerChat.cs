using UnityEngine;
using UnityEngine.Networking;

public class PlayerChat : NetworkBehaviour
{
    private PlayerUIMenu chat;

    private void Start()
    {
        if (!isLocalPlayer) return;
        chat = GameObject.Find("UI").GetComponent<PlayerUIMenu>();
        chat.chatSystem = this;
    }

    #region Command
    [Command]
    public void CmdSendAll (string msg)
    {
        GameManager.instance.SendGlobalChat(msg, GetComponent<Player>().pseudo);

    }

    [Command]
    public void CmdSendTeam (string msg)
    {
        GameManager.instance.SendTeamChat(msg, GetComponent<Player>().team, GetComponent<Player>().pseudo);
    }

    [Command]
    public void CmdSendPlayer (string msg, NetworkInstanceId player)
    {
        //GameManager.instance.SendPlayerChat(msg, player, GetComponent<Player>().pseudo);
    }
    #endregion

    #region RPC
    [TargetRpc]
    public void TargetReceiveAll (NetworkConnection target, string msg, string pseudo)
    {
        chat.ReceiveMsg(msg, 1, pseudo == GetComponent<Player>().pseudo, pseudo);
    }

    [TargetRpc]
    public void TargetReceiveTeam (NetworkConnection target, string msg, string pseudo)
    {
        chat.ReceiveMsg(msg, 3, pseudo == GetComponent<Player>().pseudo, pseudo);
    }

    [TargetRpc]
    public void TargetReceivePlayer (NetworkConnection target, string msg, string pseudo)
    {
        chat.ReceiveMsg(msg, 4, pseudo == GetComponent<Player>().pseudo, pseudo);
    }
    #endregion
}
