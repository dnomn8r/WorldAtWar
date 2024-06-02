using TMPro;
using UnityEngine;


public class LandZoneInfoDisplay : MonoBehaviour {


	[SerializeField] private TextMeshPro zoneNameText;
	[SerializeField] private SpriteRenderer flagRenderer;

	[SerializeField] private ZoneValueDisplay zoneValueDisplay;

	public void SetLandZoneInfo(LandZone zone, Zone.UnitOwnershipEntry ownership) {

		zoneNameText.text = zone.LandTerritory.name;

		zoneValueDisplay.SetValue(zone.Value);
		zoneValueDisplay.GetComponent<SpriteRenderer>().sprite = zone.factory.FactoryIcon;

		flagRenderer.sprite = ownership.owner.Flag;
    }


}
