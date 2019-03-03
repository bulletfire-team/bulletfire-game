using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour {

    public Vector3 target;

    public Vector3 pointing = Vector3.zero;

    public GameObject indicator;

	void Update () {
        if (Camera.main == null) return;
        Vector3 dir = Camera.main.WorldToScreenPoint(target);

        pointing.z = Mathf.Atan2((transform.position.y - dir.y), (transform.position.x - dir.x)) * Mathf.Rad2Deg - 90;

        transform.rotation = Quaternion.Euler(pointing);
	}

    public void SetTarget (Vector3 target)
    {
        StopCoroutine(WaitRemoveIndicator());
        indicator.SetActive(true);
        this.target = target;
        StartCoroutine(WaitRemoveIndicator());
    }

    IEnumerator WaitRemoveIndicator ()
    {
        yield return new WaitForSeconds(1.5f);
        indicator.SetActive(false);
    }
}
