using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct WaterTerritoryEntry {

	[SerializeField] private WaterTerritory waterTerritory;
	public WaterTerritory WaterTerritory { get { return waterTerritory; } }

	[SerializeField] private List<UnitEntry> unitEntries;
	public List<UnitEntry> UnitEntries { get { return unitEntries; } }
}

[CreateAssetMenu(fileName = "InitialWaterTerritoryState", menuName = "InitialStates/InitialWaterTerritoryState", order = 1)]
public class InitialWaterTerritoryState : ScriptableObject{

	[SerializeField] private List<WaterTerritoryEntry> territoryEntries;
	public List<WaterTerritoryEntry> TerritoryEntries { get { return territoryEntries; } }

}

