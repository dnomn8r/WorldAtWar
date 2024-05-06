using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDetailEntry : MonoBehaviour {

	[SerializeField] private Image imageRenderer;

	[SerializeField] private TextMeshProUGUI nameField;

		
	[SerializeField] protected TextMeshProUGUI firstEntryField;
	[SerializeField] protected TextMeshProUGUI secondEntryField;
	[SerializeField] private TextMeshProUGUI movementField;
	[SerializeField] private TextMeshProUGUI countField;

	public void SetUnit(Zone.UnitOwnershipEntry unitOwnership) {

		imageRenderer.sprite = unitOwnership.unit.UnitType.Sprite;
		nameField.text = unitOwnership.unit.name;

		movementField.text = unitOwnership.unit.Movement.ToString();

        firstEntryField.text = unitOwnership.unit.FirstStat.ToString();
        secondEntryField.text = unitOwnership.unit.SecondStat.ToString();
    }

}


