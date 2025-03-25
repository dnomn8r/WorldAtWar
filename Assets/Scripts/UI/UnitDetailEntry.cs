using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDetailEntry : MonoBehaviour {

	[SerializeField] private Image imageRenderer;

	[SerializeField] private TextMeshProUGUI nameField;

    [SerializeField] private Button moveButton;
    
    [SerializeField] private GameObject moveVisualRoot;
    [SerializeField] private TextMeshProUGUI moveCountText;

    [SerializeField] protected TextMeshProUGUI firstEntryField;
	[SerializeField] protected TextMeshProUGUI secondEntryField;
	[SerializeField] private TextMeshProUGUI movementField;
	[SerializeField] private TextMeshProUGUI countField;


	void OnEnable() {
    
        moveVisualRoot.gameObject.SetActive(false);
        moveButton.onClick.AddListener(AttemptMove);
    }
    void OnDisable() {
        moveButton.onClick.RemoveListener(AttemptMove);
    }
    void AttemptMove() {

        //toggleVisual.gameObject.SetActive();
    }

    public void SetUnit(Zone.UnitOwnershipEntry unitOwnership) {

		imageRenderer.sprite = unitOwnership.unit.unit.UnitType.Sprite;

		if(unitOwnership.unit.unit is SeaUnit) {
			imageRenderer.transform.localScale = new Vector3(4.75f, 2.25f, 1.0f);
		} else {
            imageRenderer.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
        }


		nameField.text = unitOwnership.unit.unit.name;

		movementField.text = unitOwnership.unit.unit.Movement.ToString();

        firstEntryField.text = unitOwnership.unit.unit.FirstStat.ToString();
        secondEntryField.text = unitOwnership.unit.unit.SecondStat.ToString();

		countField.text = unitOwnership.count.ToString();
    }

}


