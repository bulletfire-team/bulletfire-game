using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour {

    public float destroyTime = 2.0f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroyTime);
	}

}
