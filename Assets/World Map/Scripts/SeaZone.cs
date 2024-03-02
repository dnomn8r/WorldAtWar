using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeaZone : Zone{

	public override Color BaseColor {
		get {
			return new Color(0.15f, 0.6f, 0.9f);
		}
	}
	public override FontStyles FontStyle {
		get {
			return FontStyles.Italic;
		}
	}


}
