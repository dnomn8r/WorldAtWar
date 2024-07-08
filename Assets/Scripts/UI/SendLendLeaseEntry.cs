using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendLendLeaseEntry : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI valueField;


    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;

    private MajorPower majorPower;
    public void SetMajorPower(MajorPower power) {

        this.majorPower = power;

        upButton.onClick.AddListener(IncreaseLendLease);
		downButton.onClick.AddListener(IncreaseLendLease);
	}

	private void UpdateLendLeaseValue() {

	}

    void IncreaseLendLease() {

    }

	void DecreaseLendLease() {

	}
} 


