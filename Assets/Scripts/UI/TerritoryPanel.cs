using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Zone;

public class TerritoryPanel : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI nameField;

	[SerializeField] private UnitDetailEntry unitEntry;

	[SerializeField] private Transform unitEntryStartMount;

	private Zone currentZone;

	public void SetZone(Zone selectedZone) {

		currentZone = selectedZone;

		nameField.text = currentZone.name;

		List<UnitOwnershipEntry> units = selectedZone.GetUnits();

		Debug.Log("selected zone: " + currentZone.name);

		foreach(UnitOwnershipEntry unitEntry in units) {

			Debug.Log("unit: " + unitEntry.unit.name + "x" + unitEntry.count + " owned by: " + unitEntry.owner.name);
		}

	}

}
