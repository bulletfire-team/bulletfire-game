using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayDoor : MonoBehaviour {

    public Collider door;

    private Collider player = null;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RedTeam" || other.tag == "BlueTeam")
        {
            player = other;
            Physics.IgnoreCollision(other, door, false);
        }
    }

    public void Reinit ()
    {
        if(player != null)
        {
            Physics.IgnoreCollision(player, door, true);
        }
    }
}
