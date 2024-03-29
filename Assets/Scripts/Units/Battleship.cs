using UnityEngine;

[CreateAssetMenu(fileName = "Battleship", menuName = "Units/Battleship", order = 1)]
public class Battleship : SeaUnit{

	[SerializeField] private int firstGunStrength;
	public int FirstGunStrength {
		get { return firstGunStrength; }
	}

	[SerializeField] private int secondGunStrength;
	public int SecondGunStrength {
		get { return secondGunStrength; }
	}

	[SerializeField] private int thirdGunStrength;
	public int ThirdGunStrength {
		get { return thirdGunStrength; }
	}

	public override int BaseCost {
		get {
			return (FirstGunStrength + SecondGunStrength + ThirdGunStrength)*2;
		}
	}

	public int Resistance {
		get {

			int totalStrength = (FirstGunStrength + SecondGunStrength + ThirdGunStrength) * 2;

			if (totalStrength > 44) {
				return -3;
			}

			if(totalStrength > 41) {
				return -2;
			}

			if(totalStrength > 37) {
				return -1;
			}

			return 0;
		}
	}

}

