using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour{

    public enum TurnPhase { COLLECT_INCOME, COMBAT_MOVEMENT, COMBAT, NON_COMBAT_MOVEMENT, BUILD_RECRUIT}

    private static string GetPhaseName(TurnPhase phase) {

        switch (phase) {

            case TurnPhase.COLLECT_INCOME:
                return "Collect Income";
            case TurnPhase.COMBAT_MOVEMENT:
                return "Combat Movement";
            case TurnPhase.COMBAT:
                return "Combat";
            case TurnPhase.NON_COMBAT_MOVEMENT:
                return "Non Combat Movement";
            case TurnPhase.BUILD_RECRUIT:
                return "Build and Recruit";

        }

        return "BAD PHASE!";
    }

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
        get { 

            return instance; 
        }
    }

    public event Action OnPhaseChanged;

    // state variables

    public int currentRound = 0;

    private int _currentTurnIndex = -1;

    public int CurrentTurnIndex{

        set {
            if (value < 0 || value >= turnOrder.Count) {
                Debug.Log("bad turn index! must be between 0 and " + (turnOrder.Count - 1));
            }
            if(_currentTurnIndex == value) {
                Debug.LogError("We shouldn't be trying to change turn to what it already is!");
            }

            _currentTurnIndex = value;

            OnPhaseChanged?.Invoke();
        }

        get {
            return _currentTurnIndex;
        }
    }

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

    public string GetCurrentPhaseName() {

        return GetPhaseName((TurnPhase)CurrentTurnIndex);
    }
    public MajorPowerTurn GetCurrentlyActivePowers() {

        return turnOrder[CurrentTurnIndex];
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

        CurrentTurnIndex = 0;


        HUD.Instance.Initialize();
    }

    public bool IsCurrentTurn(MajorPower majorPower) {

        return turnOrder[_currentTurnIndex].powers.Contains(majorPower);
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

