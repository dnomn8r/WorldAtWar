using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class AvailableIPCDisplay : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI countriesField;
	[SerializeField] private TextMeshProUGUI phaseNameField;

    private void Start() {

        GameManager.Instance.OnPhaseChanged += OnPhaseChanged;
    }

    private void OnDisable() {
        GameManager.Instance.OnPhaseChanged -= OnPhaseChanged;
    }

    private void OnPhaseChanged() {

        MajorPowerTurn turn = GameManager.Instance.GetCurrentlyActivePowers();

        string countriesText = "";

        for(int i = 0; i < turn.powers.Count; ++i) {

            countriesText += turn.powers[i].name;

            if(i < turn.powers.Count - 1) {
                countriesText += " and ";
            }
        }

        countriesField.text = countriesText;

        phaseNameField.text = GameManager.Instance.GetCurrentPhaseName();
    }
}


