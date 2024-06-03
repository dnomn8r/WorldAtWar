using TMPro;
using UnityEngine;


public class ZoneUnitTypeDisplay : MonoBehaviour {

	[SerializeField] private SpriteRenderer unitTypeIcon;
	[SerializeField] private TextMeshPro unitTypeCount;

	public void SetUnitTypeAndCount(Sprite unitIcon, int count) {

		unitTypeIcon.sprite = unitIcon;
		unitTypeCount.text = count.ToString();
	}

}
