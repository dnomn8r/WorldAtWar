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

		Refresh();
	}

	private struct UnitTypeCount {
		public UnitType unitType;
		public Dictionary<Country, int> countryCounts;

		public UnitTypeCount(UnitType t, Country country, int count) {
			unitType = t;
            countryCounts = new Dictionary<Country, int> {
                { country, count }
            };
        }
		public void AddCount(Country c, int count) {

			if (countryCounts.ContainsKey(c)) {
				countryCounts[c] += count;
			} else {
				countryCounts.Add(c, count);
			}
		}

	}

	public virtual void Refresh() {

		if(zone == null) {
			return;
		}

		if(zone.GetUnits().Count == 0 ) {
			gameObject.SetActive(false);
			return;
		} else {
			gameObject.SetActive(true);
		}
	

        List<UnitTypeCount> unitTypeCounts = new List<UnitTypeCount>();

        foreach (Zone.UnitOwnershipEntry entry in zone.GetUnits()) {

			int addIndex = -1;

			for (int i = 0; i < unitTypeCounts.Count; i++) {

				if (unitTypeCounts[i].unitType == entry.unit.UnitType) {
					addIndex = i;
					break;
				}
			}

			if(addIndex == -1) {

				unitTypeCounts.Add(new UnitTypeCount(entry.unit.UnitType, entry.owner, entry.count));

			} else {

				UnitTypeCount unitTypeCount =  unitTypeCounts[addIndex];
				unitTypeCount.AddCount(entry.owner, entry.count);
				unitTypeCounts[addIndex] = unitTypeCount;
			}

            unitTypeCounts.Sort(delegate (UnitTypeCount x, UnitTypeCount y) {
                if (x.unitType.Priority > y.unitType.Priority) {
                    return 1;
                } else if (x.unitType.Priority < y.unitType.Priority) {
                    return -1;
                }
                return 0;
            });
        }

		Debug.Log("------------------ doing zone: " + zone.name);
		for(int i=0;i<zoneInfoEntries.Count;i++) {

			if(i == unitTypeCounts.Count - 1 && unitTypeCounts.Count > 0) {

                zoneInfoEntries[i].root.SetActive(true);

				for(int j = 0; j < zoneInfoEntries[i].zoneUnitTypeDisplays.Count; ++j) {
					zoneInfoEntries[i].zoneUnitTypeDisplays[j].SetUnitTypeAndCounts(unitTypeCounts[j].unitType, unitTypeCounts[j].countryCounts);
                }
            } else {

                zoneInfoEntries[i].root.SetActive(false);
            }
		}

    }


}
