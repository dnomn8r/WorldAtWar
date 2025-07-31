using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Carrier", menuName = "Units/Carrier", order = 1)]
public class Carrier : SeaUnit{

	[SerializeField] private int attack;
	[SerializeField] private int defence;


    public override int FirstStat {
        get {
            return attack;
        }
    }
    public override int SecondStat {
        get {
            return defence;
        }
    }

    [SerializeField] private int capacity;
	public int MaxCapacity {
		get {
			return capacity;
		}
	}

	public override int BaseCost {
		get {

			int capacityDiscount = 0;

			if(MaxCapacity > 33) {
				capacityDiscount = 5;
			} else if (MaxCapacity > 29) {
				capacityDiscount = 4;
			} else if (MaxCapacity > 27) {
				capacityDiscount = 3;
			} else if (MaxCapacity > 23) {
				capacityDiscount = 2;
			} else if (MaxCapacity > 19) {
				capacityDiscount = 1;
			}

			return attack + defence + MaxCapacity - capacityDiscount;
		}
	}

}

