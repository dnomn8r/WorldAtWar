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

			incomePanelEntries[i].incomeEntry.SetCountryIncome(incomePanelEntries[i].majorPower);

			if (incomePanelEntries[i].majorPower != leaser && incomePanelEntries[i].majorPower.IsAlly(leaser)) {

				GameObject newLeasePanel = GameObject.Instantiate(lendLeaseSendPanel) as GameObject;

				newLeasePanel.transform.parent = incomePanelEntries[i].incomeEntry.transform;

				newLeasePanel.transform.localPosition = new Vector3(195.0f, -1.2f, 0.0f);
				//newLeasePanel.transform.localScale = new Vector3(1.72f, 1.72f, 1.72f);
				newLeasePanel.transform.localEulerAngles = Vector3.zero;

                SendLendLeaseEntry leasePanel = newLeasePanel.GetComponent<SendLendLeaseEntry>();
				leasePanel.SetPowers(leaser, incomePanelEntries[i].majorPower);

			}
		

		}

	}


}
