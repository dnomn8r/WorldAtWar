using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitDetailEntry : MonoBehaviour {

	[SerializeField] private Image imageRenderer;

	[SerializeField] private TextMeshProUGUI nameField;

		
	[SerializeField] protected TextMeshProUGUI attackStrengthField;
	[SerializeField] protected TextMeshProUGUI defenceStrengthField;
	[SerializeField] private TextMeshProUGUI movementField;
	[SerializeField] private TextMeshProUGUI countField;

	public virtual void SetUnit(Unit unit, int count, MajorPower owner) {

		imageRenderer.sprite = unit.UnitType.Sprite;
		nameField.text = unit.name;

		movementField.text = unit.Movement.ToString();
	}

}


