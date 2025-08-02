using UnityEngine;

public abstract class Unit : ScriptableObject{

	[SerializeField] private UnitType unitType;
    public UnitType UnitType { get { return unitType; } }

    [SerializeField] private int hitpoints = 1;
	public int Hitpoints { get { return hitpoints; } }	

    public abstract int FirstStat { get; }

    public abstract int SecondStat { get; }


    [SerializeField] private int movement;
	public int Movement { get { return movement;} }

	public abstract int BaseCost { get; }

				
}

