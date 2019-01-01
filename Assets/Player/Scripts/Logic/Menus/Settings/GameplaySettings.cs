using UnityEngine;

public class GameplaySettings : MonoBehaviour
{

    public GameplaySettingsEntity settings;
    

    private void Start()
    {
        GetSettings();
    }

    private void GetSettings ()
    {
        if (PlayerPrefs.HasKey("GameplaySettings"))
        {
            string serialized = PlayerPrefs.GetString("GameplaySettings");
            GameplaySettingsEntity deserialized = JsonUtility.FromJson<GameplaySettingsEntity>(serialized);
            settings = deserialized;
        }
    }

    public void SaveSettings()
    {
        string serialized = JsonUtility.ToJson(settings);
        PlayerPrefs.SetString("GameplaySettings", serialized);
    }
}
