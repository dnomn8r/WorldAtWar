using UnityEngine;

[CreateAssetMenu(fileName = "Carrier", menuName = "Units/Carrier", order = 1)]
public class Carrier : SeaUnit{

	[SerializeField] private int attack;
	public int Attack {
		get { return attack; }
	}

	[SerializeField] private int defence;
	public int Defence {
		get { return defence; }
	}

	[SerializeField] private int capacity;
	public int Capacity {
		get {
			return capacity;
		}
	}

	public override int BaseCost {
		get {

			int capacityDiscount = 0;

			if(Capacity > 33) {
				capacityDiscount = 5;
			} else if (Capacity > 29) {
				capacityDiscount = 4;
			} else if (Capacity > 27) {
				capacityDiscount = 3;
			} else if (Capacity > 23) {
				capacityDiscount = 2;
			} else if (Capacity > 19) {
				capacityDiscount = 1;
			}

			return Attack + Defence + Capacity - capacityDiscount;
		}
	}

}

