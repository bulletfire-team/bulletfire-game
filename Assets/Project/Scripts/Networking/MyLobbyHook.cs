using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class MyLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        Player player = gamePlayer.GetComponent<Player>();

        player.team = lobby.playerTeam;
        player.pseudo = lobby.playerName;
        player.Init();
    }
}
