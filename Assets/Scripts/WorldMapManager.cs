using System.Collections.Generic;
using UnityEngine;

public class WorldMapManager : MonoBehaviour{

	[SerializeField] private List<MajorPower> majorPowers;
	public List<MajorPower> MajorPowers { get { return majorPowers; } }

	[SerializeField] private List<MinorPower> minorPowers;

	[SerializeField] private Sprite neutralFlag;

	private Dictionary<Territory, Zone> zoneMapping = new Dictionary<Territory, Zone>();
	private List<Zone> allZones;

	private List<Canal> allCanals;
	public Canal GetCanalLink(SeaZone seaZone, SeaZone otherSeaZone) {

		for (int i = 0; i < allCanals.Count; i++) {

			if (allCanals[i].FirstSeaZone == seaZone && allCanals[i].SecondSeaZone == otherSeaZone ||
                (allCanals[i].FirstSeaZone == otherSeaZone && allCanals[i].SecondSeaZone == seaZone)) {
			
				return allCanals[i];
			} 
		}

		return null;
	}


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
		allCanals = new List<Canal>(GetComponentsInChildren<Canal>());

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

		LandZone currentLandZone = currentZone as LandZone;
        SeaZone currentSeaZone = currentZone as SeaZone;

        MajorPower unitOwner = unitInstance.owner as MajorPower;
		if (unitOwner == null) {
			Debug.LogError("should never be trying to move a non major power unit!");
			return;
		}

		// if it's a land unit, needs to be on land, and be friendly (unless in combat)
		if(unitInstance.unit.MovementType == Unit.MoveType.LAND) {

			if(currentLandZone == null) {
				return;
			}

			// **** eventually have checks work with diplomacy
			if (!unitOwner.IsLandMovementAlly(currentLandZone.CurrentOwner)) {
				return;
			}
		
		}else if(unitInstance.unit.MovementType == Unit.MoveType.SEA) {

			// sea units have slightly different rules for sharing space

			if(currentSeaZone == null) {
				return;
			}

			List<Zone.UnitInstance> unitsInSeaZone = currentSeaZone.GetUnits();

			foreach(Zone.UnitInstance seaUnit in unitsInSeaZone) {
			
				if (!unitOwner.IsSeaMovementAlly(seaUnit.owner)) {
					return;
				}	
			}

        }

        if (!zonesInRange.Contains(currentZone)) {

			// air units must end up in a friendly land territory not taken this turn
			if(unitInstance.unit.MovementType != Unit.MoveType.AIR ||
				(currentLandZone != null
				&& unitOwner.IsLandMovementAlly(currentLandZone.CurrentOwner)
				&& !GameManager.Instance.IsZoneTakenThisTurn(currentLandZone))) {

                zonesInRange.Add(currentZone);
            }
		}

		if (range > 0) {

			if (currentLandZone != null && 
				(unitInstance.unit.MovementType == Unit.MoveType.LAND || unitInstance.unit.MovementType == Unit.MoveType.AIR)) {

				foreach (Zone hazardousZone in currentLandZone.HazardousAdjacencies) {

					// move goes to 0 if we're a land unit that can't fly since hazardous terrain stops movement
                    GetZonesWithinRange(hazardousZone, unitInstance, 
						unitInstance.unit.MovementType == Unit.MoveType.LAND ? 0 : range - 1, 
						ref zonesInRange);
                }
			}

			foreach (Zone potentialZone in currentZone.Adjacencies) {

				if (potentialZone is LandZone && (unitInstance.unit.MovementType == Unit.MoveType.LAND || 
					unitInstance.unit.MovementType == Unit.MoveType.AIR)) {
				
					GetZonesWithinRange(potentialZone, unitInstance, range - 1, ref zonesInRange);
				
				}else if (potentialZone is SeaZone && (unitInstance.unit.MovementType == Unit.MoveType.SEA || 
					unitInstance.unit.MovementType == Unit.MoveType.AIR)) {

					bool isValidLink = true; // check if it's a canal, and if it is, can we use it

					// if our link is canal, check if both sides allow us to share land with them
					// if so, we can use it.(even if it's been conquered this turn)
					if(unitInstance.unit.MovementType == Unit.MoveType.SEA) {

						Canal canal = GetCanalLink(potentialZone as SeaZone, currentSeaZone);

						if(canal != null) {

							foreach(LandZone land in canal.Owners) {

								if(!unitOwner.IsLandMovementAlly(land.CurrentOwner)) {
									isValidLink = false;
									break;
								}
							}
						}
					}

					if (isValidLink) {
						GetZonesWithinRange(potentialZone, unitInstance, range - 1, ref zonesInRange);
					}
                }
			}

			
		}

	}

}

