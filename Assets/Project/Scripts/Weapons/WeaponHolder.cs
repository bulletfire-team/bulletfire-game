using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour {

	public Weapon weapon;

    public int curAmmo;
    public int curLoader;

	public Transform shotPlace;

    public Transform leftIK;
    public Transform rightIK;

    public Animation anim;

    private void Start()
    {
        curAmmo = weapon.maxAmmo;
        curLoader = weapon.maxLoader;
        anim = GetComponent<Animation>();
    }

    public void NewRound (TMPro.TMP_Text ammo)
    {
        curAmmo = weapon.maxAmmo;
        curLoader = weapon.maxLoader;
        ammo.text = curAmmo + " - " + curLoader;
    }

}
