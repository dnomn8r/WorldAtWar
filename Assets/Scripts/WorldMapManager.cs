using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Search.SearchColumn;

public class WorldMapManager : MonoBehaviour{

	[SerializeField] private List<MajorPower> majorPowers;

	[SerializeField] private List<MinorPower> minorPowers;

	[SerializeField] private Sprite neutralFlag;

	[Space]

	[SerializeField] private InitialGameState initialGameState;


	private Dictionary<Territory, Zone> zoneMapping = new Dictionary<Territory, Zone>();
	private List<Zone> allZones;

	public struct General {
		public MajorPower owner;
		public LandTerritory currentTerritory;

		public General(MajorPower owner, LandTerritory territory) {
			this.owner = owner;
			this.currentTerritory = territory;
		}
	}

    private List<General> generals = new List<General>();

    public List<General> GetGeneralsAtTerritory(LandTerritory territory) {
		
		List<General> generalsAtLocation = new List<General>();

		for(int i=0;i<generals.Count;i++) {

			if (generals[i].currentTerritory == territory) {

                generalsAtLocation.Add(generals[i]);
			}
		}

		return generalsAtLocation;
	}

    private static WorldMapManager instance;
    public static WorldMapManager Instance {
        get { return instance; }
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {

		allZones = new List<Zone>(GetComponentsInChildren<Zone>());

        MapLandZones();

		InitializeOriginalTerritories();

        InitializeGenerals();

        InitializeOwnershipState(initialGameState.InitialLandTerritoryOwnership);

		InitializeNeutralStates(initialGameState.InitialNeutralState);

		InitializeCountryStates(initialGameState.InitialCountryStates);

		HUD.Instance.Initialize();
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

	private void InitializeOwnershipState(InitialOwnershipState ownershipState) {

		foreach(InitialOwnershipState.OwnedTerritories ownership in ownershipState.Ownerships) {

			foreach(LandTerritory territory in ownership.territories) {

                LandZone landZone = zoneMapping[territory] as LandZone;

                landZone.SetCurrentOwner(ownership.country);	
			}
		}
	}

	private void InitializeNeutralStates(InitialLandTerritoryState neutralLandTerritoryState) {

		foreach(LandTerritoryEntry landEntry in neutralLandTerritoryState.TerritoryEntries) {

            LandZone landZone = zoneMapping[landEntry.LandTerritory] as LandZone;

            landZone.SetFactory(landEntry.Factory);


			foreach (UnitEntry unitEntry in landEntry.UnitEntries) {

				landZone.AddUnits(landZone.CurrentOwner, unitEntry.Unit, unitEntry.Count);
			}

        }
	}

	private void InitializeCountryStates(List<InitialCountryState> initialCountryStates) {

		foreach(InitialCountryState countryState in initialCountryStates) {

			foreach (LandTerritoryEntry landEntry in countryState.TerritoryEntries) {

				LandZone landZone = zoneMapping[landEntry.LandTerritory] as LandZone;

				landZone.SetFactory(landEntry.Factory);

				foreach (UnitEntry unitEntry in landEntry.UnitEntries) {

                    landZone.AddUnits(countryState.Country, unitEntry.Unit, unitEntry.Count);
                }
			}

			foreach (WaterTerritoryEntry waterEntry in countryState.WaterTerritoryEntries) {

                SeaZone seaZone = zoneMapping[waterEntry.WaterTerritory] as SeaZone;

                foreach (UnitEntry unitEntry in waterEntry.UnitEntries) {

                    seaZone.AddUnits(countryState.Country, unitEntry.Unit, unitEntry.Count);
                }
            }
		}
	}

	private void InitializeGenerals() {

		foreach(MajorPower power in majorPowers) {

			generals.Add(new General(power, power.CapitalTerritory));
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

}

