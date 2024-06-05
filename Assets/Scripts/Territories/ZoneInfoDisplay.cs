using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ZoneInfoDisplay : MonoBehaviour {

	[SerializeField] private TextMeshPro zoneNameText;

	[SerializeField] private List<ZoneUnitTypeDisplay> zoneUnitTypeDisplays = new List<ZoneUnitTypeDisplay>();

	protected Zone zone;
	public void SetZone(Zone zone) {

		this.zone = zone;
	}

	public virtual void Refresh() {

		if(zone == null) {
			return;
		}

		if (zoneNameText != null) {
			zoneNameText.text = zone.name;
		}


		Dictionary<UnitType, int> unitTypeCounts = new Dictionary<UnitType, int>();

		foreach(Zone.UnitOwnershipEntry entry in zone.GetUnits()) {

			if (!unitTypeCounts.ContainsKey(entry.unit.UnitType)) {
				unitTypeCounts.Add(entry.unit.UnitType, 0);
			}

			unitTypeCounts[entry.unit.UnitType] += entry.count;
		}

		int currentIndex = 0;
		foreach(KeyValuePair<UnitType, int> entry in unitTypeCounts) {

			zoneUnitTypeDisplays[currentIndex].gameObject.SetActive(true);
			zoneUnitTypeDisplays[currentIndex].SetUnitTypeAndCount(entry.Key.Sprite, entry.Value);

			currentIndex++;
		}

		for(int i=currentIndex;i<zoneUnitTypeDisplays.Count;i++) {
			zoneUnitTypeDisplays[i].gameObject.SetActive(false);
		}
    }


}
