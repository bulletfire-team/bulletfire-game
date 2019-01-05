using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerEmote : NetworkBehaviour
{
    public PlayerAtributes attr;
    public Animator anim;

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
    public void CmdPlayEmote (int index)
    {
        RpcPlayEmote(index);
    }

    public void RpcPlayEmote (int index)
    {
        AnimationClip clip = attr.itemsContainer.emotes[index].clip;
        StartCoroutine(WaitEmote(clip));
        
        attr.weapons.SetActive(false);
        attr.ik.enabled = false;
        if (isLocalPlayer) StartLocal();
        anim.Play(clip.name);
    }

    IEnumerator WaitEmote (AnimationClip clip)
    {
        print("Start cor");
        yield return new WaitForSeconds(clip.length);
        print("Stop emote");
        Stop();
    }
    #endregion

    #region Stop
    public void CmdStopEmote ()
    {
        RpcStopEmote();
    }

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
    }

    private void StopLocal ()
    {
        attr.skinBody.SetActive(false);
        attr.emoteCamera.SetActive(false);
    }
}
