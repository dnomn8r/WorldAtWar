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

	public void GetZonesWithinRangeNonCombat(Zone currentZone, Zone.UnitInstance unitInstance, 
									int range, ref List<Zone> zonesInRange) {

		LandZone currentLandZone = currentZone as LandZone;
        SeaZone currentSeaZone = currentZone as SeaZone;

        MajorPower unitOwner = unitInstance.owner as MajorPower;
		if (unitOwner == null) {
			Debug.LogError("should never be trying to move a non major power unit!");
			return;
		}

		// *** link rules for non combat movement
		// the code could be more compact, with some conditionals combined, but keeping them
		// explicitly separate is a lot easier to read
		if (range > 0) {

			if (unitInstance.unit is LandUnit landUnit) {

				// check hazardous adjacencies for land units
				if (currentLandZone != null) {
					foreach (Zone hazardousZone in currentLandZone.HazardousAdjacencies) {
						if (unitOwner.IsLandMovementAlly(currentLandZone.CurrentOwner)) {
							// move goes to 0 if we're a land unit since hazardous terrain stops movement
							GetZonesWithinRangeNonCombat(hazardousZone, unitInstance, 0, ref zonesInRange);
						}
					}
				}

				foreach (Zone potentialZone in currentZone.Adjacencies) {

					if (potentialZone is LandZone potentialLandZone) {
						// check if the territory is non combat movement friendly
						if (unitOwner.IsLandMovementAlly(potentialLandZone.CurrentOwner)) {
							// if we start in a sea zone, we lose all the rest of our movement if we land
							GetZonesWithinRangeNonCombat(potentialLandZone, unitInstance,
								currentLandZone != null ? range - 1 : 0, ref zonesInRange);
						}

					} else if (potentialZone is SeaZone seaZone) {

						// check if there are transports in the zone and we start on land
						if(currentLandZone != null) {

							bool transportAvailable = false;
                            foreach (Zone.UnitInstance otherUnit in seaZone.GetUnits()) {

                                if (otherUnit is Zone.TransportInstance transport &&
                                    unitOwner == transport.owner) { // can only carry units of same country

                                    if (transport.RemainingCapacity >= landUnit.TransportLoad) {
                                        transportAvailable = true;
                                    }
                                    break;
                                }
                            }
							if(transportAvailable) { // movement ends if we load on a transport
                                GetZonesWithinRangeNonCombat(potentialZone, unitInstance, 0, ref zonesInRange);
                            }
                        }
					}
				}

			} else if (unitInstance.unit is AirUnit airUnit) {

				if (currentLandZone != null) {
					foreach (Zone hazardousZone in currentLandZone.HazardousAdjacencies) {
						GetZonesWithinRangeNonCombat(hazardousZone, unitInstance, range - 1, ref zonesInRange);
					}
				}
				foreach (Zone potentialZone in currentZone.Adjacencies) {
					GetZonesWithinRangeNonCombat(potentialZone, unitInstance, range - 1, ref zonesInRange);
				}

			} else if (unitInstance.unit is SeaUnit seaUnit) {

				if (currentLandZone != null) {
					Debug.LogError("wtf, a sea unit on a land zone???");
					return;
				}

				foreach (Zone potentialZone in currentZone.Adjacencies) {

					if (potentialZone is SeaZone potentialSeaZone) {

						bool isClearOfEnemies = true;
						// first, check if there are any enemies in the sea zone, if so,
						// we can't go there in non combat
                        List<Zone.UnitInstance> unitsInSeaZone = potentialSeaZone.GetUnits();

                        foreach (Zone.UnitInstance otherSeaUnit in unitsInSeaZone) {

                            if (!unitOwner.IsSeaMovementAlly(otherSeaUnit.owner)) {
                                isClearOfEnemies = false;
								break;
                            }
                        }

						if (isClearOfEnemies) {

							bool isValidLink = true; // check if it's a canal, and if it is, can we use it

							// if our link is canal, check if both sides allow us to share land with them
							// if so, we can use it.(even if it's been conquered this turn)

							Canal canal = GetCanalLink(potentialSeaZone, currentSeaZone);

							if (canal != null) {

								foreach (LandZone land in canal.Owners) {

									if (!unitOwner.IsLandMovementAlly(land.CurrentOwner)) {
										isValidLink = false;
										break;
									}
								}
							}

							if (isValidLink) {

								GetZonesWithinRangeNonCombat(potentialZone, unitInstance, range - 1, ref zonesInRange);
							}
						}

                    }

				}
			}

		} 
		//else {
            // *** end of movement rules for non combat movement

        if (!zonesInRange.Contains(currentZone)) {

			// air units have special rules where they can end up
            if (unitInstance.unit is AirUnit airUnit) {

				// air units must end up in a friendly land territory not taken this turn
				if (currentZone is LandZone landZone && 
					(GameManager.Instance.IsZoneTakenThisTurn(currentLandZone) ||
                    !unitOwner.IsLandMovementAlly(landZone.CurrentOwner))) {
					return;
				}

			
				if(currentZone is SeaZone seaZone) {
                    bool carrierAvailable = false;
					if (airUnit is Fighter fighter) {

						foreach (Zone.UnitInstance otherUnit in seaZone.GetUnits()) {

							if (otherUnit is Zone.CarrierInstance carrier &&
								unitOwner == carrier.owner) { // can only carry units of same country

								if (carrier.RemainingCapacity >= fighter.CarrierLoad) {
									carrierAvailable = true;
								}
								break;
							}
						}
						
					}
                    if (!carrierAvailable) {
                        return;
                    }
                }
                
            }
                
			zonesInRange.Add(currentZone);        
        }
        //}

	}

}

