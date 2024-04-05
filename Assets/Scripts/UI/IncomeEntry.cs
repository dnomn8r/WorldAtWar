using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncomeEntry : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI nameField;
	[SerializeField] private TextMeshProUGUI incomeField;

	public void SetCountryIncome(Country country, int income) {

		nameField.text = country.name;

		incomeField.text = income.ToString();
	}

}


