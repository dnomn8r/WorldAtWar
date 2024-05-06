using UnityEngine;

[CreateAssetMenu(fileName = "Submarine", menuName = "Units/Submarine", order = 1)]
public class Submarine : SeaUnit{

	[SerializeField] private int attack;
    [SerializeField] private int defence;

    public override int FirstStat {
        get { return attack; }
    }
    public override int SecondStat {
        get { return defence; }
    }

    [SerializeField] private int airRetaliationStrength;
	public int AirRetaliationStrength {
		get { return airRetaliationStrength; }
	}

	public bool HasFirstStike {
		get {
			return attack + defence > 7;
		}
	}

	public bool HasAirOnlyDefence { 
		get {
			return attack + defence > 7;
		}
	}

	public override int BaseCost {
		get {
			return attack + defence;
		}
	}

}

