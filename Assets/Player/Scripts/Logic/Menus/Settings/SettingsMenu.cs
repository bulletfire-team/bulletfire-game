using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private InputManager inputManager;
    private AudioSettings audioSettings;
    private GameplaySettings gameplaySettings;

    [Header("UI")]

    [Header("Commands")]
    public Transform keyContainer;
    public GameObject keyItem;
    public GameObject clickMenu;

    [Header("Audio")]
    public Slider effectVolume;
    public Slider ambianceVolume;
    public Slider voiceVolume;
    public Slider chatVolume;

    [Header("Gameplay")]
    public Slider sensitivity;
    public Toggle showkillfeed;
    public Toggle showchat;

    private void Start()
    {
        inputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
        ShowKeys();
        
        audioSettings = GameObject.Find("AudioManager").GetComponent<AudioSettings>();
        ShowAudioSettings();

        gameplaySettings = GameObject.Find("GameplayManager").GetComponent<GameplaySettings>();
        ShowGameplaySettings();
    }

    #region Commands
    private void ShowKeys ()
    {
        foreach (Transform i in keyContainer)
        {
            Destroy(i.gameObject);
        }
        foreach (InputKey item in inputManager.keyCodes)
        {
            GameObject obj = Instantiate(keyItem, keyContainer);
            obj.GetComponent<KeyItem>().Init(this, item);
        }
    }

    public void ChangeKey(string name)
    {
        clickMenu.SetActive(true);
        inputManager.ChangeKeyCode(name, OnKeyChanged);
    }

    public void OnKeyChanged ()
    {
        clickMenu.SetActive(false);
        ShowKeys();
    }
    #endregion

    #region Audio
    private void ShowAudioSettings ()
    {
        effectVolume.value = audioSettings.settings.effectsVolume;
        ambianceVolume.value = audioSettings.settings.ambianceVolume;
        voiceVolume.value = audioSettings.settings.voiceVolume;
        chatVolume.value = audioSettings.settings.chatVolume;
    }
    public void ChangeAudioSettings (int which)
    {
        switch(which)
        {
            case 1:
                audioSettings.settings.effectsVolume = (int)effectVolume.value;
                break;
            case 2:
                audioSettings.settings.ambianceVolume = (int)ambianceVolume.value;
                break;
            case 3:
                audioSettings.settings.voiceVolume = (int)voiceVolume.value;
                break;
            case 4:
                audioSettings.settings.chatVolume = (int)chatVolume.value;
                break;
        }
        audioSettings.SaveSettings();
    }
    #endregion

    #region Gameplay
    private void ShowGameplaySettings ()
    {
        sensitivity.value = gameplaySettings.settings.sensitivity;
        showkillfeed.isOn = gameplaySettings.settings.showKillfeed;
        showchat.isOn = gameplaySettings.settings.showChat;
    }

    public void ChangeGameplaySettings (int which)
    {
        switch(which)
        {
            case 1:
                gameplaySettings.settings.sensitivity = (int)sensitivity.value;
                break;
            case 2:
                gameplaySettings.settings.showKillfeed = showkillfeed.isOn;
                break;
            case 3:
                gameplaySettings.settings.showChat = showchat.isOn;
                break;
        }
        gameplaySettings.SaveSettings();
    }
    #endregion

    #region Connection
    public void logout()
    {
        if (PlayerPrefs.HasKey("mail"))
        {
            PlayerPrefs.DeleteKey("mail");
        }
        if (PlayerPrefs.HasKey("pass"))
        {
            PlayerPrefs.DeleteKey("pass");
        }
        GameObject.Find("Server").GetComponent<Server>().socket.Disconnect();
        SceneManager.LoadScene("Log");
    }
    #endregion
}
