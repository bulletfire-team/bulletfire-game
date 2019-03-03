using UnityEngine;
using UnityEngine.Networking;

public class PlayerFoot : MonoBehaviour
{
    private Vector3 jump = new Vector3(0, 150f, 0);
    public bool surterre = false;

    public Rigidbody rb;

    public MyNetworkAudioManager audioManager;

    private void Start()
    {
        if (!rb.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            enabled = false;
        }
    }

    public bool Jump ()
    {
        if (surterre)
        {
            //rb.AddForce(jump);
            rb.velocity = new Vector3(rb.velocity.x, 4, rb.velocity.z);
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        surterre = true;
        rb.GetComponent<PlayerMovement>().anime.SetBool("InTheAir", false);
        rb.GetComponent<PlayerMovement>().weapAnim.CmdSetBool("InTheAir", false);
        if (enabled)
        {
            rb.GetComponent<PlayerMovement>().PlaySound("Land", 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!surterre)
        {
            surterre = true;
            rb.GetComponent<PlayerMovement>().anime.SetBool("InTheAir", false);
            rb.GetComponent<PlayerMovement>().weapAnim.CmdSetBool("InTheAir", false);
            if (enabled)
            {
                rb.GetComponent<PlayerMovement>().PlaySound("Land", 0);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        surterre = false;
        rb.GetComponent<PlayerMovement>().anime.SetBool("InTheAir", true);
        rb.GetComponent<PlayerMovement>().weapAnim.CmdSetBool("InTheAir", true);
    }
}
