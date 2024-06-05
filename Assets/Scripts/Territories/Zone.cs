using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public abstract class Zone : MonoBehaviour {

#if UNITY_EDITOR
	[MenuItem("Zones/Clear Adjacencies")]
	static void ClearAdjacencies() {

		Debug.Log("clear all zone adjacencies");

		Zone[] zones = FindObjectsByType<Zone>(FindObjectsSortMode.None);

		foreach (Zone zone in zones) {
			zone.Adjacencies.Clear();
		}
	}


	[MenuItem("Zones/Make Scriptables")]
	static void MakeScriptables() {

		LandZone[] landZones = FindObjectsByType<LandZone>(FindObjectsSortMode.None);

		foreach (LandZone zone in landZones) {

			LandTerritory territory = ScriptableObject.CreateInstance<LandTerritory>();

			territory.Value = zone.Value;

			string path = "Assets/Definitions/LandTerritories/" + zone.name + ".asset";

			AssetDatabase.CreateAsset(territory, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			zone.LandTerritory = territory;
		}

		SeaZone[] seaZones = FindObjectsByType<SeaZone>(FindObjectsSortMode.None);

		foreach (SeaZone zone in seaZones) {

			WaterTerritory territory = ScriptableObject.CreateInstance<WaterTerritory>();

			string path = "Assets/Definitions/WaterTerritories/" + zone.name + ".asset";

			AssetDatabase.CreateAsset(territory, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			zone.WaterTerritory = territory;
		}
	}


	public abstract Color BaseColor { get; }
	public abstract FontStyles FontStyle { get; }

	public void ToggleSelection(bool toggle) {

		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer ren in renderers) {
			if (ren.GetComponentInParent<ZoneUnitTypeDisplay>() == null) {
				ren.color = toggle ? Color.green : BaseColor;
			}
		}

		ToggleAdjacencyHighlights(toggle);
	}

	protected void ToggleHighlight(bool toggle, bool isHazard = false) {

		SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer ren in renderers) {
			if (ren.GetComponentInParent<ZoneUnitTypeDisplay>() == null) {
				ren.color = toggle ? (isHazard ? Color.red : Color.yellow) : BaseColor;
			}
		}
	}

	protected virtual void ToggleAdjacencyHighlights(bool toggle) {

		foreach (Zone zone in adjacencies) {

			if (zone != null) {

				zone.ToggleHighlight(toggle);
			} else {

				Debug.LogError("zone is null in " + gameObject.name + " this should not happen");
			}
		}
	}
#endif



	[SerializeField] protected List<Zone> adjacencies = new List<Zone>();

	public List<Zone> Adjacencies {
		get { return adjacencies; }	
	}

    [SerializeField] private List<SpriteRenderer> zoneRenderers = new List<SpriteRenderer>();

    public List<SpriteRenderer> ZoneRenderers {

        get { return zoneRenderers; }
#if UNITY_EDITOR

        set { zoneRenderers = value; }
#endif
    }


    [SerializeField] private ZoneNameDisplay zoneNameDisplay;
    public ZoneNameDisplay ZoneNameDisplay {

        get { return zoneNameDisplay; }
#if UNITY_EDITOR

        set { zoneNameDisplay = value; }
#endif
    }

	public struct UnitOwnershipEntry {

		public Country owner;
		public Unit unit;
		public int count;

		public UnitOwnershipEntry(Country owner, Unit unit, int count) {

			this.owner = owner;
			this.unit = unit;
			this.count = count;
		}
	}

	public abstract Territory Territory {
		get;
	}

    [SerializeField] protected ZoneInfoDisplay zoneInfoDisplay;


    private Dictionary<string, UnitOwnershipEntry> units = new Dictionary<string, UnitOwnershipEntry>();

	public List<UnitOwnershipEntry> GetUnits() {
		return new List<UnitOwnershipEntry>(units.Values);
	}

	public void AddUnits(Country owner, Unit unit, int count) {

		string key = owner.name + unit.name;

		if(!units.ContainsKey(key)) {

			units.Add(key, new UnitOwnershipEntry(owner, unit, count));
		} else {

			UnitOwnershipEntry entry = units[key];
			entry.count += count;
			units[key] = entry;
		}

		if(zoneInfoDisplay != null) {
			zoneInfoDisplay.Refresh();
		}
	}

    public void RemoveUnits(Country owner, Unit unit, int count) {

        string key = owner.name + unit.name;

        if (!units.ContainsKey(key)) {

			Debug.LogError("trying to remove units that aren't there!");

        } else {

            UnitOwnershipEntry entry = units[key];
            entry.count -= count;

			if (entry.count < 0) {

				Debug.LogError("trying to remove more units that we have!");

			} else if (entry.count == 0) {

				units.Remove(key);

			} else {

				units[key] = entry;
			}
        }

        if (zoneInfoDisplay != null) {
            zoneInfoDisplay.Refresh();
        }
    }


    protected virtual void Awake() {

		if(zoneInfoDisplay != null) {
			zoneInfoDisplay.SetZone(this);
		}

		SetHoverState(false);
	}

	public virtual void SetHoverState(bool hover) {

        //Debug.Log("hovered zone: " + name, gameObject);

		SetNameColor(hover ? "yellow" : null);

		foreach (Zone zone in adjacencies) {
			zone.SetNameColor(hover ? "green" : null);
		}

    }

	public void SetNameColor(string color) {

		if(ZoneNameDisplay != null) {
			zoneNameDisplay.SetNameColor(color);
		}
	}

	public void SetSelectedState(bool selected) {

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer ren in zoneRenderers) {
			if (ren.GetComponentInParent<ZoneUnitTypeDisplay>() == null) {
				ren.color = selected ? Color.green : BaseColor;
			}
		}
    }



}
