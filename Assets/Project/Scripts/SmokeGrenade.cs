using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{

    public GameObject smoke;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        smoke.SetActive(true);
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
