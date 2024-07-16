using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendLendLeaseEntry : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI valueField;


    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;

    private MajorPower leaser;
    private MajorPower recipient;

    public void SetPowers(MajorPower leaser, MajorPower recipient) {

        this.leaser = leaser;
        this.recipient = recipient;

        upButton.onClick.AddListener(IncreaseLendLease);
		downButton.onClick.AddListener(DecreaseLendLease);
	}

	private void UpdateLendLeaseValue() {

	}

    void IncreaseLendLease() {

        Debug.Log("INCREASE sending lend lease from: " + leaser.name  + " to " + recipient.name);
    }

	void DecreaseLendLease() {

        Debug.Log("decrease lend lease from: " + leaser.name + " to " + recipient.name);

    }
} 


