using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour{

    [System.Serializable]
    public struct MajorPowerTurn {
        public List<MajorPower> powers;
    }

    [SerializeField] private List<MajorPowerTurn> turnOrder = new List<MajorPowerTurn>();

    [SerializeField] private InitialGameState initialGameState;

    public struct General {
        public MajorPower owner;
        public LandTerritory currentTerritory;

        public General(MajorPower owner, LandTerritory territory) {
            this.owner = owner;
            this.currentTerritory = territory;
        }
    }

    private List<General> generals = new List<General>();

    public List<General> GetGeneralsAtTerritory(LandTerritory territory) {

        List<General> generalsAtLocation = new List<General>();

        for (int i = 0; i < generals.Count; i++) {

            if (generals[i].currentTerritory == territory) {

                generalsAtLocation.Add(generals[i]);
            }
        }

        return generalsAtLocation;
    }

    private static GameManager instance;
    public static GameManager Instance {
        get { return instance; }
    }

    // state variables
    public int currentTurnIndex = 0;

    private Dictionary<MajorPower, int> savedIPCs = new Dictionary<MajorPower, int>();
    private Dictionary<MajorPower, int> lendLease = new Dictionary<MajorPower, int>();


    public int GetSavedIPCs(MajorPower power) {
        int ipcs = 0;

        savedIPCs.TryGetValue(power, out ipcs);
     
        return ipcs;
    } 
    public int GetLendLease(MajorPower power) {
        int ll = 0;

        lendLease.TryGetValue(power, out ll);

        return ll;
    }


    private void Awake() {
        instance = this;
    }

    void Start() {

        StartNewGame(); // eventually have a 'load game' flow
    }

    private void StartNewGame() {

        if(initialGameState == null) {
            Debug.LogError("we need an initial game state!");
            return;
        }

        WorldMapManager.Instance.Initialize();


        InitializeGenerals();

        InitializeOwnershipState(initialGameState.InitialLandTerritoryOwnership);

        InitializeNeutralStates(initialGameState.InitialNeutralState);

        InitializeCountryStates(initialGameState.InitialCountryStates);

        HUD.Instance.Initialize();

        SetCurrentTurnIndex(0);
    }

    public bool IsCurrentTurn(MajorPower majorPower) {

        return turnOrder[currentTurnIndex].powers.Contains(majorPower);
    }

    private void SetCurrentTurnIndex(int turnIndex) {

        if (turnIndex < 0 || turnIndex >= turnOrder.Count) {
            Debug.Log("bad turn index! must be between 0 and " + (turnOrder.Count - 1));
        }

        currentTurnIndex = turnIndex;      
    }


    private void InitializeOwnershipState(InitialOwnershipState ownershipState) {

        foreach (InitialOwnershipState.OwnedTerritories ownership in ownershipState.Ownerships) {

            foreach (LandTerritory territory in ownership.territories) {

                LandZone landZone = WorldMapManager.Instance.GetZone(territory) as LandZone;

                landZone.SetCurrentOwner(ownership.country);
            }
        }
    }

    private void InitializeNeutralStates(InitialLandTerritoryState neutralLandTerritoryState) {

        foreach (LandTerritoryEntry landEntry in neutralLandTerritoryState.TerritoryEntries) {

            LandZone landZone = WorldMapManager.Instance.GetZone(landEntry.LandTerritory) as LandZone;

            landZone.SetFactory(landEntry.Factory);


            foreach (UnitEntry unitEntry in landEntry.UnitEntries) {

                landZone.AddUnits(landZone.CurrentOwner, unitEntry.Unit, unitEntry.Count);
            }

        }
    }

    private void InitializeCountryStates(List<InitialCountryState> initialCountryStates) {

        foreach (InitialCountryState countryState in initialCountryStates) {

            foreach (LandTerritoryEntry landEntry in countryState.TerritoryEntries) {

                LandZone landZone = WorldMapManager.Instance.GetZone(landEntry.LandTerritory) as LandZone;

                landZone.SetFactory(landEntry.Factory);

                foreach (UnitEntry unitEntry in landEntry.UnitEntries) {

                    landZone.AddUnits(countryState.Country, unitEntry.Unit, unitEntry.Count);
                }
            }

            foreach (WaterTerritoryEntry waterEntry in countryState.WaterTerritoryEntries) {

                SeaZone seaZone = WorldMapManager.Instance.GetZone(waterEntry.WaterTerritory) as SeaZone;

                foreach (UnitEntry unitEntry in waterEntry.UnitEntries) {

                    seaZone.AddUnits(countryState.Country, unitEntry.Unit, unitEntry.Count);
                }
            }
        }
    }

    private void InitializeGenerals() {

        foreach (MajorPower power in WorldMapManager.Instance.MajorPowers) {

            generals.Add(new General(power, power.CapitalTerritory));
        }
    }


}

