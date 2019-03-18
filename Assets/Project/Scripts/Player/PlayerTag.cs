using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Experimental.Rendering.HDPipeline;

public class PlayerTag : NetworkBehaviour
{

    public PlayerAtributes attr;
    public GameObject tagContainer;

    private void Start()
    {
        if (isLocalPlayer)
        {
            GameObject.Find("TagMenu").GetComponent<TagUI>().SetPlayerTag(this);
        }
    }

    public void PrintTag (int index)
    {
        RaycastHit hit;
        if(Physics.Raycast(attr.mainCamera.transform.position, attr.mainCamera.transform.TransformDirection(Vector3.forward), out hit, 5))
        {
            if(hit.transform.tag == "Untagged")
            {
                CmdPrintTag(index, hit.point, hit.transform.rotation);
            }
        }
    }

    [Command]
    public void CmdPrintTag(int index, Vector3 pos, Quaternion rot)
    {
        RpcPrintTag(index, pos, rot);
    }

    [ClientRpc]
    public void RpcPrintTag (int index, Vector3 pos, Quaternion rot)
    {
        Tag tag = attr.itemsContainer.GetTagByIndex(index);
        GameObject g = Instantiate(tagContainer, pos, rot);
        g.GetComponent<DecalProjectorComponent>().m_Material = tag.tag;
    }
}
