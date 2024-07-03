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

        MapLandZones();

		InitializeOriginalTerritories();

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

}

