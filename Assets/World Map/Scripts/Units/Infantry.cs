using UnityEngine;

[CreateAssetMenu(fileName = "Infantry", menuName = "Units/Infantry", order = 1)]
public class Infantry : LandUnit{

	public bool CanGainRommelBonuses {
		get {
			return (Attack + Defence) > 4;
		}
	}

	public bool CanMoveBySeaOrAir {
		get {
			return (Attack + Defence) > 4;
		}
	}
	public bool CanAmphibiousAttackOrBecomeParatrooper {
		get {
			return (Attack + Defence) > 5;
		}
	}


}

