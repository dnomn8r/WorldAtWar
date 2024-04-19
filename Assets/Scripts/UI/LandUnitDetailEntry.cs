using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandUnitDetailEntry : UnitDetailEntry {


	public override void SetUnit(Unit unit, int count, MajorPower owner) {

		base.SetUnit(unit, count, owner);	

		LandUnit landUnit = unit as LandUnit;

		attackStrengthField.text = landUnit.Attack.ToString();
		defenceStrengthField.text = landUnit.Defence.ToString();	
		
	}

}


