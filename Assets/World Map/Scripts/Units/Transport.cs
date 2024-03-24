using UnityEngine;

[CreateAssetMenu(fileName = "Transport", menuName = "Units/Transport", order = 1)]
public class Transport : SeaUnit{

	[SerializeField] private int capacity;
	public int Capacity {
		get { return capacity; }
	}

	[SerializeField] private bool nonCombat = false;
	public bool NonCombat {
		get { return nonCombat; }
	}

	public int Defence {
		get { 
			if(nonCombat) {
				return 0;
			}else if(capacity < 12) {
				return 1;
			}else if(capacity < 18) {
				return 2;
			} else {
				return 3;
			}		
		}
	}

	public override int BaseCost {
		get {

			int cost = Capacity;

			if(nonCombat) {
				cost -= 3;
			}

			if(Capacity > 23) {
				cost -= 6;
			}else if(Capacity > 21) {
				cost -= 5;
			}else if(Capacity > 19) {
				cost -= 4;
			}else if(Capacity > 17) {
				cost -= 3;
			}else if(Capacity > 15) {
				cost -= 2;
			}else if(Capacity > 11) {
				cost -= 1;
			}

			return cost;
		}
	}

}

