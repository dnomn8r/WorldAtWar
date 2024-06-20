using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ZoneUnitTypeDisplay : MonoBehaviour {

	[SerializeField] private SpriteRenderer unitTypeIcon;
	[SerializeField] private TextMeshPro unitTypeCount;

	public void SetUnitTypeAndCounts(UnitType unitType, Dictionary<Country, int> counts) {

		unitTypeIcon.sprite = unitType.Sprite;

		List<Country> ownerCountries = new List<Country>();
		int maxUnitCount = 0;

		int totalCount = 0;
		foreach (KeyValuePair<Country, int> kvp in counts) {
			totalCount += kvp.Value;

			if(kvp.Value > maxUnitCount) {
				ownerCountries.Insert(0, kvp.Key);
			} else {
				ownerCountries.Add(kvp.Key);
			}
		}

		unitTypeIcon.color = ownerCountries[0].OwnershipColor;

		unitTypeCount.text = totalCount.ToString();
	}

}
