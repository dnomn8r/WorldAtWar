using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class IPCBreakdownDisplay : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI countryField;
    [SerializeField] private TextMeshProUGUI breakdownIPCField;

    public void SetCountry(MajorPower country) {

        if ((TurnPhase)GameManager.Instance.CurrentPhaseIndex == TurnPhase.COLLECT_INCOME) {

            countryField.text = country.name + ": <color=green>" + GameManager.Instance.GetCurrentTotalIncome(country) + "</color>";

            int baseIncome = WorldMapManager.Instance.GetIncome(country);

            int savedIncome = GameManager.Instance.GetSavedIPCs(country);

            int receivedLL = GameManager.Instance.GetTotalReceivingLendLease(country);
            int sentLL = GameManager.Instance.GetPendingSentLendlease(country);

            string breakdownString = baseIncome + "(Base)";
            if (savedIncome > 0) {
                breakdownString += " + " + savedIncome + "(Saved)";
            }
            if (receivedLL > 0) {
                breakdownString += " + " + receivedLL + "(LL)";
            }
            if (sentLL > 0) {
                breakdownString += " - " + sentLL + "(Sent)";
            }

            breakdownIPCField.text = breakdownString;
        } else {

            countryField.text = country.name + ": " + 
                (GameManager.Instance.GetCurrentIPCs(country) + GameManager.Instance.GetSavedIPCs(country))  + 
                (GameManager.Instance.GetLendLease(country) > 0 ? (" + " + GameManager.Instance.GetLendLease(country) + "(LL)") : "");

            breakdownIPCField.text = "";
        }
    }


}


