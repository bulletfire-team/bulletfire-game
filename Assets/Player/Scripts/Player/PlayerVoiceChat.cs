using UnityEngine;
using UnityEngine.Networking;
using VoiceChat.Networking;
using VoiceChat.Demo;

public class PlayerVoiceChat : NetworkBehaviour
{

    public GameObject voiceChatProxyPref;

    private void Start()
    {
        if(isLocalPlayer)
        {
            if (isServer)
            {
                VoiceChatNetworkProxy.OnManagerStartServer();
                gameObject.AddComponent<VoiceChatServerUi>();
            }
            
            VoiceChatNetworkProxy.OnManagerClientConnect(connectionToServer);
            gameObject.AddComponent<VoiceChatUi>();
        }
        
    }

    private void OnDestroy()
    {
        if (isServer)
        {
            VoiceChatNetworkProxy.OnManagerStopServer();
            Destroy(GetComponent<VoiceChatServerUi>());
        }
        if (isLocalPlayer)
        {
            VoiceChatNetworkProxy.OnManagerStopClient();
        }
    }
}
