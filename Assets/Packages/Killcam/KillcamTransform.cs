using UnityEngine;
using System.Collections.Generic;

public class KillcamTransform : MonoBehaviour
{
    private List<TransformData> datas = new List<TransformData>();

    private bool isRewinding = false;
    private float seconds = 0f;

    private void Start()
    {
        seconds = KillcamManager.timeToSave;
        KillcamManager.instance.StartRewind += StartRewind;
        KillcamManager.instance.StopRewind += StopRewind;
    }

    private void FixedUpdate()
    {
        if(isRewinding)
        {
            Rewind();
        }
        else
        {
            GetDatas();
        }
    }

    private void GetDatas ()
    {
        if(datas.Count > Mathf.Round(seconds / Time.fixedDeltaTime))
        {
            datas.RemoveAt(datas.Count - 1);
        }
        datas.Insert(0, new TransformData(transform.position, transform.rotation, transform.localScale));
    }

    private void Rewind ()
    {
        if(datas.Count > 0)
        {
            TransformData data = datas[0];
            transform.position = data.position;
            transform.rotation = data.rotation;
            transform.localScale = data.scale;
        }
    }

    public void StartRewind ()
    {
        isRewinding = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void StopRewind ()
    {
        isRewinding = false;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }
}
