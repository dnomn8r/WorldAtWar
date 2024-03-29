using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Factory", menuName = "Units/Factory", order = 1)]
public class Factory : ScriptableObject{

	[SerializeField] private FactoryType factoryType;
	public FactoryType FactoryType { get { return factoryType; } }

	[SerializeField] private List<UnitType> validUnitTypes;

	public bool CanBuildUnit(Unit unit) {

		return validUnitTypes.Contains(unit.UnitType);
	}

	[SerializeField] private int cost;
	public int BaseCost {
		get {
			return cost;
		}
	}
}

