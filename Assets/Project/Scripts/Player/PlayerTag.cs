using UnityEngine;
using UnityEngine.Networking;

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
                CmdPrintTag(index, hit.point, hit.normal);
            }
        }
    }

    [Command]
    public void CmdPrintTag(int index, Vector3 pos, Vector3 rot)
    {
        RpcPrintTag(index, pos, rot);
    }

    [ClientRpc]
    public void RpcPrintTag (int index, Vector3 pos, Vector3 rot)
    {
        Tag tag = attr.itemsContainer.GetTagByIndex(index);
        GameObject g = Instantiate(tag.tag, pos, Quaternion.identity);
        g.transform.right = rot;
    }
}
