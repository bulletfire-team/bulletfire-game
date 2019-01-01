using UnityEngine;
using Quobject.SocketIoClientDotNet.Client;
using UnityEngine.UI;
using Prototype.NetworkLobby;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System;

public class MatchMakingManager : MonoBehaviour
{

    private Server server;

    [Header("UI")]
    public GameObject searchingObj;
    public GameObject foundObj;

    public LobbyManager lobby;
    public LobbyMainMenu main;

    public Match m;
    public Game g;
    public bool isRed;

    // Use this for initialization
    void Start()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        ServerCallbacks();
    }

    public void StartMatchmaking (int type)
    {
        server.StartMatchmaking(type);
    }

    public void StartTestMM (Slider s)
    {
        GameStruct st = new GameStruct();
        st.nbPlayer = ((int)s.value * 2);
        server.StartTestMM(JsonUtility.ToJson(st));
    }

    public void StopMatchmaking ()
    {
        server.StopMatchmaking();
    }

    public void HostReady (NetworkID gameNetId)
    {
        server.HostReady((ulong)gameNetId);
    }

    private void ServerCallbacks ()
    {
        Socket socket = server.socket;

        socket.On("MMStartMatchMaking", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                searchingObj.SetActive(true);
            });
        });

        socket.On("MMWaitingPlayers", (evt) =>
        {

        });

        socket.On("MMAlreadySearching", (evt) =>
        {

        });

        socket.On("MMNotLeader", (evt) =>
        {

        });

        socket.On("MMStopMatchMaking", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                searchingObj.SetActive(false);
            });
        });

        socket.On("MMNotSearching", (evt) =>
        {

        });

        socket.On("MMFoundGame", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                Game game = JsonUtility.FromJson<Game>(evt.ToString());
                g = game;
                // Show ui stuff
                foundObj.SetActive(true);

                if (game.host)
                {
                    isRed = game.isRed;
                    lobby.gameObject.SetActive(true);
                    lobby.maxPlayers = game.nbPlayer;
                    lobby.minPlayers = game.nbPlayer;
                    lobby.matchName = game.gameId.ToString();
                    // Create a game
                    main.OnClickCreateMatchmakingGame();
                }
            });
        });

        socket.On("MMGetNetGame", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                Match match = JsonUtility.FromJson<Match>(evt.ToString());
                // Connect to match
                m = match;
                isRed = match.isRed;
                lobby.gameObject.SetActive(true);
                print("match id : " + match.matchId);
                lobby.StartMatchMaker();
                lobby.backDelegate = lobby.SimpleBackClbk;
                lobby.matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
            });
        });
    }

    public void OnMatchList (bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        print("get list : " + matches.Count);
        foreach (MatchInfoSnapshot item in matches)
        {
            print(item.networkId);
            string newItem = item.networkId.ToString();
            string newMatch = m.matchId.ToString();
            newItem = newItem.Substring(0, newItem.Length - 3);
            newMatch = newMatch.Substring(0, newMatch.Length - 3);
            print(newItem);
            print(newMatch);
            if(newItem == newMatch)
            {
                print("join");
                lobby.matchMaker.JoinMatch(item.networkId, "", "", "", 0, 0, lobby.OnMatchJoined);
                lobby.backDelegate = lobby.StopClientClbk;
                lobby._isMatchmaking = true;
                lobby.DisplayIsConnecting();
            }
        }
    }


    public void StartLocalPlayer (LobbyPlayer pla)
    {
        pla.OnSetTeam(isRed ? "red" : "blue");
    }

}