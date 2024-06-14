using TMPro;
using UnityEngine;


public class ZoneUnitTypeDisplay : MonoBehaviour {

	[SerializeField] private SpriteRenderer unitTypeIcon;
	[SerializeField] private TextMeshPro unitTypeCount;

	public void SetUnitTypeAndCount(UnitType unitType, int count) {

		unitTypeIcon.sprite = unitType.Sprite;
		unitTypeCount.text = count.ToString();
	}

}
