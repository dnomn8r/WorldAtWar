using System;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour{

    public enum TurnPhase { COLLECT_INCOME, COMBAT_ORDERS, COMBAT, NON_COMBAT_MOVEMENT, BUILD_RECRUIT}

    private static string GetPhaseName(TurnPhase phase) {

        switch (phase) {

            case TurnPhase.COLLECT_INCOME:
                return "Collect Income";
            case TurnPhase.COMBAT_ORDERS:
                return "Combat Orders";
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
    public event Action OnTurnChanged;
    public event Action OnRoundChanged;

    public event Action OnPendingLendLeaseChanged;

    private List<LandZone> zonesTakenThisTurn = new List<LandZone>();
    public bool IsZoneTakenThisTurn(LandZone zone) {
        return zonesTakenThisTurn.Contains(zone);
    }

    // state variables

    public int currentRound = 0;

    private int _currentTurnIndex = -1;

    private int _currentPhaseIndex = -1;
    public int CurrentPhaseIndex {
        get { return _currentPhaseIndex; }
        private set {
            _currentPhaseIndex = value;

            //Debug.Log("current phase: " + _currentPhaseIndex);

            OnPhaseChanged?.Invoke();
        }
    }

    public void EndPhase() {

        if ((TurnPhase)CurrentPhaseIndex == TurnPhase.COLLECT_INCOME) {

            foreach(KeyValuePair<MajorPower, int> kvp in pendingReceivedLendLease) {

                if (!lendLease.ContainsKey(kvp.Key)) {
                    lendLease.Add(kvp.Key, 0);
                }
                lendLease[kvp.Key] += kvp.Value;
            }

            MajorPowerTurn majorPowerTurn = GetCurrentlyActivePowers();

            // add actual income available for this turn
            foreach (MajorPower power in majorPowerTurn.powers) {

                if(!currentIPCs.ContainsKey(power)) {
                    currentIPCs.Add(power, 0);  
                }

                int sentLendLease = 0;
                if(pendingSentLendLease.ContainsKey(power)) {
                    sentLendLease = pendingSentLendLease[power];
                }

                currentIPCs[power] = WorldMapManager.Instance.GetIncome(power) - sentLendLease;
            }
            
            pendingReceivedLendLease.Clear();
            pendingSentLendLease.Clear();
        }

        int maxPhases = Enum.GetValues(typeof(TurnPhase)).Length;

        if(CurrentPhaseIndex + 1 < maxPhases) {

            CurrentPhaseIndex = CurrentPhaseIndex + 1;
        } else {
          
            CurrentTurn = CurrentTurn + 1;
        }
    }

    public int CurrentRound {
        get {
            return currentRound;
        }
        set {
            currentRound = value;

            //Debug.Log("current round: " + value);

            CurrentTurn = 0;
   
            OnRoundChanged?.Invoke();   
        }
    }

    public int CurrentTurn{

        set {
            if (value < 0 || value > turnOrder.Count) {
                Debug.Log("bad turn index! must be between 0 and " + (turnOrder.Count));
            }
            if(_currentTurnIndex == value) {
                Debug.LogError("We shouldn't be trying to change turn to what it already is!");
            }

            //Debug.Log("current turn: " + value);

            zonesTakenThisTurn.Clear(); // conquered zones can now be landed on

            if (value < turnOrder.Count) {

                if (_currentTurnIndex >= 0) {
                    MajorPowerTurn majorPowerTurn = GetCurrentlyActivePowers();

                    foreach (MajorPower power in majorPowerTurn.powers) {

                        if (!savedIPCs.ContainsKey(power)) {
                            savedIPCs.Add(power, 0);
                        }
                        savedIPCs[power] += currentIPCs[power];
                    }
                }

                _currentTurnIndex = value;

                CurrentPhaseIndex = 0;

                OnTurnChanged?.Invoke();
            } else {

                CurrentRound = CurrentRound + 1;             
            }
          
 
        }

        get {
            return _currentTurnIndex;
        }
    }

    private Dictionary<MajorPower, int> currentIPCs = new Dictionary<MajorPower, int>();

    private Dictionary<MajorPower, int> savedIPCs = new Dictionary<MajorPower, int>();
    
    private Dictionary<MajorPower, int> lendLease = new Dictionary<MajorPower, int>();


    private Dictionary<MajorPower, int> pendingSentLendLease = new Dictionary<MajorPower, int>();
    private Dictionary<MajorPower, int> pendingReceivedLendLease = new Dictionary<MajorPower, int>();


    public int GetCurrentIPCs(MajorPower power) {

        int IPCs = 0;
        currentIPCs.TryGetValue(power, out IPCs);
        return IPCs;
    }

    public int GetCurrentTotalIncome(MajorPower power) {

        return WorldMapManager.Instance.GetIncome(power) + GetSavedIPCs(power) + GetTotalReceivingLendLease(power) - GetPendingSentLendlease(power);

    }

    public int GetTotalReceivingLendLease(MajorPower power) {

        int lendLeaseValue = 0;
        lendLease.TryGetValue(power, out lendLeaseValue);

        int pendingLendLeaseValue = 0;
        pendingReceivedLendLease.TryGetValue(power, out pendingLendLeaseValue);

        return lendLeaseValue + pendingLendLeaseValue;
    }

    public void ChangePendingLendLease(MajorPower source, MajorPower recipient, int delta) {

        if(!pendingSentLendLease.ContainsKey(source)) {
            pendingSentLendLease.Add(source, 0);
        }
        if (!pendingReceivedLendLease.ContainsKey(recipient)) {
            pendingReceivedLendLease.Add(recipient, 0);
        }

        if (GetPendingSentLendlease(source) + delta < 0 || 
            pendingReceivedLendLease[recipient] + delta < 0) {

            //Debug.LogError("should never have less than 0 pending lend lease!");
            return;
        }

        if (GetPendingSentLendlease(source) + delta > Mathf.FloorToInt(WorldMapManager.Instance.GetIncome(source) / 3)) {

            //Debug.LogError("should never have more than 1/3 of income! We have income: " + WorldMapManager.Instance.GetIncome(source) + " and sent " + pendingSentLendLease[source]);
            return;
        }

        pendingSentLendLease[source] += delta;

        pendingReceivedLendLease[recipient] += delta;

        OnPendingLendLeaseChanged?.Invoke();
    }

    public int GetPendingSentLendlease(MajorPower power) {

        int sentLL = 0;
        pendingSentLendLease.TryGetValue(power, out sentLL);

        return sentLL;
    }

    public int GetPendingReceivedLendlease(MajorPower power) {

        int receivedLL = 0;
        pendingReceivedLendLease.TryGetValue(power, out receivedLL);

        return receivedLL;
    }

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

        return GetPhaseName((TurnPhase)CurrentPhaseIndex);
    }
    public MajorPowerTurn GetCurrentlyActivePowers() {

        return turnOrder[CurrentTurn];
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

        CurrentRound = 0;
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

