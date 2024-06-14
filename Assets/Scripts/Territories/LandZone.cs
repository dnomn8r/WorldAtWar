using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class LandZone : Zone {

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

		LandZone[] landZones = FindObjectsByType<LandZone>(FindObjectsSortMode.None);

		foreach (LandZone zone in landZones) {

			ZoneValueDisplay zoneValueDisplay = zone.GetComponentInChildren<ZoneValueDisplay>();

			if (zoneValueDisplay != null) {
				zone.ZoneValueDisplay = zoneValueDisplay;
			} else if (zone.Value > 0) {
				Debug.LogError("missing zone value display on " + zone.name, zone);
			}

            ZoneInfoDisplay zoneInfoDisplay = zone.GetComponentInChildren<ZoneInfoDisplay>();

            if (zoneInfoDisplay != null) {
                zone.ZoneInfoDisplay = zoneInfoDisplay;
            } else if (zone.Value > 0) {
                Debug.LogError("missing zone info display on " + zone.name, zone);
            }

        }

		Zone[] zones = FindObjectsByType<Zone>(FindObjectsSortMode.None);

		foreach (Zone zone in zones) {

			ZoneNameDisplay zoneNameDisplay = zone.GetComponentInChildren<ZoneNameDisplay>();
			if (zoneNameDisplay != null) {
				zone.ZoneNameDisplay = zoneNameDisplay;
			}

			SpriteRenderer[] renderers = zone.GetComponentsInChildren<SpriteRenderer>();

			List<SpriteRenderer> zoneRenderers = new List<SpriteRenderer>();

			foreach (SpriteRenderer renderer in renderers) {

				if (renderer.GetComponent<ZoneValueDisplay>() == null && renderer.GetComponent<ZoneNameDisplay>() == null && !renderer.name.StartsWith("CountryFlag")) {

					if (renderer.GetComponent<PolygonCollider2D>() == null) {
						renderer.gameObject.AddComponent<PolygonCollider2D>();
					}

					zoneRenderers.Add(renderer);
				}
			}

			zone.ZoneRenderers = zoneRenderers;

		}

	}


	public override Color BaseColor {
		get {
			return CurrentOwner != null ? CurrentOwner.OwnershipColor : Color.white;
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

	public LandTerritory LandTerritory {

		get { return landTerritory; }
#if UNITY_EDITOR

		set { landTerritory = value; }
#endif
	}

    public override Territory Territory {
		get {
			return landTerritory;
		}
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

	public Factory factory;

	protected override void Awake() {

		base.Awake();

		if (Value > 0) {
			ZoneValueDisplay.SetValue(Value);
		}

		SetFactory(null);
	}

	public void SetOriginalOwner(Country country) {

		OriginalOwner = country;
	}

	public void SetCurrentOwner(Country country) {

		CurrentOwner = country;

        if (ZoneInfoDisplay != null) {

			ZoneInfoDisplay.Refresh();
        }
    }

	public void SetFactory(Factory factory) {

		this.factory = factory;

		if (factory != null) {

			ZoneValueDisplay.GetComponent<SpriteRenderer>().sprite = factory.FactoryIcon;

		} else {

			if (ZoneValueDisplay != null) {

				Sprite[] circles = Resources.LoadAll<Sprite>("CircleOutline");

				ZoneValueDisplay.GetComponent<SpriteRenderer>().sprite = circles[0];
			}
		}

		if(ZoneInfoDisplay != null) {

			ZoneInfoDisplay.Refresh();
		}
	}

	private void UpdateLandColors() {

		foreach (SpriteRenderer renderer in ZoneRenderers) {

			renderer.color = CurrentOwner.OwnershipColor;
		}
	}

	private void UpdateOwnerFlag() {

		OriginalOwnerFlag.gameObject.SetActive(isCapital || OriginalOwner != CurrentOwner);
	}

	public override void SetHoverState(bool hover) {

		base.SetHoverState(hover);

		foreach(Zone zone in hazardousAdjacencies) {

			zone.SetNameColor(hover ? "red" : null);
		}

	}
}
