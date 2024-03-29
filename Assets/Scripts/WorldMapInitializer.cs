using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldMapInitializer : MonoBehaviour{

	[SerializeField] private List<MajorPower> majorPowers;

	[SerializeField] private List<MinorPower> minorPowers;

	private void Start() {

		InitializeOriginalTerritories();
	}

	private void InitializeOriginalTerritories() {

		LandZone[] allLandZones = GetComponentsInChildren<LandZone>();


		foreach (LandZone zone in allLandZones) {

			foreach (MajorPower majorPower in majorPowers) {

				foreach (LandTerritory currentTerritory in majorPower.LandTerritories) {

					if (zone.LandTerritory == currentTerritory) {

						SpriteRenderer renderer = zone.GetComponent<SpriteRenderer>();

						renderer.color = majorPower.OwnershipColor;
						
					}
				}
			}


			foreach (MinorPower minorPower in minorPowers) {

				foreach (LandTerritory currentTerritory in minorPower.LandTerritories) {

					if (zone.LandTerritory == currentTerritory) {

						SpriteRenderer renderer = zone.GetComponent<SpriteRenderer>();
		  
						renderer.color = minorPower.OwnershipColor;
						
					}
				}

			}

		}

	}

}

