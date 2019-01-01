using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObj
{
    public string netId;
    public Player player;
    public int nbKill = 0;
    public int nbAssist = 0;
    public int nbDeath = 0;

    public PlayerObj (string netId, Player player) {
        this.netId = netId;
        this.player = player;
    }

}

[System.Serializable]
public class PlayerGameStat
{
    public string pseudo;
    public int nbKill;
    public int nbAssist;
    public int nbDeath;
    public int win;
}

public class GameManager : NetworkBehaviour {

    public List<PlayerObj> players = new List<PlayerObj>();

    public int playerAliveRedTeam = 0;
    public int playerAliveBlueTeam = 0;

    public int redScore = 0;
    public int blueScore = 0;

    private int nbPlayerRed = 0;
    private int nbPlayerBlue = 0;

    private NetworkLobbyManager lobby;

    private bool gameStarted = false;

    public static GameManager instance = null;

    public GameObject[] blueSpawns;
    public GameObject[] redSpawns;

    private List<NetworkInstanceId> redPeople = new List<NetworkInstanceId>();
    private List<NetworkInstanceId> bluePeople = new List<NetworkInstanceId>();


    public void SetInstance(GameManager gameManager)
    {
        instance = gameManager;
        blueSpawns = GameObject.FindGameObjectsWithTag("BlueSpawn");
        redSpawns = GameObject.FindGameObjectsWithTag("RedSpawn");
    }

    public void RegisterPlayer (string netId, Player player)
    {
        lobby = GameObject.Find("Manager").GetComponent<NetworkLobbyManager>();
        players.Add(new PlayerObj(netId, player));
        player.transform.name = "Player " + netId;
        Transform point = null;
        if (player.team == "red")
        {
            playerAliveRedTeam++;
            nbPlayerRed++;
            redPeople.Add(player.netId);
            point = redSpawns[playerAliveRedTeam - 1].transform;
        }
        else if (player.team == "blue")
        {
            playerAliveBlueTeam++;
            nbPlayerBlue++;
            bluePeople.Add(player.netId);
            point = blueSpawns[playerAliveBlueTeam - 1].transform;
        }
        StartCoroutine(waitSendInfos(point, player));
        
        if(!gameStarted && players.Count == lobby.maxPlayers)
        {
            gameStarted = true;
            StartCoroutine(waitStartGame());
        }
        
    }

    public void UnregisterPlayer (string netId)
    {
        PlayerObj p = players.Find(u => u.netId == netId);
        Player pl = p.player;

        if (!pl.isDead)
        {
            if(pl.team == "red")
            {
                playerAliveRedTeam--;
                nbPlayerRed--;
            }else if(pl.team == "blue")
            {
                playerAliveBlueTeam--;
                nbPlayerBlue--;
            }
        }

        players.Remove(p);

        foreach (PlayerObj pla in players)
        {
            pla.player.TargetNbPlayerChanged(pla.player.connectionToClient, playerAliveRedTeam, playerAliveBlueTeam);
        }
    }

    public Player GetPlayer (string netId)
    {
        return players.Find(u => u.netId == netId).player;
    }

    public void PlayerGone (string killednetId, string killernetId)
    {
        players.Find(u => u.netId == killernetId).nbKill++;
        players.Find(u => u.netId == killednetId).nbDeath++;
        Player killerP = players.Find(u => u.netId == killernetId).player;
        Player victimP = players.Find(u => u.netId == killednetId).player;
        string[] killer = new string[2] { killerP.team, killerP.pseudo };
        string[] victim = new string[2] { victimP.team, victimP.pseudo };
        foreach (PlayerObj pla in players)
        {
            pla.player.TargetPlayerGone(pla.player.connectionToClient, killer, victim);
        }
    }

