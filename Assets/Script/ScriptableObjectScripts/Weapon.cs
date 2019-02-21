using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : Tool {
	public short physicalDmg;
	public short magicalDmg;
	public string effect;

	public Weapon() {//: base() {
		physicalDmg = 0;
		magicalDmg = 0;
		effect = "none";
	}
	public Weapon(string id, short total, Sprite[] sp_arr, short function, short tier, short physicalDmg, short magicalDmg, string effect)
	: base(id, total, sp_arr, function, tier) {
		this.physicalDmg = physicalDmg;
		this.magicalDmg = magicalDmg;
		this.effect = effect;
	}

	// This was made to test that this Inheritance works
	public override void attack() {
		Debug.Log("Attacked");
	}

	public override void toString() {
		base.toString();
		Debug.Log("physicalDmg=" + physicalDmg + "magicalDmg=" + magicalDmg + "effect=" + effect);
	}
}
