using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    private static HUD instance;
    public static HUD Instance {
        get { return instance; }
    }

    private void Awake() {
        instance = this;
    }

    [SerializeField] private Toggle toggleIncomeButton;
	[SerializeField] private IncomePanel incomeTogglePanel;
	[SerializeField] private TerritoryPanel territoryPanel;

	public void Initialize() {

		incomeTogglePanel.gameObject.SetActive(toggleIncomeButton.isOn);

		toggleIncomeButton.onValueChanged.AddListener(ToggleIncomeGroup);

		incomeTogglePanel.UpdateIncomes();
	}
						   
	void ToggleIncomeGroup(bool isActive) {

		incomeTogglePanel.gameObject.SetActive(toggleIncomeButton.isOn);
	}

	public void SetHoveredZone(Zone zone) {

		territoryPanel.gameObject.SetActive(zone != null);

		if (zone != null) {
			territoryPanel.SetZone(zone);
		}
	}

}
