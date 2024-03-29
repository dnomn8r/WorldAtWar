using UnityEngine;

public abstract class Unit : ScriptableObject{

	[SerializeField] private UnitType unitType;
	public UnitType UnitType { get { return unitType; } }


	[SerializeField] private int movement;
	public int Movement { get { return movement;} }

	public abstract int BaseCost { get; }
				
}

