using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LandZoneInfoDisplay : MonoBehaviour {


	[SerializeField] private TextMeshPro zoneNameText;
	[SerializeField] private SpriteRenderer flagRenderer;

	[SerializeField] private ZoneValueDisplay zoneValueDisplay;

	[SerializeField] private List<ZoneUnitTypeDisplay> zoneUnitTypeDisplays = new List<ZoneUnitTypeDisplay>();

	private LandZone zone;
	public void SetZone(Zone zone) {

		this.zone = zone as LandZone;
	}

	public void Refresh() {

		if(zone == null) {
			return;
		}

		zoneNameText.text = zone.LandTerritory.name;

		zoneValueDisplay.SetValue(zone.Value);

		if (zone.factory != null) {
			zoneValueDisplay.GetComponent<SpriteRenderer>().sprite = zone.factory.FactoryIcon;
		}

		flagRenderer.sprite = zone.CurrentOwner.Flag;

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
