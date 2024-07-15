using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class IPCBreakdownDisplay : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI countryField;
    [SerializeField] private TextMeshProUGUI breakdownIPCField;

    public void SetCountry(MajorPower country) {

        countryField.text = country.ToString() + ": <color=green>" + GameManager.Instance.GetCurrentTotalIncome(country) + "</color>";

        //totalIPCField.text = string.Format("{0}(Base) + {1}(Saved) + {2}(LL) - " WorldMapManager.Instance.GetIncome(country) + ""
        int baseIncome = WorldMapManager.Instance.GetIncome(country);
        int savedIncome = GameManager.Instance.GetSavedIPCs(country);  
        int receivedLL = GameManager.Instance.GetTotalReceivingLendLease(country);
        int sentLL = GameManager.Instance.GetPendingSentLendlease(country);

        string breakdownString = baseIncome + "(Base)";
        if(savedIncome > 0) {
            breakdownString += " + " + savedIncome + "(Saved)"; 
        }
        if(receivedLL > 0) {
            breakdownString += " + " + receivedLL + "(LL)";
        }
        if(sentLL > 0) {
            breakdownString += " - " + sentLL + "(Sent)";
        }

        breakdownIPCField.text = breakdownString;
    }


}


