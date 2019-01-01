using UnityEngine;
using System.Collections;
using DiscordPresence;

public class DiscordRPManager : MonoBehaviour
{
    public void StartMenu ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans les menus");
    }

    public void StartSquad ()
    {
        PresenceManager.UpdatePresence(detail: "En escouade (1 sur 4)");
    }

    public void StopSquad ()
    {
        PresenceManager.UpdatePresence(detail: "En solo");
    }

    public void SquadNbChange (int nb)
    {
        PresenceManager.UpdatePresence(detail: "En escouade (" + nb + " sur 4)");
    }

    public void StartSearchingQuick ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Recherche de partie rapide");
    }

    public void StopSearchingQuick ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans les menus");
    }

    public void StartSearchingRank ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Recherche de partie classée");
    }

    public void StopSearchingRank ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans les menus");
    }

    public void StartQuick ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans une partie rapide");
    }

    public void StopQuick ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans les menus");
    }

    public void StartRank ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans une partie classée");
    }

    public void StopRank ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans les menus");
    }

    public void StartCustom ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans une partie personnalisée");
    }

    public void StopCustom ()
    {
        PresenceManager.UpdatePresence(detail: null, state: "Dans les menus");
    }
}
