using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	[SerializeField] private Toggle toggleIncomeButton;
	[SerializeField] private GameObject incomeTogglePanel;

	void Start() {

		toggleIncomeButton.onValueChanged.AddListener(ToggleIncomeGroup);
	}
						   
	void ToggleIncomeGroup(bool isActive) {

		Debug.Log("You have clicked the button!");

		incomeTogglePanel.SetActive(isActive);
	}

}
