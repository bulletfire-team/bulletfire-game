using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{

    public InputKey[] keyCodes;

    private bool isChanging = false;

    private InputKey change;

    public delegate void OnKeyChanged();

    public OnKeyChanged hook;

    private void Awake()
    {
        GetInputs();
    }

    private void OnGUI()
    {
        if(isChanging)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                change.keyCode = e.keyCode;
                isChanging = false;
                hook();
                SaveInputs();
            }else if (e.isMouse)
            {
                KeyCode key = KeyCode.Mouse0;
                switch (e.button)
                {
                    case 0:
                        key = KeyCode.Mouse0;
                        break;
                    case 1:
                        key = KeyCode.Mouse1;
                        break;
                    case 2:
                        key = KeyCode.Mouse2;
                        break;
                    case 3:
                        key = KeyCode.Mouse3;
                        break;
                    case 4:
                        key = KeyCode.Mouse4;
                        break;
                    case 5:
                        key = KeyCode.Mouse5;
                        break;
                    case 6:
                        key = KeyCode.Mouse6;
                        break;
                }
                change.keyCode = key; 
                isChanging = false;
                hook();
                SaveInputs();
            }
        }
    }

    public KeyCode GetKeyCode(string name)
    {
        return Array.Find(keyCodes, key => key.name == name).keyCode;
    }

    public void ChangeKeyCode (string name, OnKeyChanged callback)
    {
        isChanging = true;
        hook = callback;
        change = Array.Find(keyCodes, key => key.name == name);
    }

    private void SaveInputs ()
    {
        Keys keys = new Keys();
        keys.inputKeys = keyCodes;
        string serialized = JsonUtility.ToJson(keys);
        PlayerPrefs.SetString("Inputs", serialized);
    }

    private void GetInputs ()
    {
        if (PlayerPrefs.HasKey("Inputs"))
        {
            string serialized = PlayerPrefs.GetString("Inputs");
            Keys deserialized = JsonUtility.FromJson<Keys>(serialized);
            keyCodes = deserialized.inputKeys;
        }
    }
}
