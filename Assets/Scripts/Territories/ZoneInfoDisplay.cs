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

	private struct UnitTypeCount {
		public UnitType unitType;
		public int count;

		public UnitTypeCount(UnitType t, int c) {
			unitType = t;
			count = c;	
		}
	}

	public virtual void Refresh() {

		if(zone == null) {
			return;
		}

		
		//Dictionary<UnitType, int> unitTypeCounts = new Dictionary<UnitType, int>();

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

				unitTypeCounts.Add(new UnitTypeCount(entry.unit.UnitType, entry.count));

			} else {

				UnitTypeCount unitTypeCount =  unitTypeCounts[addIndex];
				unitTypeCount.count += entry.count;
				unitTypeCounts[addIndex] = unitTypeCount;
			}

            unitTypeCounts.Sort(delegate (UnitTypeCount x, UnitTypeCount y) {
                if (x.unitType.Priority < y.unitType.Priority) {
                    return 1;
                } else if (x.unitType.Priority > y.unitType.Priority) {
                    return -1;
                }
                return 0;
            });
        }

		Debug.Log("unit type counts: " +  unitTypeCounts.Count);	

	
		for(int i=0;i<zoneInfoEntries.Count;i++) {

			if(unitTypeCounts.Count == i - 1) {

                zoneInfoEntries[i].root.SetActive(true);

				for(int j = 0; j < zoneInfoEntries[i].zoneUnitTypeDisplays.Count; ++j) {
					zoneInfoEntries[i].zoneUnitTypeDisplays[j].SetUnitTypeAndCount(unitTypeCounts[j].unitType, unitTypeCounts[j].count);
                }
            } else {

                zoneInfoEntries[i].root.SetActive(false);
            }
		}

    }


}
