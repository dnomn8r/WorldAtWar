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

		if(unitOwnership.unit is SeaUnit) {
			imageRenderer.transform.localScale = new Vector3(4.75f, 2.25f, 1.0f);
		} else {
            imageRenderer.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
        }


		nameField.text = unitOwnership.unit.name;

		movementField.text = unitOwnership.unit.Movement.ToString();

        firstEntryField.text = unitOwnership.unit.FirstStat.ToString();
        secondEntryField.text = unitOwnership.unit.SecondStat.ToString();

		countField.text = unitOwnership.count.ToString();
    }

}


