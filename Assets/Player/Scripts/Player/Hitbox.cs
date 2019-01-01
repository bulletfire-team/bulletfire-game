using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour {

	private float[] multipliers = {2f,1f,0.5f};
	
	public enum bodypart {head, torso, other};
	
	public NetworkPlayer player;
	public bodypart part;

	public int GetDammages (int dam, string killerNetId, Vector3 pos) {
		float newDam = dam;
		switch(part){
			case bodypart.head :
				newDam *= multipliers[0];
				break;
			case bodypart.torso :
				newDam *= multipliers[1];
				break;
			case bodypart.other :
				newDam *= multipliers[2];
				break;
			default :
				newDam *= multipliers[1];
				break;
		}

		return player.GetDammages((int)newDam, killerNetId, pos);
	}
}
