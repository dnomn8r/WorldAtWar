using UnityEngine;

[CreateAssetMenu(fileName = "Destroyer", menuName = "Units/Destroyer", order = 1)]
public class Destroyer : SeaUnit{

	[SerializeField] private int attack;
	public int Attack {
		get { return attack; }
	}

	[SerializeField] private int defence;
	public int Defence {
		get { return defence; }
	}

	public bool CanReturnFireOnSubPreemptiveHit {
		get {
			return Attack + Defence >= 10;
		}
	}

	public bool CanFirstStrikeSubsWhenAttacking { 
		get {
			return Attack + Defence >= 14;
		}
	}


	public override int BaseCost {
		get {
			return Attack + Defence;
		}
	}

}

