using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class ConnectionMenu : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField mailIF;
    public TMP_InputField passIF;

    public GameObject errorCon;

    public GameObject loading;

    public GameObject loadingScene;

    public Toggle stayConnected;

    public void Connect ()
    {
        loading.SetActive(true);
        print("Try to connect menu");
        Server serv = GameObject.Find("Server").GetComponent<Server>();
        PlayerEntity player = new PlayerEntity(mailIF.text, passIF.text);
        serv.TryToConnect(player, stayConnected.isOn);
        serv.socket.On("ErrorCon", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                loading.SetActive(false);
                errorCon.SetActive(true);
            });
        });

        serv.socket.On("SuccesCon", (evt) =>
        {
            UnityThread.executeInUpdate(() =>
            {
                loadingScene.SetActive(true);
            });
        });
    }

}
