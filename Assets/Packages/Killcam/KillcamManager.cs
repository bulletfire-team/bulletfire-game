using UnityEngine;
using System;

public class KillcamManager : MonoBehaviour
{
    public static float timeToSave;
    public Action StartRewind;
    public Action StopRewind;

    public static KillcamManager instance;

    public void StartingRewind ()
    {
        StartRewind();
    }

    public void StopingRewind ()
    {
        StopingRewind();
    }

    public void OnSpawn(GameObject obj, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent = null)
    {

    }

    public void OnAnim()
    {

    }
}
