using UnityEngine;
using System.Collections;

public class SensitivityCare : MonoBehaviour
{

    private GameplaySettings settings;

    private void Start()
    {
        settings = GameObject.Find("GameplayManager").GetComponent<GameplaySettings>();
        Actualize();
    }

    public void Actualize()
    {
        gameObject.SendMessage("ChangeSensitivity", settings.settings.sensitivity / 10f);
    }
}
