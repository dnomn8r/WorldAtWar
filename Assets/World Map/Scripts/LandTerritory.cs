using UnityEngine;

[CreateAssetMenu(fileName = "LandTerritory", menuName = "InitialStates/LandTerritory", order = 1)]
public class LandTerritory : Territory{

	[SerializeField] private int landValue;
	
	public int Value {
#if UNITY_EDITOR
		set { landValue = value; }
#endif
		get { return landValue; } 
	}



}

