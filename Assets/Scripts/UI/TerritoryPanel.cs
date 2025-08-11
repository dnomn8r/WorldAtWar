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

	private Zone targetMoveZone;

	public void SetZone(Zone selectedZone, Zone targetMoveZone = null) {

		this.targetMoveZone = targetMoveZone;

		// clear previous entries
		for (int i = 0;i< unitEntries.Count; i++) {
			Destroy(unitEntries[i].gameObject);
		}

		unitEntries.Clear();

		currentZone = selectedZone;

		nameField.text = currentZone.name;


		List<UnitInstance> allUnits = selectedZone.GetUnits();


		Dictionary<string, UnitOwnershipEntry> unitOwnershipDictionary = new Dictionary<string, UnitOwnershipEntry>();	

		foreach (UnitInstance unitInstance in allUnits) {

			string key = unitInstance.owner.name + unitInstance.unit.name;

			if (!unitOwnershipDictionary.ContainsKey(key)) {

				unitOwnershipDictionary.Add(key, new UnitOwnershipEntry(unitInstance, 1));
			} else {

				UnitOwnershipEntry entry = unitOwnershipDictionary[key];
				entry.count += 1;
				unitOwnershipDictionary[key] = entry;
			}
		}

		List<UnitOwnershipEntry> unitOwnerships = new List<UnitOwnershipEntry>(unitOwnershipDictionary.Values);

		foreach (UnitOwnershipEntry currentOwnershipEntry in unitOwnerships) {

            UnitDetailEntry newEntry = Instantiate<UnitDetailEntry>(unitEntry, unitEntryStartMount);

			newEntry.SetUnit(currentOwnershipEntry, currentZone, targetMoveZone);

			unitEntries.Add(newEntry);

			//Debug.Log("unit: " + currentOwnershipEntry.unit.unit.name + " x" + currentOwnershipEntry.count + " owned by: " + currentOwnershipEntry.unit.owner.name);
		}

	}

}
