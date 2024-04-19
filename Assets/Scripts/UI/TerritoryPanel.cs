using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryPanel : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI nameField;

	[SerializeField] private LandUnitDetailEntry landUnitEntry;

	[SerializeField] private Transform unitEntryStartMount;

	private Zone currentZone;

	public void SetZone(Zone selectedZone) {

		currentZone = selectedZone;

		nameField.text = currentZone.name;

		LandZone landZone = selectedZone as LandZone;

		//LandZone.u


		//if(selectedZone is LandZone) { 
		//foreach(Unit unit in selectedZone as LandZone) {

		//}


	}

}