    public void PlayerDied (string netId, string killernetId)
    {
        players.Find(u => u.netId == killernetId).nbKill++;
        players.Find(u => u.netId == netId).nbDeath++;
        Player pl = players.Find(u => u.netId == netId).player;
        if(pl.team == "red")
        {
            playerAliveRedTeam--;
        }else if(pl.team == "blue")
        {
            playerAliveBlueTeam--;
        }
        string[] victim = new string[2] { pl.team, pl.pseudo };
        Player killerP = players.Find(u => u.netId == killernetId).player;
        string[] killer = new string[2] { killerP.team, killerP.pseudo };
        foreach (PlayerObj player in players)
        {
            player.player.TargetPlayerDied(player.player.connectionToClient, playerAliveRedTeam, playerAliveBlueTeam);
            player.player.TargetPlayerGone(player.player.connectionToClient, killer, victim);
        }

        if(playerAliveBlueTeam == 0)
        {
            // Red team win the round
            redScore++;
            foreach (PlayerObj item in players)
            {
                if(item.player.team == "red")
                {
                    item.player.TargetGetMoney(item.player.connectionToClient, 2500);
                }
                else
                {
                    item.player.TargetGetMoney(item.player.connectionToClient, 1500);
                }
            }
            playerAliveBlueTeam = nbPlayerBlue;
            playerAliveRedTeam = nbPlayerRed;
            startRound();
        }else if(playerAliveRedTeam == 0)
        {
            // Blue team win the round
            blueScore++;
            foreach (PlayerObj item in players)
            {
                if (item.player.team == "blue")
                {
                    item.player.TargetGetMoney(item.player.connectionToClient, 2500);
                }
                else
                {
                    item.player.TargetGetMoney(item.player.connectionToClient, 1500);
                }
            }
            playerAliveBlueTeam = nbPlayerBlue;
            playerAliveRedTeam = nbPlayerRed;
            startRound();
        }
    }

    private void startRound ()
    {
        if(redScore == 5 || blueScore == 5)
        {
            foreach (PlayerObj player in players)
            {
                player.player.TargetEndGame(player.player.connectionToClient, redScore == 5);
            }
            EndGame(redScore == 5);
            return;
        }
        foreach (PlayerObj player in players)
        {
            player.player.TargetBeginRound(player.player.connectionToClient, redScore, blueScore, playerAliveRedTeam, playerAliveBlueTeam);
        }
    }

    private void EndGame (bool isRedWin)
    {
        Server serv = GameObject.Find("Server").GetComponent<Server>();
        foreach (PlayerObj player in players)
        {
            PlayerGameStat stats = new PlayerGameStat();
            bool isRed = player.player.team == "red";
            stats.pseudo = player.player.pseudo;
            stats.nbKill = player.nbKill;
            stats.nbAssist = player.nbAssist;
            stats.nbDeath = player.nbDeath;
            stats.win = ((isRedWin && isRed) || (!isRedWin && !isRed)) ? 1 : 0;
            serv.UpdatePlayerStats(stats);
        }
        serv.EndGame();
    }

    IEnumerator waitStartGame ()
    {
        yield return new WaitForSeconds(1f);

        List<string> red = new List<string>();
        List<string> blue = new List<string>();
        foreach (PlayerObj item in players)
        {
            if (item.player.team == "red")
            {
                red.Add(item.player.pseudo);
            }
            else
            {
                blue.Add(item.player.pseudo);
            }
        }

        foreach (PlayerObj player in players)
        {
            player.player.TargetGetPlayers(player.player.connectionToClient, red.ToArray(), blue.ToArray());
            player.player.TargetPeopleOnMyTeam(player.player.connectionToClient, player.player.team == "red" ? redPeople.ToArray() : bluePeople.ToArray());
        }
        startRound();
    }

    IEnumerator waitSendInfos (Transform point, Player p)
    {
        yield return new WaitForSeconds(1f);
        p.TargetFirstPoint(p.connectionToClient, point.position, point.rotation);
        foreach (PlayerObj pl in players)
        {
            pl.player.TargetNbPlayerChanged(pl.player.connectionToClient, playerAliveRedTeam, playerAliveBlueTeam);
        }
    }

    // Chat System
    #region Chat System
    public void SendTeamChat (string msg, string team, string pseudo)
    {
        foreach (PlayerObj pl in players)
        {
            if (pl.player.team == team)
            {
                pl.player.GetComponent<PlayerChat>().TargetReceiveTeam(pl.player.connectionToClient, msg, pseudo);
            }
        }
    }

    public void SendPlayerChat (string msg, string player, string pseudo)
    {
        foreach (PlayerObj pl in players)
        {
            if (pl.player.pseudo == player ||pl.player.pseudo == pseudo)
            {
                pl.player.GetComponent<PlayerChat>().TargetReceivePlayer(pl.player.connectionToClient, msg, pseudo);
            }
        }
    }

    public void SendGlobalChat (string msg, string pseudo)
    {
        foreach (PlayerObj pl in players)
        {
            pl.player.GetComponent<PlayerChat>().TargetReceiveAll(pl.player.connectionToClient, msg, pseudo);
        }
    }
    #endregion

}
