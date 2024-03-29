using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LandZone : Zone{

	[SerializeField] private LandTerritory landTerritory;

	public LandTerritory LandTerritory {

		get { return landTerritory; }
#if UNITY_EDITOR

		set { landTerritory = value; }
#endif
	}

	[SerializeField] protected List<LandZone> hazardousAdjacencies = new List<LandZone>();
	public List<LandZone> HazardousAdjacencies {
		get { return hazardousAdjacencies; }
	}

	public int Value { get { return landTerritory.Value ; } }


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
