using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct UnitCostModifier {

	[SerializeField] UnitType unitType;
	public UnitType UnitType { get { return unitType; } }

	[SerializeField] int modifier;
	public int Modifier { get { return modifier; } }
}

[System.Serializable]
public struct FactoryCostModifier {

	[SerializeField] FactoryType factoryType;
	public FactoryType FactoryType { get { return factoryType; } }

	[SerializeField] int modifier;
	public int Modifier { get { return modifier; } }
}

[CreateAssetMenu(fileName = "MajorPower", menuName = "InitialStates/MajorPower", order = 1)]
public class MajorPower : Country{

	[SerializeField] List<UnitCostModifier> unitCostModifiers;
	public List<UnitCostModifier> UnitCostModifiers { get { return unitCostModifiers; } }

	[SerializeField] private List<FactoryCostModifier> factoryCostModifiers;
	public List<FactoryCostModifier> FactoryCostModifiers { get { return factoryCostModifiers; } }

	[SerializeField] private LandTerritory capitalTerritory;
	public LandTerritory CapitalTerritory { get { return capitalTerritory; } }

	//[SerializeField] private bool canSendLendLease = false;
	public bool CanSendLL { get { return ipcAllies.Count > 0; } }

	[SerializeField, FormerlySerializedAs("allies")] private List<MajorPower> ipcAllies = new List<MajorPower>();

	[SerializeField, FormerlySerializedAs("movementAllies")] private List<MajorPower> landMovementAllies = new List<MajorPower>();
	[SerializeField] private List<MajorPower> seaMovementAllies = new List<MajorPower>();
	
	[SerializeField] private List<MajorPower> railroadAllies = new List<MajorPower>();

	public bool IsIPCAlly(MajorPower potentialAlly) {
		return ipcAllies.Contains(potentialAlly);
	}

	public bool IsLandMovementAlly(Country potentialAlly) {
		if (potentialAlly is MajorPower majorPower) {
			return landMovementAllies.Contains(majorPower);
		}
		return false;
	}
    public bool IsSeaMovementAlly(Country potentialAlly) {
		if (potentialAlly is MajorPower majorPower) {
			return seaMovementAllies.Contains(majorPower);
		}
		return true; // minor powers allow us to share with sea zones with their units
    }

    public bool IsRailroadAlly(Country potentialAlly) {
		if (potentialAlly is MajorPower majorPower) {
			return railroadAllies.Contains(majorPower);
		}
		return false;
    }
}

