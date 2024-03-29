using System.Collections.Generic;
using UnityEngine;

public class WorldMapInitializer : MonoBehaviour{

	[SerializeField] private List<MajorPower> majorPowers;

	[SerializeField] private List<MinorPower> minorPowers;

	private void Start() {

		InitializeOriginalTerritories();
	}

	private void InitializeOriginalTerritories() {

		List<LandZone> currentLandZones = new List<LandZone>(GetComponentsInChildren<LandZone>());

		List<LandZone> usedLandZones = new List<LandZone>();

		foreach (MajorPower majorPower in majorPowers) {

			foreach (LandTerritory currentTerritory in majorPower.LandTerritories) {

				foreach (LandZone zone in currentLandZones) {

					if (zone.LandTerritory == currentTerritory) {

						if (zone.LandTerritory == currentTerritory) {

							GameManager.Instance.SetOwner(zone, majorPower);

							usedLandZones.Add(zone);
							break;
						}
					}
				}
			}
		}

		foreach (MinorPower minorPower in minorPowers) {

			foreach (LandTerritory currentTerritory in minorPower.LandTerritories) {

				foreach (LandZone zone in currentLandZones) {

					if (zone.LandTerritory == currentTerritory) {

						GameManager.Instance.SetOwner(zone, minorPower);

						usedLandZones.Add(zone);
						break;
					}
				}
			}
		}

		foreach(LandZone zone in usedLandZones) {
			currentLandZones.Remove(zone);
		}

		foreach(LandZone zone in currentLandZones) {

			Country newCountry = ScriptableObject.CreateInstance<Country>();
			newCountry.name = zone.name;

			GameManager.Instance.SetOwner(zone, newCountry);
		}

	}

}

