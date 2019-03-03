using UnityEngine;

public class LeaderboardMenu : MonoBehaviour
{

    private int page = 1;
    private Server server;

    public Transform leaderboardContainer;
    public GameObject entryPref;

    private void Start()
    {
        server = GameObject.Find("Server").GetComponent<Server>();
        GetPage();
        CallBack();
    }

    private void CallBack ()
    {
        server.socket.On("receiveleaderboard", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                print(evt.ToString());
                PlayerEntryList friends = JsonUtility.FromJson<PlayerEntryList>(evt.ToString());
                int pos = (page - 1) * 10;
                foreach(Transform t in leaderboardContainer)
                {
                    Destroy(t.gameObject);
                }
                foreach (PlayerEntry friend in friends.friends)
                {
                    pos++;
                    GameObject o = Instantiate(entryPref, leaderboardContainer);
                    o.GetComponent<LeaderboardEntry>().Init(friend, pos);
                }
            });
        });
    }

    private void OnDisable()
    {
        page = 1;
    }

    private void OnEnable()
    {
        GetPage();
    }

    public void ChangePage (int direction)
    {
        if(page + direction >= 1)
        {
            page += direction;
            GetPage();
        }
    }

    private void GetPage ()
    {
        if(server != null)
        {
            print("Get leaderboard");
            server.GetLeaderBoard(page);
        }
        
    }
}
