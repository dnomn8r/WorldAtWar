using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitEntry {
	[SerializeField] private Unit unit;
	public Unit	Unit { get { return unit; } }

	[SerializeField] private int count;
	public int Count { get { return count; } }
}

[System.Serializable]
public struct LandTerritoryEntry {

	[SerializeField] private LandTerritory landTerritory;
	public LandTerritory LandTerritory { get { return landTerritory; } }

	[SerializeField] private List<UnitEntry> unitEntries;
	public List<UnitEntry> UnitEntries { get { return unitEntries; } }

	[SerializeField] private Factory factory;
	public Factory Factory { get { return factory; } }
}

[System.Serializable]
public struct WaterTerritoryEntry{

    [SerializeField] private WaterTerritory waterTerritory;
    public WaterTerritory WaterTerritory { get { return waterTerritory; } }

    [SerializeField] private List<UnitEntry> unitEntries;
    public List<UnitEntry> UnitEntries { get { return unitEntries; } }
}


