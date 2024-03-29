using UnityEngine;

[CreateAssetMenu(fileName = "Bomber", menuName = "Units/Bomber", order = 1)]
public class Bomber : Unit{

	[SerializeField] private int bombAttack;
	public int BombAttack { get { return bombAttack; } }

	[SerializeField] private int airStrength;
	public int AirStrength { get { return airStrength; } }

	public bool CanStrategicBomb {
		get {
			return BombAttack > 13;
		}
	}

	public int Payload {
		get {
			return BombAttack < 14 ? 0 : (BombAttack * 2) + AirStrength - Movement; // weak bombers don't carry units
		}
	}

	public int TechModifier{
		get {
				
			if(BombAttack > 8) {
				return 1;
			}

			if (BombAttack > 9) {
				return 2;
			}

			return (BombAttack < 6 ? -1 : 0) + (Movement < 6 ? -1 : 0);
		}
	}

	public override int BaseCost {
		get {
			return (BombAttack * 2) + AirStrength + Movement;
		}
	}


}

