using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncomeEntry : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI nameField;
	[SerializeField] private TextMeshProUGUI incomeField;

	[SerializeField] private GameObject savedRoot;
	[SerializeField] private GameObject lendLeaseRoot;

	[SerializeField] private TextMeshProUGUI savedField;
	[SerializeField] private TextMeshProUGUI lendLeaseField;

	[SerializeField]
    public void SetCountryIncome(MajorPower power) {

        if (GameManager.Instance.IsCurrentTurn(power)) {
            nameField.text = "<u>" + power.name + "</u>";
        } else {
            nameField.text = power.name;
        }

        int income = WorldMapManager.Instance.GetIncome(power);

        if (GameManager.Instance.IsCurrentTurn(power)) {
            incomeField.text = "<u>" + income + "</u>";
        } else {
            incomeField.text = income.ToString();
        }

        int savedIPCs = GameManager.Instance.GetSavedIPCs(power);

        savedRoot.SetActive(savedIPCs > 0);
        savedField.text = savedIPCs.ToString();

        int lendLease = GameManager.Instance.GetLendLease(power);  

        lendLeaseRoot.SetActive(lendLease > 0);
        lendLeaseField.text = lendLease.ToString();
    }

}


