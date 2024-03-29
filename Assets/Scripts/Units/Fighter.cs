using UnityEngine;

[CreateAssetMenu(fileName = "Fighter", menuName = "Units/Fighter", order = 1)]
public class Fighter : Unit{

	[SerializeField] private int landSeaStrength;
	public int LandSeaStrength { get { return landSeaStrength; } }

	[SerializeField] private int airStrength;
	public int AirStrength { get { return airStrength; } }

	public bool CanBeDispatched {
		get {
			return (LandSeaStrength + AirStrength + Movement) > 10;
		}
	}

	public int TechModifier{
		get {

			if((LandSeaStrength + AirStrength + Movement) < 10) {
				return -2;
			}

			if(LandSeaStrength + AirStrength + Movement > 16) {				
				return 2;
			}

			return 0;
		}
	}

	public int CarrierLoad {
		get {
			return LandSeaStrength + AirStrength;
		}
	}

	public override int BaseCost {
		get {
			return LandSeaStrength + AirStrength + Movement;
		}
	}


}

