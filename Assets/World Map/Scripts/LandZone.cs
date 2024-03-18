using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandZone : Zone{

	[SerializeField] protected List<LandZone> hazardousAdjacencies = new List<LandZone>();
	public List<LandZone> HazardousAdjacencies {
		get { return hazardousAdjacencies; }
	}

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

	protected override void ToggleAdjacencyHighlights(bool toggle) {

		base.ToggleAdjacencyHighlights(toggle);

		foreach (LandZone zone in hazardousAdjacencies) {

			if (zone != null) {

				zone.ToggleHighlight(toggle, true);
			} else {

				Debug.LogError("zone is null in " + gameObject.name + " this should not happen");
			}
		}
	}

}
