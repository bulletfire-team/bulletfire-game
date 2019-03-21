using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour {

    public float sec = 2f;
    public GameObject particle;

    public int dammages = 150;
    public string ennemyTag;
    private bool boom = false;

    public string netIdPlayer;
    


    private void OnCollisionEnter (Collision collision)
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(sec);
        boom = true;
        Instantiate(particle, transform.position, transform.rotation);
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, 10, Vector3.forward);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.tag == "Player")
            {
                NetworkPlayer o = hit.transform.GetComponent<NetworkPlayer>();
                if (o != null)
                {
                    o.GetDammages(dammages, netIdPlayer, transform.position, 2);
                }
            }
        }
        Destroy(gameObject);
    }

}
