using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

	private static GameManager instance;
	public static GameManager Instance {
		get { return instance; }
	}

	[SerializeField] private Dictionary<LandZone, Country> ownershipState = new Dictionary<LandZone, Country>();

	private void Awake() {
		instance = this;	
	}

	public void SetOwner(LandZone zone, Country country) {

		ownershipState[zone] = country;


		SpriteRenderer[] renderers = zone.GetComponentsInChildren<SpriteRenderer>();

		foreach (SpriteRenderer renderer in renderers) {

			if (renderer.GetComponent<ZoneValueDisplay>() == null && renderer.GetComponent<ZoneNameDisplay>() == null) {
				renderer.color = country.OwnershipColor;
			}
		}
	}	

	

}

