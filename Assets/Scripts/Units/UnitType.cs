using UnityEngine;

[CreateAssetMenu(fileName = "UnitType", menuName = "Units/UnitType", order = 1)]
public class UnitType : ScriptableObject{

	[SerializeField] private int priority;
	[SerializeField] private Sprite sprite;
	public Sprite Sprite {  get { return sprite; } }

	public int Priority {
		get { return priority; }
	}		
}

