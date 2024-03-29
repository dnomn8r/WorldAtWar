using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialOwnershipState", menuName = "InitialStates/InitialOwnershipState", order = 1)]
public class InitialOwnershipState : ScriptableObject{

	[System.Serializable]
	public struct OwnedTerritories {

		[SerializeField] public Country country;

		[SerializeField] public List<LandTerritory> territories;

	}

	[SerializeField] private List<OwnedTerritories> ownerships = new List<OwnedTerritories>();



}

