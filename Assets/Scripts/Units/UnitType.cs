using UnityEngine;

[CreateAssetMenu(fileName = "UnitType", menuName = "Units/UnitType", order = 1)]
public class UnitType : ScriptableObject{

	[SerializeField] private Sprite sprite;
	public Sprite Sprite {  get { return sprite; } }
				
}

