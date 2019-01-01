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

    IEnumerator Explosion ()
    {
        yield return new WaitForSeconds(sec);
        boom = true;
        Instantiate(particle, transform.position, transform.rotation);
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

    private void OnTriggerStay (Collider other)
    {
        if (!boom) return;
        
        if(other.CompareTag("Player"))
        {
            NetworkPlayer o = other.GetComponent<NetworkPlayer>();
            if (o!= null)
            {
                o.GetDammages(dammages, netIdPlayer, transform.position);
            }
        }
    }

}
