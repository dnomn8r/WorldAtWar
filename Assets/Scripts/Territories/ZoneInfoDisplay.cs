using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ZoneInfoDisplay : MonoBehaviour {

	[System.Serializable]
	public struct ZoneInfoEntry {

		public GameObject root;
		public List<ZoneUnitTypeDisplay> zoneUnitTypeDisplays;
	}

	[SerializeField] private List<ZoneInfoEntry> zoneInfoEntries;

	protected Zone zone;
	public void SetZone(Zone zone) {

		this.zone = zone;
	}

	public virtual void Refresh() {

		if(zone == null) {
			return;
		}


		Dictionary<UnitType, int> unitTypeCounts = new Dictionary<UnitType, int>();

		foreach(Zone.UnitOwnershipEntry entry in zone.GetUnits()) {

			if (!unitTypeCounts.ContainsKey(entry.unit.UnitType)) {
				unitTypeCounts.Add(entry.unit.UnitType, 0);
			}

			unitTypeCounts[entry.unit.UnitType] += entry.count;
		}

		int currentIndex = 0;
		//foreach(KeyValuePair<UnitType, int> entry in unitTypeCounts) {

		//	zoneUnitTypeDisplays[currentIndex].gameObject.SetActive(true);
		//	zoneUnitTypeDisplays[currentIndex].SetUnitTypeAndCount(entry.Key.Sprite, entry.Value);

		//	currentIndex++;
		//}

		//for(int i=currentIndex;i<zoneUnitTypeDisplays.Count;i++) {
		//	zoneUnitTypeDisplays[i].gameObject.SetActive(false);
		//}
    }


}
