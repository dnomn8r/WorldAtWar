using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Units/Armor", order = 1)]
public class Armor : LandUnit{

	public int FirstInfantryRommelBonus {
		get {
			if(BaseCost > 13) {

				return 2;

			}else if(BaseCost > 6) {

				return 1;
			}
			return 0;
		}
	}
	public int SecondInfantryRommelBonus {
		get {
			if (BaseCost > 15) {

				return 2;

			} else if (BaseCost > 8) {

				return 1;
			}
			return 0;
		}
	}

	public int DefensiveInfantryRommelBonus {
		get {
			if (BaseCost > 11) {

				return 1;
			}
			return 0;
		}
	}

	public bool HasMetalTracks {
		get {
			return Attack > 4;
		}
	}



}

