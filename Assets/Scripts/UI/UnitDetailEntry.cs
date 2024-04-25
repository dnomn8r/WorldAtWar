using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDetailEntry : MonoBehaviour {

	[SerializeField] private Image imageRenderer;

	[SerializeField] private TextMeshProUGUI nameField;

		
	[SerializeField] protected TextMeshProUGUI attackStrengthField;
	[SerializeField] protected TextMeshProUGUI defenceStrengthField;
	[SerializeField] private TextMeshProUGUI movementField;
	[SerializeField] private TextMeshProUGUI countField;

	public void SetUnit(Unit unit, int count, Country owner) {

		imageRenderer.sprite = unit.UnitType.Sprite;
		nameField.text = unit.name;

		movementField.text = unit.Movement.ToString();

        //attackStrengthField.text = landUnit.Attack.ToString();
        //defenceStrengthField.text = landUnit.Defence.ToString();
    }

}


