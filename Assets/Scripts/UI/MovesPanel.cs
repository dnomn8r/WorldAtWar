using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Zone;

public class MovesPanel : MonoBehaviour {

	[SerializeField] private UnitDetailEntry unitEntry;

	[SerializeField] private Transform unitEntryStartMount;

	private List<UnitDetailEntry> unitEntries = new List<UnitDetailEntry>();



	public void ShowMoves(Zone selectedZone) {

		// clear previous entries
		for (int i = 0;i< unitEntries.Count; i++) {
			Destroy(unitEntries[i].gameObject);
		}

		unitEntries.Clear();


		//List<UnitInstance> allUnits = selectedZone.GetUnits();


		//Dictionary<string, UnitOwnershipEntry> unitOwnershipDictionary = new Dictionary<string, UnitOwnershipEntry>();	

		//foreach (UnitInstance unitInstance in allUnits) {

		//	string key = unitInstance.owner.name + unitInstance.unit.name;

		//	if (!unitOwnershipDictionary.ContainsKey(key)) {

		//		unitOwnershipDictionary.Add(key, new UnitOwnershipEntry(unitInstance, 1));
		//	} else {

		//		UnitOwnershipEntry entry = unitOwnershipDictionary[key];
		//		entry.count += 1;
		//		unitOwnershipDictionary[key] = entry;
		//	}
		//}

		//List<UnitOwnershipEntry> unitOwnerships = new List<UnitOwnershipEntry>(unitOwnershipDictionary.Values);

		//foreach (UnitOwnershipEntry currentOwnershipEntry in unitOwnerships) {

  //          UnitDetailEntry newEntry = Instantiate<UnitDetailEntry>(unitEntry, unitEntryStartMount);

		//	newEntry.SetUnit(currentOwnershipEntry, currentZone, targetMoveZone);

		//	unitEntries.Add(newEntry);
		//}

	}

}
