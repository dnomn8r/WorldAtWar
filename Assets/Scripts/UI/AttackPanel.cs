using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Zone;

public class AttackPanel : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI attackedTerritoryText;

	[SerializeField] private UnitDetailEntry unitEntry;

    [SerializeField] private TextMeshProUGUI attackerStrengthText;
    [SerializeField] private TextMeshProUGUI attackerUnitCountText;

    [SerializeField] private TextMeshProUGUI defenderStrengthText;
    [SerializeField] private TextMeshProUGUI defenderUnitCountText;

    [SerializeField] private Transform attackerUnitsEntryStartMount;

	[SerializeField] private Transform defenderUnitsEntryStartMount;

	private Zone attackedZone;

    private List<UnitDetailEntry> attackingUnitEntries = new List<UnitDetailEntry>();
    private List<UnitDetailEntry> defendingUnitEntries = new List<UnitDetailEntry>();

    List<UnitInstance> attackers = new List<UnitInstance>();

	public void CreateAttack(Zone zone) {

		attackedZone = zone;

		attackedTerritoryText.text = attackedZone.name;

		List<UnitInstance> defendingUnits = attackedZone.GetUnits();

        int totalDefenceStrength = 0;

        for(int i = 0; i < defendingUnits.Count; i++) {

            //totalDefenceStrength += defendingUnits[i].unit.
        }

        defenderUnitCountText.text = defendingUnits.Count.ToString();

        DisplayUnits(defendingUnits, defendingUnitEntries, defenderUnitsEntryStartMount);

	}

    public void AddAttacker(UnitInstance unit) {

        if (!attackers.Contains(unit)){
            attackers.Add(unit);
        } else {
            Debug.LogError("we should never be trying to add the same attacker twice! " + unit.unit.name);
        }

        attackerUnitCountText.text = attackers.Count.ToString();

        DisplayUnits(attackers, attackingUnitEntries, attackerUnitsEntryStartMount);
    }

    public void RemoveAttacker(UnitInstance unit) {

        if (!attackers.Contains(unit)) {
            Debug.LogError("we should never be trying to remove a non-added attacker " + unit.unit.name);
        } else {
            attackers.Remove(unit);
        }

        attackerUnitCountText.text = attackers.Count.ToString();

        DisplayUnits(attackers, attackingUnitEntries, attackerUnitsEntryStartMount);
    }

    private void DisplayUnits(List<UnitInstance> units, List<UnitDetailEntry> existingEntries, Transform mount) {

        // clear previous entries
        for (int i = 0; i < existingEntries.Count; i++) {
            Destroy(existingEntries[i].gameObject);
        }

        existingEntries.Clear();

        float currentOffset = 0.0f;
        float entrySize = 40.0f;

        Dictionary<string, UnitOwnershipEntry> unitOwnershipDictionary = new Dictionary<string, UnitOwnershipEntry>();

        foreach (UnitInstance unitInstance in units) {

            string key = unitInstance.owner.name + unitInstance.unit.name;

            if (!unitOwnershipDictionary.ContainsKey(key)) {

                unitOwnershipDictionary.Add(key, new UnitOwnershipEntry(unitInstance, 1));
            } else {

                UnitOwnershipEntry entry = unitOwnershipDictionary[key];
                entry.count += 1;
                unitOwnershipDictionary[key] = entry;
            }
        }

        foreach (UnitOwnershipEntry currentOwnershipEntry in new List<UnitOwnershipEntry>(unitOwnershipDictionary.Values)) {

            UnitDetailEntry newEntry = Instantiate<UnitDetailEntry>(unitEntry, mount);

            newEntry.SetUnit(currentOwnershipEntry, null);

            defendingUnitEntries.Add(newEntry);

            currentOffset += entrySize;

            //Debug.Log("unit: " + currentOwnershipEntry.unit.name + " x" + currentOwnershipEntry.count + " owned by: " + currentOwnershipEntry.owner.name);
        }
    }

}
