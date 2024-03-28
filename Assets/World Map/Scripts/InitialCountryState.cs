using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "InitialCountryState", menuName = "InitialStates/InitialCountryState", order = 1)]
public class InitialCountryState : ScriptableObject{

    [SerializeField] private Country country;
    public Country Country { get { return country; } }

    [SerializeField] private List<LandTerritoryEntry> territoryEntries;
    public List<LandTerritoryEntry> TerritoryEntries { get { return territoryEntries; } }

    [SerializeField] private List<WaterTerritoryEntry> waterTerritoryEntries;
    public List<WaterTerritoryEntry> WaterTerritoryEntries { get { return waterTerritoryEntries; } }


}

