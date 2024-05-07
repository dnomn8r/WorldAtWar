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
    [SerializeField] private TerritoryPanel territoryHoverPanel;

    public void Initialize() {

		incomeTogglePanel.gameObject.SetActive(toggleIncomeButton.isOn);

		toggleIncomeButton.onValueChanged.AddListener(ToggleIncomeGroup);

		incomeTogglePanel.UpdateIncomes();

        SetSelectedZone(null);
	}
						   
	void ToggleIncomeGroup(bool isActive) {

		incomeTogglePanel.gameObject.SetActive(toggleIncomeButton.isOn);
	}



    public void SetSelectedZone(Zone zone) {

        territoryPanel.gameObject.SetActive(zone != null);

        if (zone != null) {
            territoryPanel.SetZone(zone);
        }
    }

    public void SetHoveredZone(Zone zone) {

		territoryHoverPanel.gameObject.SetActive(zone != null);

		if (zone != null) {
			territoryHoverPanel.SetZone(zone);
		}
	}

}
