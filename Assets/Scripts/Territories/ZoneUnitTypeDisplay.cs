using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ZoneUnitTypeDisplay : MonoBehaviour {

	[SerializeField] private SpriteRenderer unitTypeIcon;
	[SerializeField] private TextMeshPro unitTypeCount;

	[SerializeField] private List<SpriteRenderer> flagIcons = new List<SpriteRenderer>();

    int maxUnitCount = 0; 

    public void SetUnitTypeAndCounts(UnitType unitType, Dictionary<Country, int> counts) {

		unitTypeIcon.sprite = unitType.Sprite;

		List<Country> ownerCountries = new List<Country>();
		

		int totalCount = 0;
		foreach (KeyValuePair<Country, int> kvp in counts) {
			totalCount += kvp.Value;

			Debug.Log("adding count: " + kvp.Value + " for country: " + kvp.Key.name + " and unit type: " + unitType.name);

			if(kvp.Value > maxUnitCount) {
				ownerCountries.Insert(0, kvp.Key);
			} else {
				ownerCountries.Add(kvp.Key);
			}
		}


		unitTypeIcon.color = ownerCountries[0].OwnershipColor;

		for(int i = 1; i < ownerCountries.Count; ++i) {
			flagIcons[i - 1].enabled = true;
			flagIcons[i - 1].sprite = ownerCountries[i].Flag;
		}

		for(int i=ownerCountries.Count; i < flagIcons.Count+1; ++i) {
			flagIcons[i-1].enabled = false;
		}

		unitTypeCount.text = totalCount.ToString();
	}

}
