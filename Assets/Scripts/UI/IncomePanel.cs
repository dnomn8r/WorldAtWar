using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class IncomePanel : MonoBehaviour {

	[System.Serializable]
	public struct IncomePanelEntry {

		public MajorPower majorPower;
		public IncomeEntry incomeEntry;
	}

	[SerializeField] private List<IncomePanelEntry> incomePanelEntries;

	[SerializeField] private GameObject lendLeaseSendPanel;

	private List<GameObject> existingLeasePanels = new List<GameObject>();

	public void UpdateIncomes() {

		for(int i = 0; i < existingLeasePanels.Count; ++i) {
			Destroy(existingLeasePanels[i]);
		}

		existingLeasePanels.Clear();	

        MajorPowerTurn turn = GameManager.Instance.GetCurrentlyActivePowers();

		MajorPower leaser = null;

		for(int i=0;i<turn.powers.Count;i++) {

			if (turn.powers[i].CanSendLL) {
				leaser = turn.powers[i];
				break;
			}
		}

        for (int i = 0;i<incomePanelEntries.Count;i++) {

			incomePanelEntries[i].incomeEntry.UpdateCountryIncome(leaser, incomePanelEntries[i].majorPower);

		}

	}


}
