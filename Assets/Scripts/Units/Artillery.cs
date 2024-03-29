using UnityEngine;

[CreateAssetMenu(fileName = "Artillery", menuName = "Units/Artillery", order = 1)]
public class Artillery : LandUnit{

	public bool LowLevelAntiAir {
		get {
			return Attack + Defence < 10;
		}
	}
	public bool AntiAir {
		get {
			return Attack + Defence < 18;
		}
	}

	public bool CanSupportShot {
		get {
			return Attack + Defence > 9;
		}
	}

	public bool CanShootAmphibiousAttackingShips {
		get {
			return Attack + Defence > 13;
		}
	}

	public bool PreemptiveShot {
		get {
			return Attack + Defence > 17;
		}
	}

	public bool CanMoveByAirTransport {
		get {
			return Attack + Defence <= 10;
		}
	}


}

