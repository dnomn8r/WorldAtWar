using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InitialLandTerritoryState", menuName = "InitialStates/InitialLandTerritoryState", order = 1)]
public class InitialLandTerritoryState : ScriptableObject{

	[SerializeField] private List<LandTerritoryEntry> territoryEntries;
	public List<LandTerritoryEntry> TerritoryEntries { get { return territoryEntries; } }

}

