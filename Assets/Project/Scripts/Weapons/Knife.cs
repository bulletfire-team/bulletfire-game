using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {

    public int damage = 100;
    public string ennemyTag;

    public string netIdPlayer;


    void OnCollisionEnter(Collision hit)
    {

        if(hit.collider.tag == "RedTeam" || hit.collider.tag == "BlueTeam")
        {
            hit.gameObject.GetComponent<Hitbox>().GetDammages(damage, netIdPlayer, transform.position, 3);
        }
    }
}
