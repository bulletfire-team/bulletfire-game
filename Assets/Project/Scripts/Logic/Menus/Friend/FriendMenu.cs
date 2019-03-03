using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Quobject.SocketIoClientDotNet.Client;

public class FriendMenu : MonoBehaviour
{

    private Server server;

    public ChatSystem chat;

    [Header("UI")]
    public Transform conFriendsContainer;
    public Transform decoFriendsContainer;
    public Transform requestFriendContainer;
    public GameObject friendItem;
    public GameObject requestItem;
    public GameObject squadRequestObj;
    public TMP_Text squadRequestTxt;
    public Transform squadMembersContainer;
    public GameObject squadMemberItem;
    public GameObject squadObj;

    private int squadId;

    private List<FriendItem> friendItems = new List<FriendItem>();
    private List<RequestItem> requestItems = new List<RequestItem>();
    private List<SquadMemberItem> squadMembers = new List<SquadMemberItem>();

    private void Start()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        ServerCallbacks();
        server.GetFriends();
        server.GetFriendRequests();
    }

    public void SendFriendRequest (TMP_InputField ifPseudo)
    {
        server.SendFriendRequest(ifPseudo.text);
    }

    public void AcceptFriendRequest (string pseudo)
    {
        server.AcceptFriendRequest(pseudo);
    }

    public void RefuseFriendRequest (string pseudo)
    {
        server.RefuseFriendRequest(pseudo);
    }

    public void InviteFriendIntoSquad (string pseudo)
    {
        server.InviteFriendIntoSquad(pseudo);
    }

    public void AcceptSquadInvitation ()
    {
        squadRequestObj.SetActive(false);
        server.AcceptSquadInvitation(squadId);
    }

    public void RefuseSquadInvitation ()
    {
        squadRequestObj.SetActive(false);
        server.RefuseSquadRequest(squadId);
    }

    public void KickUserFromSquad (string pseudo)
    {
        server.KickUserFromSquad(pseudo);
    }

    public void LeaveSquad ()
    {
        server.LeaveSquad();
    }

    public void DeleteFriend (string pseudo)
    {
        server.DeleteFriend(pseudo);
    }
    
    private void ServerCallbacks ()
    {
        Socket socket = server.socket;

        /*  ###   Friend Management   ###  */

        // Friend connection / disconnection
        socket.On("FriendConnection", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                    FriendItem friendItemFound = friendItems.Find(f => f.friend.nickname == friend.nickname);
                    if (friendItemFound != null)
                    {
                        friendItemFound.transform.parent = conFriendsContainer;
                        friendItemFound.Connect();
                    }
                    else
                    {
                        GameObject obj = Instantiate(friendItem, conFriendsContainer);
                        obj.GetComponent<FriendItem>().Init(this, friend);
                        friendItems.Add(obj.GetComponent<FriendItem>());
                    }
                }
            });
        }); 

        socket.On("FriendDisconnection", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                    FriendItem friendItemFound = friendItems.Find(f => f.friend.nickname == friend.nickname);
                    if (friendItemFound != null)
                    {
                        friendItemFound.transform.parent = decoFriendsContainer;
                        friendItemFound.Disconnect();
                    }
                    else
                    {
                        GameObject obj = Instantiate(friendItem, decoFriendsContainer);
                        obj.GetComponent<FriendItem>().Init(this, friend);
                        friendItems.Add(obj.GetComponent<FriendItem>());
                    }
                }
            });
        });

        // Get Friends
        socket.On("OnReceiveFriends", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friends friends = JsonUtility.FromJson<Friends>(evt.ToString());
                    foreach (Friend f in friends.friends)
                    {
                        GameObject obj;
                        if (f.isConnected)
                        {
                            obj = Instantiate(friendItem, conFriendsContainer);
                        }
                        else
                        {
                            obj = Instantiate(friendItem, decoFriendsContainer);
                        }

                        obj.GetComponent<FriendItem>().Init(this, f);
                        friendItems.Add(obj.GetComponent<FriendItem>());
                    }
                }
            });
        }); 

        // Send friend request
        socket.On("NoUserFoundSFR", (evt) =>
        {

        });

        socket.On("AlreadyFriend", (evt) =>
        {

        });

        socket.On("SendFriendRequestSuccess", (evt) =>
        {

        });

        // Accept friend request
        socket.On("NoRequestFound", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                    RequestItem friendItemFound = requestItems.Find(f => f.friend.nickname == friend.nickname);
                    if (friendItemFound != null)
                    {
                        Destroy(friendItemFound.gameObject);
                    }
                }
            });
        });

        socket.On("AcceptFriendRequestSuccess", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                    RequestItem friendItemFound = requestItems.Find(f => f.friend.nickname == friend.nickname);
                    if (friendItemFound != null)
                    {
                        Destroy(friendItemFound.gameObject);
                    }
                }
            });
        });

        // Refuse friend request
        socket.On("RefuseFriendRequestSuccess", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                    RequestItem friendItemFound = requestItems.Find(f => f.friend.nickname == friend.nickname);
                    if (friendItemFound != null)
                    {
                        Destroy(friendItemFound.gameObject);
                    }
                }
            });
        });

        // Delete friend
        socket.On("DeleteFriendSuccess", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                    FriendItem friendItemFound = friendItems.Find(f => f.friend.nickname == friend.nickname);
                    if (friendItemFound != null)
                    {
                        Destroy(friendItemFound.gameObject);
                        friendItems.Remove(friendItemFound);
                    }
                }
                
            });
        });

        // Friend request
        socket.On("OnReceiveFriendRequests", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friends friends = JsonUtility.FromJson<Friends>(evt.ToString());
                    foreach (Friend f in friends.friends)
                    {
                        GameObject obj = Instantiate(requestItem, requestFriendContainer);
                        obj.GetComponent<RequestItem>().Init(this, f);
                        requestItems.Add(obj.GetComponent<RequestItem>());
                    }
                }
                
            });
        });

        socket.On("NewFriendRequest", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                if (!string.IsNullOrEmpty(evt.ToString()))
                {
                    Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                    GameObject obj = Instantiate(requestItem, requestFriendContainer);
                    obj.GetComponent<RequestItem>().Init(this, friend);
                    requestItems.Add(obj.GetComponent<RequestItem>());
                }
            });
        });


        /*  ###   Squad Management   ###  */

        // Invite friend into squad
        socket.On("FriendNotConnected", (evt) =>
        {

        }); // done

        socket.On("NotAFriendIFIS", (evt) =>
        {

        }); // done

        socket.On("InviteFriendIntoSquadSuccess", (evt) =>
        {

        }); // done

        // Create squad
        socket.On("OnCreateSquad", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                squadObj.SetActive(true);

                Friends friends = JsonUtility.FromJson<Friends>(evt.ToString());
                foreach (Friend f in friends.friends)
                {
                    GameObject obj = Instantiate(squadMemberItem, squadMembersContainer);
                    obj.GetComponent<SquadMemberItem>().Init(this, f);
                    squadMembers.Add(obj.GetComponent<SquadMemberItem>());
                }
            });
        }); // done

        // Receive squad request
        socket.On("ReceiveSquadRequest", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                SquadRequest squadRequest = JsonUtility.FromJson<SquadRequest>(evt.ToString());
                squadRequestObj.SetActive(true);
                squadRequestTxt.text = squadRequest.nickname + " vous invite dans une escouade";
                squadId = squadRequest.squadId;
            });
        }); // done

        socket.On("NoSquadFound", (evt) =>
        {

        }); // done

        socket.On("RefuseSquadRequestSuccess", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                squadId = 0;
            });
        }); // done

        // Join squad
        socket.On("OnJoinSquad", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                squadObj.SetActive(true);

                Friends friends = JsonUtility.FromJson<Friends>(evt.ToString());
                foreach (Friend f in friends.friends)
                {
                    GameObject obj = Instantiate(squadMemberItem, squadMembersContainer);
                    obj.GetComponent<SquadMemberItem>().Init(this, f);
                    squadMembers.Add(obj.GetComponent<SquadMemberItem>());
                }
            });
        }); // done

        // New user in squad
        socket.On("NewUserInSquad", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                GameObject obj = Instantiate(squadMemberItem, squadMembersContainer);
                obj.GetComponent<SquadMemberItem>().Init(this, friend);
                squadMembers.Add(obj.GetComponent<SquadMemberItem>());
            });
        }); // done

        // Leave squad
        socket.On("NotInASquad", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                NoSquad();
            });
        }); // done

        socket.On("LeaveSquadSuccess", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                NoSquad();
            });
        }); // done 

        // User leave squad
        socket.On("UserLeaveSquad", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                SquadMemberItem squadMFound = squadMembers.Find(f => f.friend.nickname == friend.nickname);
                if (squadMFound != null)
                {
                    Destroy(squadMFound);
                }
            });
        }); // done

        // Change leader
        socket.On("ChangeLeader", (evt) =>
        {

        }); // done

        // Kick user from squad
        socket.On("NotSquadAdmin", (evt) =>
        {

        }); // done

        socket.On("KickUserFromSquadSuccess", (evt) =>
        {

        }); // done

        // User kicked
        socket.On("OnUserKicked", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                Friend friend = JsonUtility.FromJson<Friend>(evt.ToString());
                SquadMemberItem friendItemFound = squadMembers.Find(f => f.friend.nickname == friend.nickname);
                if (friendItemFound != null)
                {
                    Destroy(friendItemFound.gameObject);
                }
            });
        }); // done

        // OnKicked
        socket.On("OnKicked", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                NoSquad();
            });
        }); // done

        // Delete squad
        socket.On("DeleteSquad", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                NoSquad();
            });
        });
        
    }

    public void Talk (string pseudo)
    {
        chat.OpenPersonalChat(pseudo);
    }

    private void NoSquad ()
    {
        squadId = 0;
        squadObj.SetActive(false);
        foreach (Transform t in squadMembersContainer)
        {
            Destroy(t.gameObject);
        }
        squadMembers.Clear();
    }
}