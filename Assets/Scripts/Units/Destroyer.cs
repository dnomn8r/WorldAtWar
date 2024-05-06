using UnityEngine;

[CreateAssetMenu(fileName = "Destroyer", menuName = "Units/Destroyer", order = 1)]
public class Destroyer : SeaUnit{

    [SerializeField] private int attack;
    [SerializeField] private int defence;

    public override int FirstStat {
		get { return attack; }
	}
	public override int SecondStat {
		get { return defence; }
	}

	public bool CanReturnFireOnSubPreemptiveHit {
		get {
			return attack + defence >= 10;
		}
	}

	public bool CanFirstStrikeSubsWhenAttacking { 
		get {
			return attack + defence >= 14;
		}
	}


	public override int BaseCost {
		get {
			return attack + defence;
		}
	}

}

