using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandZone : Zone{

	[SerializeField]private int zoneValue = 0;

	public int Value { get { return zoneValue; } }


	public override Color BaseColor {
		get {
			return Color.white;
		}
	}

	public override FontStyles FontStyle {
		get {
			return FontStyles.Bold;
		}
	}

}
