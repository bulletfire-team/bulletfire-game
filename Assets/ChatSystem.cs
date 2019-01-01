using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ChatSystem : MonoBehaviour
{
    private List<FriendMessages> friendMessages = new List<FriendMessages>();
    private Server server;

    private string friendTalking = null;


    public MenuUI menuUI;

    [Header("UI")]
    public Transform squadChatContainer;
    public Transform personalChatContainer;
    public GameObject chatMessageItem;
    public GameObject friendItem;
    public Transform friendContainer;


    private void Start ()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        server.GetChat();
        Callbacks();
    }

    public void SendSquadChat (TMP_InputField i_f)
    {
        Msg msg = new Msg();
        msg.Message = i_f.text; 
        server.SendSquadChat(msg);
        i_f.text = "";
    }

    public void SendPersonalChat (TMP_InputField i_f)
    {
        if (friendTalking == null) return; 
        Msg msg = new Msg();
        msg.Message = i_f.text;
        msg.Receiver = friendTalking;
        server.SendUserChat(msg);
    }

    public void OpenPersonalChat (string pseudo)
    {
        friendTalking = pseudo;
        foreach (Transform item in personalChatContainer)
        {
            Destroy(item.gameObject);
        }
        FriendMessages f = friendMessages.Find(u => u.friendName == pseudo);
        if(f != null)
        {
            foreach (Msg msg in f.messages.messages)
            {
                GameObject o = Instantiate(chatMessageItem, personalChatContainer);
                o.GetComponent<MessageItem>().Init(msg, server);
            }
        }
        menuUI.slideRightMenu(-1125);
        
    }

    public void ClosePersonalChat ()
    {
        friendTalking = null;
    }

    private void Callbacks ()
    {
        server.socket.On("NoUserFoundSPM", () =>
        {
           UnityThread.executeInUpdate(() =>
           {

           });
        });

        server.socket.On("SPMSuccess", (data) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                Msg msg = JsonUtility.FromJson<Msg>(data.ToString());
                msg.Sender = server.player.nickname;
                AddMessage(msg);
                if(msg.Receiver == friendTalking)
                {
                    GameObject o = Instantiate(chatMessageItem, personalChatContainer);
                    o.GetComponent<MessageItem>().Init(msg, server);
                }
            });
        });

        server.socket.On("NewPersonalMessage", (data) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                Debug.Log("Get new personal msg : " + data.ToString());
                Msg msg = JsonUtility.FromJson<Msg>(data.ToString());
                if(msg.Sender == friendTalking || msg.Receiver == friendTalking)
                {
                    GameObject o = Instantiate(chatMessageItem, personalChatContainer);
                    o.GetComponent<MessageItem>().Init(msg, server);
                    AddMessage(msg);
                }
                else
                {
                    AddMessage(msg);
                }
            });
        });

        server.socket.On("nosquad", () =>
        {
            UnityThread.executeInUpdate(() =>
            {

            });
        });

        server.socket.On("NewSquadMessage", (data) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                Msg msg = JsonUtility.FromJson<Msg>(data.ToString());
                GameObject o = Instantiate(chatMessageItem, squadChatContainer);
                o.GetComponent<MessageItem>().Init(msg, server);
            });
        });

        server.socket.On("getchat", (data) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                print(data.ToString());
                Msgs messages = JsonUtility.FromJson<Msgs>(data.ToString());
                foreach(Msg msg in messages.messages)
                {
                    AddMessage(msg);
                }
                foreach (Transform item in friendContainer)
                {
                    Destroy(item.gameObject);
                }
                foreach (FriendMessages item in friendMessages)
                {
                    GameObject o = Instantiate(friendItem, friendContainer);
                    o.GetComponent<FriendChatItem>().Init(item, this);
                }
            });
        });
    }

    private void AddMessage (Msg msg)
    {
        FriendMessages f = friendMessages.Find(u => u.friendName == msg.Receiver || u.friendName == msg.Sender);
        if(f != null)
        {
            f.AddMessage(msg);
        }
        else
        {
            FriendMessages fr = new FriendMessages();
            fr.friendName = msg.Sender != server.player.nickname ? msg.Sender : msg.Receiver;
            fr.messages = new Msgs();
            fr.AddMessage(msg);
            friendMessages.Add(fr);
        }
    }
}
