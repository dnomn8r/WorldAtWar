using UnityEngine;

public abstract class LandUnit : Unit{

    [SerializeField] private int attack;
    [SerializeField] private int defence;

    public override int FirstStat { get { return attack; } }

    public override int SecondStat { get { return defence; } }

    public override int BaseCost {
		get {
			return FirstStat + SecondStat;
		}
	}

	public virtual int TransportLoad {
		get {
			return FirstStat + SecondStat;
		}
	}

}

