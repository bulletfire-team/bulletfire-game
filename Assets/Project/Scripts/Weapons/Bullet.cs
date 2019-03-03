using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public string ennemyTag = "";
	public int dammages = 0;
    public string netIdPlayer;

	void Start () {
		Destroy(gameObject, 2.0f);
	}
	
	void OnCollisionEnter(Collision other){
		if(other.gameObject.tag == ennemyTag){
            other.gameObject.GetComponent<Hitbox>().GetDammages(dammages, netIdPlayer, transform.position,1) ;
        }
        else
        {

        }
		Destroy(gameObject);
	}
}
