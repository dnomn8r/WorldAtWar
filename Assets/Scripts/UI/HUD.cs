using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

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

    [SerializeField] private AvailableIPCDisplay availableIPCDisplay;
    [SerializeField] private PhaseDisplay phaseDisplay;

    [SerializeField] private TextMeshProUGUI turnDisplay;

    [SerializeField] private Button endPhaseButton;

    public void Initialize() {

		incomeTogglePanel.gameObject.SetActive(toggleIncomeButton.isOn);

		toggleIncomeButton.onValueChanged.AddListener(ToggleIncomeGroup);

        GameManager.Instance.OnPhaseChanged += OnPhaseChanged;
        GameManager.Instance.OnTurnChanged += Instance_OnTurnChanged;
        GameManager.Instance.OnRoundChanged += Instance_OnRoundChanged;

        GameManager.Instance.OnPendingLendLeaseChanged += OnLendLeaseChanged;

        endPhaseButton.onClick.AddListener(EndPhaseEvent);

        SetSelectedZone(null);
	}

    private void Instance_OnRoundChanged() {

        turnDisplay.text = "Round " + (GameManager.Instance.currentRound + 1);
    }

    private void Instance_OnTurnChanged() {

       
    }

    private void EndPhaseEvent() {

        GameManager.Instance.EndPhase();
    }

    private void OnDisable() {
        GameManager.Instance.OnPhaseChanged -= OnPhaseChanged;
        GameManager.Instance.OnPendingLendLeaseChanged -= OnLendLeaseChanged;
    }

    private void OnLendLeaseChanged() {

        incomeTogglePanel.UpdateIncomes();
        availableIPCDisplay.UpdateDisplay();
    }

    private void OnPhaseChanged() {

        incomeTogglePanel.UpdateIncomes();
        availableIPCDisplay.UpdateDisplay();

        phaseDisplay.UpdateDisplay();


        if ((TurnPhase)GameManager.Instance.CurrentPhaseIndex == TurnPhase.COLLECT_INCOME) {

            DisplayCollectIncomePhase();

        } else if ((TurnPhase)GameManager.Instance.CurrentPhaseIndex == TurnPhase.COMBAT_ORDERS) {


        }
    }
    private void DisplayCollectIncomePhase() {


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
