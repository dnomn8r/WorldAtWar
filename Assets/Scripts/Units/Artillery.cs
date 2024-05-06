using UnityEngine;

[CreateAssetMenu(fileName = "Artillery", menuName = "Units/Artillery", order = 1)]
public class Artillery : LandUnit{

	public bool LowLevelAntiAir {
		get {
			return FirstStat + SecondStat < 10;
		}
	}
	public bool AntiAir {
		get {
			return FirstStat + SecondStat < 18;
		}
	}

	public bool CanSupportShot {
		get {
			return FirstStat + SecondStat > 9;
		}
	}

	public bool CanShootAmphibiousAttackingShips {
		get {
			return FirstStat + SecondStat > 13;
		}
	}

	public bool PreemptiveShot {
		get {
			return FirstStat + SecondStat > 17;
		}
	}

	public bool CanMoveByAirTransport {
		get {
			return FirstStat + SecondStat <= 10;
		}
	}


}

