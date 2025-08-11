using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitMoveDisplay : MonoBehaviour {

	[SerializeField] private Image imageRenderer;

	[SerializeField] private TextMeshProUGUI nameField;

    [SerializeField] private Button undoButton;
    
	[SerializeField] private TextMeshProUGUI countField;

    private Zone currentZone;
    private Zone targetMoveZone;
    private Zone.UnitOwnershipEntry unitOwnershipEntry;


    void OnEnable() {
    
        undoButton.gameObject.SetActive(false);
        undoButton.onClick.AddListener(AttemptMove);
    }
    void OnDisable() {
        undoButton.onClick.RemoveListener(AttemptMove);
    }
    void AttemptMove() {

        Debug.Log("attempt move from: " + currentZone.name + " to: "  + targetMoveZone.name);
        //toggleVisual.gameObject.SetActive();
    }

    public void SetUnit(Zone.UnitOwnershipEntry unitOwnership, Zone currentZone, Zone targetMoveZone = null) {

        this.currentZone = currentZone;
        this.targetMoveZone = targetMoveZone;
        this.unitOwnershipEntry = unitOwnership;

        undoButton.gameObject.SetActive(targetMoveZone != null);

		imageRenderer.sprite = unitOwnership.unit.unit.UnitType.Sprite;

		if(unitOwnership.unit.unit is SeaUnit) {
			imageRenderer.transform.localScale = new Vector3(4.75f, 2.25f, 1.0f);
		} else {
            imageRenderer.transform.localScale = new Vector3(3.0f, 3.0f, 1.0f);
        }


		nameField.text = unitOwnership.unit.unit.name;

		countField.text = unitOwnership.count.ToString();
    }

}


