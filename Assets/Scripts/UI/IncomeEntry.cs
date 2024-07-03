using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncomeEntry : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI nameField;
	[SerializeField] private TextMeshProUGUI incomeField;

	public void SetCountryIncome(MajorPower power, int income) {

		if (GameManager.Instance.IsCurrentTurn(power)) {
            nameField.text = "<u>" + power.name + "</u>";
        } else {
			nameField.text = power.name;
		}

		incomeField.text = income.ToString();
	}

}


