using System.Collections.Generic;
using UnityEngine;

public class WorldMapManager : MonoBehaviour{

	[SerializeField] private List<MajorPower> majorPowers;
	public List<MajorPower> MajorPowers { get { return majorPowers; } }

	[SerializeField] private List<MinorPower> minorPowers;

	[SerializeField] private Sprite neutralFlag;

	//[Space]

	//[SerializeField] private InitialGameState initialGameState;


	private Dictionary<Territory, Zone> zoneMapping = new Dictionary<Territory, Zone>();
	private List<Zone> allZones;



    private static WorldMapManager instance;
    public static WorldMapManager Instance {
        get { return instance; }
    }

	public Zone GetZone(Territory territory) {

		Zone zone = null;
		if (zoneMapping.TryGetValue(territory, out zone)) {
			return zone;
		}
		else{
			Debug.LogError("no zone for territory: " + territory.name + " should never happen!");
		}

		return null;
	}

    private void Awake() {
        instance = this;
    }

    public void Initialize() {

		allZones = new List<Zone>(GetComponentsInChildren<Zone>());

		InitializeMovementArrows(allZones);

        MapLandZones();

		InitializeOriginalTerritories();

	}

	private void InitializeMovementArrows(List<Zone> zones) {

		foreach (Zone zone in zones) {

			MovementArrow[] moveArrows = zone.GetComponentsInChildren<MovementArrow>();
			for(int i = 0; i < moveArrows.Length; i++) {
				moveArrows[i].ToggleVisibility(false);
			}
		}

	}

	private void MapLandZones() {

		zoneMapping.Clear();		

		foreach (Zone zone in allZones) {

			zoneMapping.Add(zone.Territory, zone);
		}
	}

	private void InitializeOriginalTerritories() {

		List<LandZone> usedLandZones = new List<LandZone>();

		foreach (MajorPower majorPower in majorPowers) {

			foreach (LandTerritory currentTerritory in majorPower.LandTerritories) {

				LandZone landZone = zoneMapping[currentTerritory] as LandZone;

                landZone.SetOriginalOwner(majorPower);

				usedLandZones.Add(landZone);
			}
		}

		foreach (MinorPower minorPower in minorPowers) {

			foreach (LandTerritory currentTerritory in minorPower.LandTerritories) {

                LandZone landZone = zoneMapping[currentTerritory] as LandZone;

                landZone.SetOriginalOwner(minorPower);

				usedLandZones.Add(landZone);					
			}
		}

		List<LandZone> allLandZones = new List<LandZone>(GetComponentsInChildren<LandZone>());

		foreach (LandZone zone in allLandZones) {

			if (!usedLandZones.Contains(zone)) {

				Country newCountry = ScriptableObject.CreateInstance<Country>();
				newCountry.name = zone.name;
				newCountry.Flag = neutralFlag;

				zone.SetOriginalOwner(newCountry);
			}
		}

	}


    public int GetIncome(Country country) {

		int totalIncome = 0;

		for(int i = 0;i<allZones.Count;i++) {

			LandZone landZone = allZones[i] as LandZone;

			if (landZone != null) {

				if (landZone.CurrentOwner == country) {

					totalIncome += landZone.Value;
				}
			}
		}

		return totalIncome;
	}

	public void GetZonesWithinRange(Zone currentZone, Zone.UnitInstance unitInstance, 
									int range, ref List<Zone> zonesInRange) {

		LandZone landZone = currentZone as LandZone;
        SeaZone seaZone = currentZone as SeaZone;

        MajorPower unitOwner = unitInstance.owner as MajorPower;
		if (unitOwner == null) {
			Debug.LogError("should never be trying to move a non major power unit!");
			return;
		}

		// if it's a land unit, needs to be on land, and be friendly (unless in combat)
		if(unitInstance.unit.MovementType == Unit.MoveType.LAND) {

			if(landZone == null) {
				return;
			}

		
			// **** eventually have checks work with diplomacy

			if (!unitOwner.IsLandMovementAlly(landZone.CurrentOwner as MajorPower)) {
				return;
			}
			
        
		}else if(unitInstance.unit.MovementType == Unit.MoveType.SEA) {

			// sea units have slightly different rules for sharing space

			if(seaZone == null) {
				return;
			}

			List<Zone.UnitInstance> unitsInSeaZone = seaZone.GetUnits();

			foreach(Zone.UnitInstance seaUnit in unitsInSeaZone) {
				// we always allow sharing with minor powers or neutrals
				if(seaUnit.owner is MajorPower seaPower) {

					if (!unitOwner.IsSeaMovementAlly(seaPower)) {
						return;
					}
				} 
			}

        }

        if (!zonesInRange.Contains(currentZone)) {
			// air units must end up in a friendly land territory
			if (unitInstance.unit.MovementType != Unit.MoveType.AIR ||
				(landZone != null && unitOwner.IsLandMovementAlly(landZone.CurrentOwner as MajorPower))) {

				zonesInRange.Add(currentZone);
			}
		}

		if (range > 0) {

			if (landZone != null && 
				(unitInstance.unit.MovementType == Unit.MoveType.LAND || unitInstance.unit.MovementType == Unit.MoveType.AIR)) {

				foreach (Zone hazardousZone in landZone.HazardousAdjacencies) {

					// move goes to 0 if we're a land unit that can't fly since hazardous terrain stops movement
                    GetZonesWithinRange(hazardousZone, unitInstance, 
						unitInstance.unit.MovementType == Unit.MoveType.LAND ? range - 1 : 0, 
						ref zonesInRange);
                }
			}

			foreach (Zone zone in currentZone.Adjacencies) {

				if ((unitInstance.unit.MovementType == Unit.MoveType.LAND || 
					unitInstance.unit.MovementType == Unit.MoveType.AIR) && zone is LandZone) {
				
					GetZonesWithinRange(zone, unitInstance, range - 1, 
						ref zonesInRange);
				
				}else if ((unitInstance.unit.MovementType == Unit.MoveType.SEA || 
					unitInstance.unit.MovementType == Unit.MoveType.AIR) && zone is SeaZone) {

                    GetZonesWithinRange(zone, unitInstance, range - 1, 
						ref zonesInRange);
                }
			}
		}

	}

}

