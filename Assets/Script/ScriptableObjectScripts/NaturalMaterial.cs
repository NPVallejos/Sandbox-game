using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Material", menuName = "NaturalMaterial")]
public class NaturalMaterial : Item {
	public NaturalMaterial()
	: base() {}
	public NaturalMaterial(string id, short total, Sprite[] sp_arr) : base(id, total, sp_arr) {}
	public override void toString() {
		base.toString();
	}
}
