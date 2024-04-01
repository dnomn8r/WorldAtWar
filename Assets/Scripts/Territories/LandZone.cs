using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LandZone : Zone{

#if UNITY_EDITOR

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
#endif

    [SerializeField] private LandTerritory landTerritory;

    public LandTerritory LandTerritory{

        get { return landTerritory; }
#if UNITY_EDITOR

        set { landTerritory = value; }
#endif
    }

    [SerializeField] protected List<LandZone> hazardousAdjacencies = new List<LandZone>();
	public List<LandZone> HazardousAdjacencies {
		get { return hazardousAdjacencies; }
	}

	public int Value { get { return landTerritory.Value; } }

	public Country OriginalOwner { get; private set; }
	public Country CurrentOwner { get; private set; }

	private List<SpriteRenderer> landRenderers = new List<SpriteRenderer>();


	private void Awake() {

		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer renderer in renderers) {

			if (renderer.GetComponent<ZoneValueDisplay>() == null && renderer.GetComponent<ZoneNameDisplay>() == null) {
				landRenderers.Add(renderer);
			}
		}
	}

	public void SetOriginalOwner(Country country) {

		OriginalOwner = country;

		UpdateLandColors();
	}

	public void SetCurrentOwner(Country country) {

		CurrentOwner = country;

		UpdateLandColors();
	}

	private void UpdateLandColors() {

		foreach (SpriteRenderer renderer in landRenderers) {

			renderer.color = OriginalOwner.OwnershipColor;
		}
	}

}
