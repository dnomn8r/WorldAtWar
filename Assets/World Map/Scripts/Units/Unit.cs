using UnityEngine;

public abstract class Unit : ScriptableObject{

	[SerializeField] private int attack;
	public int Attack { get { return attack; } }

	[SerializeField] private int defence;
	public int Defence { get { return defence;} }

	[SerializeField] private int movement;
	public int Movement { get { return movement;} }


	public virtual int BaseCost {
		get {
			return Attack + Defence;
		}
	}

	public virtual int TransportLoad {
		get {
			return Attack + Defence;
		}
	}

}

