using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryPanel : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI nameField;

	private Zone currentZone;

	public void SetZone(Zone selectedZone) {

		currentZone = selectedZone;

		nameField.text= currentZone.name;
	}

}
