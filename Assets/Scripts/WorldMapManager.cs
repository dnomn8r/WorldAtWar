using System.Collections.Generic;
using UnityEngine;

public class WorldMapManager : MonoBehaviour{

	[SerializeField] private List<MajorPower> majorPowers;

	[SerializeField] private List<MinorPower> minorPowers;

	[SerializeField] private Sprite neutralFlag;

	[Space]

	[SerializeField] private InitialGameState initialGameState;


	private Dictionary<LandTerritory, LandZone> landZoneMapping = new Dictionary<LandTerritory, LandZone>();

	private List<LandZone> allLandZones;


    private static WorldMapManager instance;
    public static WorldMapManager Instance {
        get { return instance; }
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {

		allLandZones = new List<LandZone>(GetComponentsInChildren<LandZone>());

        MapLandZones();

		InitializeOriginalTerritories();

		InitializeOwnershipState(initialGameState.InitialLandTerritoryOwnership);

		HUD.Instance.Initialize();
	}

	private void MapLandZones() {

		landZoneMapping.Clear();		

		foreach (LandZone zone in allLandZones) {

			landZoneMapping.Add(zone.LandTerritory, zone);
		}
	}

	private void InitializeOriginalTerritories() {

		List<LandZone> usedLandZones = new List<LandZone>();

		foreach (MajorPower majorPower in majorPowers) {

			foreach (LandTerritory currentTerritory in majorPower.LandTerritories) {

				landZoneMapping[currentTerritory].SetOriginalOwner(majorPower);

				usedLandZones.Add(landZoneMapping[currentTerritory]);
			}
		}

		foreach (MinorPower minorPower in minorPowers) {

			foreach (LandTerritory currentTerritory in minorPower.LandTerritories) {

				landZoneMapping[currentTerritory].SetOriginalOwner(minorPower);

				usedLandZones.Add(landZoneMapping[currentTerritory]);					
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

				landZoneMapping[territory].SetCurrentOwner(ownership.country);
			}
		}

	}

	public int GetIncome(Country country) {

		int totalIncome = 0;

		for(int i = 0;i<allLandZones.Count;i++) {

			if (allLandZones[i].CurrentOwner == country) {

				totalIncome += allLandZones[i].Value;
			}
		}

		return totalIncome;
	}

}

