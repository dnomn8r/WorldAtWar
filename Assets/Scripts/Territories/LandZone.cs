using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LandZone : Zone{

#if UNITY_EDITOR

    [MenuItem("Zones/Add Flags")]
    static void AddFlagsToZones() {

        LandZone[] zones = FindObjectsByType<LandZone>(FindObjectsSortMode.None);

        foreach (LandZone zone in zones) {

			GameObject newFlag = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/CountryFlags/CountryFlagIndicator.prefab")) as GameObject;

			newFlag.name = "CountryFlagIndicator";
            newFlag.transform.SetParent(zone.transform, false);

			zone.originalOwnerFlag = newFlag.GetComponent<SpriteRenderer>();
        }
    }

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

	[SerializeField] private SpriteRenderer originalOwnerFlag;
    public SpriteRenderer OriginalOwnerFlag {

        get { return originalOwnerFlag; }
#if UNITY_EDITOR

        set { originalOwnerFlag = value; }
#endif
    }

    private void Awake() {

		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer renderer in renderers) {

			if (renderer.GetComponent<ZoneValueDisplay>() == null && renderer.GetComponent<ZoneNameDisplay>() == null && !renderer.name.StartsWith("CountryFlag")) {
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
