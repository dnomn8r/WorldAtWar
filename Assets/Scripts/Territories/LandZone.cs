using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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

	[MenuItem("Zones/Hookup Components")]
	static void HookupComponents() {

		LandZone[] zones = FindObjectsByType<LandZone>(FindObjectsSortMode.None);

		foreach (LandZone zone in zones) {

			ZoneNameDisplay zoneNameDisplay = zone.GetComponentInChildren<ZoneNameDisplay>();
			if(zoneNameDisplay != null) {
				zone.ZoneNameDisplay = zoneNameDisplay;
			} else {
				Debug.LogError("missing zone name display on " + zone.name, zone);
			}

			ZoneValueDisplay zoneValueDisplay = zone.GetComponentInChildren<ZoneValueDisplay>();

			if (zoneValueDisplay != null) {
				zone.ZoneValueDisplay = zoneValueDisplay;
			} else if(zone.Value > 0){
				Debug.LogError("missing zone value display on " + zone.name, zone);
			}

			SpriteRenderer[] renderers = zone.GetComponentsInChildren<SpriteRenderer>();


			List<SpriteRenderer> landRenderers = new List<SpriteRenderer>();

			foreach (SpriteRenderer renderer in renderers) {

				if (renderer.GetComponent<ZoneValueDisplay>() == null && renderer.GetComponent<ZoneNameDisplay>() == null && renderer != zone.OriginalOwnerFlag) {
					landRenderers.Add(renderer);
				}
			}

			zone.LandRenderers = landRenderers;

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

	private Country originalOwner;
	public Country OriginalOwner {
		get {
			return originalOwner;
		}

		private set {
			originalOwner = value;
			currentOwner = value;

			OriginalOwnerFlag.sprite = OriginalOwner.Flag;

			UpdateLandColors();
			UpdateOwnerFlag();
		}
	}

	private Country currentOwner;
	public Country CurrentOwner { 
		get { return currentOwner; }

		private set {
			currentOwner = value;

			UpdateLandColors();
			UpdateOwnerFlag();
		}
	}

	[SerializeField] private List<SpriteRenderer> landRenderers = new List<SpriteRenderer>();

	public List<SpriteRenderer> LandRenderers {

		get { return landRenderers; }
#if UNITY_EDITOR

		set { landRenderers = value; }
#endif
	}


	[SerializeField] private ZoneNameDisplay zoneNameDisplay;
	public ZoneNameDisplay ZoneNameDisplay {

		get { return zoneNameDisplay; }
#if UNITY_EDITOR

		set { zoneNameDisplay = value; }
#endif
	}

	[SerializeField] private ZoneValueDisplay zoneValueDisplay;
	public ZoneValueDisplay ZoneValueDisplay {

		get { return zoneValueDisplay; }
#if UNITY_EDITOR

		set { zoneValueDisplay = value; }
#endif
	}

	[SerializeField] private SpriteRenderer originalOwnerFlag;
    public SpriteRenderer OriginalOwnerFlag {

        get { return originalOwnerFlag; }
#if UNITY_EDITOR

        set { originalOwnerFlag = value; }
#endif
    }

	public bool isCapital = false;

    private void Awake() {

		if (Value > 0) {
			ZoneValueDisplay.SetValue(Value);
		}
	}

	public void SetOriginalOwner(Country country) {

		OriginalOwner = country;
	}

	public void SetCurrentOwner(Country country) {

		CurrentOwner = country;		
	}

	private void UpdateLandColors() {

		foreach (SpriteRenderer renderer in landRenderers) {

			renderer.color = CurrentOwner.OwnershipColor;
		}
	}

	private void UpdateOwnerFlag() {

		OriginalOwnerFlag.gameObject.SetActive(isCapital || OriginalOwner != CurrentOwner);
	}

}
