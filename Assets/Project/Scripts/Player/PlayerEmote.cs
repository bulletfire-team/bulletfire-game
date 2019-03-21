using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerEmote : NetworkBehaviour
{
    public PlayerAtributes attr;
    public Animator anim;
    private bool needMove = false;
    public Vector3 targetPosition;

    private void Start()
    {
        if(isLocalPlayer)
        {
            GameObject.Find("EmoteMenu").GetComponent<EmoteUI>().SetPlayerEmote(this);
        }
    }

    private void Update()
    {
        if(needMove)
        {
            if(Vector3.Distance(targetPosition, attr.emoteCamera.transform.localPosition) > 0.1)
            {
                float step = Time.deltaTime * 5f;
                attr.emoteCamera.transform.localPosition = Vector3.MoveTowards(attr.emoteCamera.transform.localPosition, targetPosition, step);
            }
            else
            {
                needMove = false;
            }
        }
    }

    #region Public void
    public void PlayEmote (int index)
    {
        CmdPlayEmote(index);
    }

    public void StopEmote ()
    {
        CmdStopEmote();
    }
    #endregion

    #region Network
    #region Play
    [Command]
    public void CmdPlayEmote (int index)
    {
        RpcPlayEmote(index);
    }

    [ClientRpc]
    public void RpcPlayEmote (int index)
    {
        AnimationClip clip = attr.itemsContainer.GetEmoteByIndex(index).clip;
        StartCoroutine(WaitEmote(clip));
        
        attr.weapons.SetActive(false);
        attr.ik.enabled = false;
        if (isLocalPlayer) StartLocal();
        anim.Play(clip.name);
    }

    IEnumerator WaitEmote (AnimationClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        Stop();
    }
    #endregion

    #region Stop
    [Command]
    public void CmdStopEmote ()
    {
        RpcStopEmote();
    }

    [ClientRpc]
    public void RpcStopEmote ()
    {
        StopCoroutine(WaitEmote(null));
        Stop();
    }
    #endregion
    #endregion

    private void Stop ()
    {
        anim.Play("IdleEmote");
        attr.weapons.SetActive(true);
        attr.ik.enabled = true;
        if (isLocalPlayer) StopLocal();
    }

    private void StartLocal ()
    {
        attr.skinBody.SetActive(true);
        attr.emoteCamera.SetActive(true);
        attr.emoteCamera.transform.position = attr.mainCamera.transform.position;
        needMove = true;
    }

    private void StopLocal ()
    {
        needMove = false;
        attr.skinBody.SetActive(false);
        attr.emoteCamera.SetActive(false);
    }
}
