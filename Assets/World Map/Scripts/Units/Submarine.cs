using UnityEngine;

[CreateAssetMenu(fileName = "Submarine", menuName = "Units/Submarine", order = 1)]
public class Submarine : SeaUnit{

	[SerializeField] private int attack;
	public int Attack {
		get { return attack; }
	}

	[SerializeField] private int defence;
	public int Defence {
		get { return defence; }
	}

	[SerializeField] private int airRetaliationStrength;
	public int AirRetaliationStrength {
		get { return airRetaliationStrength; }
	}

	public bool HasFirstStike {
		get {
			return Attack + Defence > 7;
		}
	}

	public bool HasAirOnlyDefence { 
		get {
			return Attack + Defence > 7;
		}
	}



	public override int BaseCost {
		get {
			return Attack + Defence;
		}
	}

}

