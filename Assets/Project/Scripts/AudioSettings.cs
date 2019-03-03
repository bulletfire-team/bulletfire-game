using UnityEngine;

public class AudioSettings : MonoBehaviour {

    public AudioSettingsEntity settings;


    private void Start()
    {
        GetSettings();
    }

    private void GetSettings ()
    {
        if(PlayerPrefs.HasKey("AudioSettings"))
        {
            string serialized = PlayerPrefs.GetString("AudioSettings");
            AudioSettingsEntity deserialized = JsonUtility.FromJson<AudioSettingsEntity>(serialized);
            settings = deserialized;
        }
    }

    public void SaveSettings ()
    {
        string serialized = JsonUtility.ToJson(settings);
        PlayerPrefs.SetString("AudioSettings", serialized);
    }


}
