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

	[MenuItem("Zones/Add Moves")]
	static void AddMoves() {

		Zone[] zones = FindObjectsByType<Zone>(FindObjectsSortMode.None);

		foreach (Zone zone in zones) {

			zone.movementArrows.Clear();

			foreach (Zone adjacency in zone.adjacencies) {

				GameObject newArrow = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/World Map/Prefabs/MovementArrow.prefab")) as GameObject;

				newArrow.name = "Move to " + adjacency.name;
				newArrow.transform.SetParent(zone.transform, false);

				newArrow.transform.localPosition = Vector3.zero;


				zone.movementArrows.Add(newArrow);
			}

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

    public abstract Color BaseColor { get; }
    public abstract FontStyles FontStyle { get; }


    [SerializeField] protected List<Zone> adjacencies = new List<Zone>();

	public List<Zone> Adjacencies {
		get { return adjacencies; }	
	}

	[SerializeField] protected List<GameObject> movementArrows = new List<GameObject>();
	public List<GameObject> MovementArrows { get { return movementArrows; } }


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


	public class UnitInstance {

		public Country owner;
		public Unit unit;

		public int moveRemaining;
		public int hitsRemaining;

		public UnitInstance(Country owner, Unit unit) {
			this.owner = owner;
			this.unit = unit;

			moveRemaining = unit.Movement;
			hitsRemaining = unit.Hitpoints;
		}
	}
	public class TransportInstance : UnitInstance {
        public TransportInstance(Country owner, Unit unit) : base(owner, unit) {

        }

        private List<UnitInstance> carriedLandUnits = new List<UnitInstance>();
        public List<UnitInstance> CarriedLandUnits { get { return carriedLandUnits; } }

        public void AddLandUnit(UnitInstance landUnit) {
            carriedLandUnits.Add(landUnit);
        }
        public void RemoveFighter(UnitInstance landUnit) {
            carriedLandUnits.Remove(landUnit);
        }
        public bool HasRoomForLandUnit(Unit unit) {

			if(unit is LandUnit landUnit) {

				return RemainingCapacity >= landUnit.TransportLoad;
			}

			return false;
        }
        public int RemainingCapacity {
            get {
                int remainingCapacity = (unit as Transport).Capacity;

                for (int i = 0; i < carriedLandUnits.Count; i++) {
                    remainingCapacity -= (carriedLandUnits[i].unit as LandUnit).TransportLoad;
                }
                if (remainingCapacity < 0) {
                    Debug.LogError("should never have less than 0 capacity! something went wrong!");
                    remainingCapacity = 0;
                }
                return remainingCapacity;
            }
        }
    }

	public class CarrierInstance : UnitInstance {
		
		public CarrierInstance(Country owner, Unit unit) : base(owner, unit){
			
		}

        private List<UnitInstance> carriedFighters = new List<UnitInstance>();
        public List<UnitInstance> CarriedFighters { get { return carriedFighters; } }

        public void AddFighter(UnitInstance fighter) {
            carriedFighters.Add(fighter);
        }
        public void RemoveFighter(UnitInstance fighter) {
            carriedFighters.Remove(fighter);
        }
        public bool HasRoomForFighter(Fighter fighter) {
            return RemainingCapacity >= fighter.CarrierLoad;
        }
        public int RemainingCapacity {
            get {
                int remainingCapacity = (unit as Carrier).MaxCapacity;

                for (int i = 0; i < carriedFighters.Count; i++) {
                    remainingCapacity -= (carriedFighters[i].unit as Fighter).CarrierLoad;
                }
                if (remainingCapacity < 0) {
                    Debug.LogError("should never have less than 0 capacity! something went wrong!");
                    remainingCapacity = 0;
                }
                return remainingCapacity;
            }
        }
    }

	public abstract Territory Territory {
		get;
	}

    [SerializeField] private ZoneInfoDisplay zoneInfoDisplay;
    public ZoneInfoDisplay ZoneInfoDisplay {

        get { return zoneInfoDisplay; }
#if UNITY_EDITOR

        set { zoneInfoDisplay = value; }
#endif
    }

	public struct UnitOwnershipEntry {

		public UnitInstance unit;
		public int count;

		public UnitOwnershipEntry(UnitInstance unit, int count) {

			this.unit = unit;
			this.count = count;
		}
	}


	private List<UnitInstance> units = new List<UnitInstance>();

	public List<UnitInstance> GetUnits() {

		return units;
	}

	public void AddUnits(Country owner, Unit unit, int count) {

		for (int i = 0; i < count; i++) {

			UnitInstance newUnit;

			if(unit is Transport) {

				newUnit = new TransportInstance(owner, unit);
			}else if (unit is Carrier) {

				newUnit = new CarrierInstance(owner, unit);
			} else { 
				newUnit = new UnitInstance(owner, unit);
			}
			units.Add(newUnit);
		}

		if(zoneInfoDisplay != null) {
			zoneInfoDisplay.Refresh();
		}
	}

    public void RemoveUnit(UnitInstance unit) {

		units.Remove(unit);

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

        //SetNameColor(hover ? "yellow" : null);

        //foreach (Zone zone in adjacencies) {
        //	zone.SetNameColor(hover ? "green" : null);
        //}
        foreach (SpriteRenderer ren in zoneRenderers) {
            if (ren.GetComponentInParent<ZoneUnitTypeDisplay>() == null) {
                //ren.color = hover ? new Color(1, 0.65f, 0) : BaseColor; // orange
                ren.color = hover ? Color.cyan : BaseColor; // orange
            }
        }
    }

	//public void SetNameColor(string color) {

	//	if(ZoneNameDisplay != null) {
	//		zoneNameDisplay.SetNameColor(color);
	//	}
	//}

	public void SetSelectedState(bool selected) {

		foreach (SpriteRenderer ren in zoneRenderers) {
			if (ren.GetComponentInParent<ZoneUnitTypeDisplay>() == null) {
				ren.color = selected ? Color.green : BaseColor;
			}
		}
    }



}
