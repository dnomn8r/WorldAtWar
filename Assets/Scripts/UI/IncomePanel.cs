using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomePanel : MonoBehaviour {

	[System.Serializable]
	public struct IncomePanelEntry {

		public MajorPower majorPower;
		public IncomeEntry incomeEntry;
	}

	[SerializeField] private List<IncomePanelEntry> incomePanelEntries;

	public void UpdateIncomes() {

		for(int i = 0;i<incomePanelEntries.Count;i++) {

			incomePanelEntries[i].incomeEntry.SetCountryIncome(incomePanelEntries[i].majorPower, WorldMapManager.Instance.GetIncome(incomePanelEntries[i].majorPower));

		}

	}


}
