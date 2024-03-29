using UnityEngine;

public abstract class LandUnit : Unit{

	[SerializeField] private int attack;
	public int Attack { get { return attack; } }

	[SerializeField] private int defence;
	public int Defence { get { return defence; } }

	public override int BaseCost {
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

