using System.Collections.Generic;

using UnityEngine;

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
}

