using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LandZoneInfoDisplay : ZoneInfoDisplay {

	[SerializeField] private SpriteRenderer flagRenderer;

	[SerializeField] private ZoneValueDisplay zoneValueDisplay;


	public override void Refresh() {

		base.Refresh();

		LandZone landZone = zone as LandZone;

		if(zone == null || landZone.CurrentOwner == null) {
			return;
		}

		zoneValueDisplay.SetValue(landZone.Value);

		if (landZone.factory != null) {
			zoneValueDisplay.GetComponent<SpriteRenderer>().sprite = landZone.factory.FactoryIcon;
		}

		flagRenderer.sprite = landZone.CurrentOwner.Flag;
    }


}
