using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Zone;

public class TerritoryPanel : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI nameField;

	[SerializeField] private UnitDetailEntry unitEntry;

	[SerializeField] private Transform unitEntryStartMount;

	private Zone currentZone;

	private List<UnitDetailEntry> unitEntries = new List<UnitDetailEntry>();

	public void SetZone(Zone selectedZone) {

		// clear previous entries
		for (int i = 0;i< unitEntries.Count; i++) {
			Destroy(unitEntries[i].gameObject);
		}

		unitEntries.Clear();

		currentZone = selectedZone;

		nameField.text = currentZone.name;

		List<UnitOwnershipEntry> unitOwnerships = selectedZone.GetUnits();

		//Debug.Log("selected zone: " + currentZone.name);

		float currentOffset = 0.0f;
		float entrySize = 40.0f;

		foreach(UnitOwnershipEntry currentOwnershipEntry in unitOwnerships) {

            UnitDetailEntry newEntry = Instantiate<UnitDetailEntry>(unitEntry, unitEntryStartMount);
			//newEntry.transform.localPosition = new Vector3(0, -currentOffset, 0);

			newEntry.SetUnit(currentOwnershipEntry);

			unitEntries.Add(newEntry);

			currentOffset += entrySize;

			//Debug.Log("unit: " + currentOwnershipEntry.unit.name + " x" + currentOwnershipEntry.count + " owned by: " + currentOwnershipEntry.owner.name);
		}

	}

}
