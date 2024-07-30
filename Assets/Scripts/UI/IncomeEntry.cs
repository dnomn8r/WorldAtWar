using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncomeEntry : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI nameField;
	[SerializeField] private TextMeshProUGUI incomeField;

	[SerializeField] private GameObject savedRoot;
	[SerializeField] private GameObject lendLeaseRoot;

	[SerializeField] private TextMeshProUGUI savedField;
	[SerializeField] private TextMeshProUGUI lendLeaseField;

    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;
    [SerializeField] private TextMeshProUGUI lendLeaseValue;

    private MajorPower leaser;
    private MajorPower power;

    void Awake () {

        upButton.onClick.AddListener(IncreaseLendLease);
        downButton.onClick.AddListener(DecreaseLendLease);
    }

    void IncreaseLendLease() {

        //Debug.Log("INCREASE sending lend lease from: " + leaser.name + " to " + power.name);

        GameManager.Instance.ChangePendingLendLease(leaser, power, 1);
    }

    void DecreaseLendLease() {

        //Debug.Log("decrease lend lease from: " + leaser.name + " to " + power.name);
        GameManager.Instance.ChangePendingLendLease(leaser, power, -1);

    }

    public void UpdateCountryIncome(MajorPower leaser, MajorPower power) {

        this.leaser = leaser;
        this.power = power;

        int pendingReceivedLL = GameManager.Instance.GetPendingReceivedLendlease(power);

        lendLeaseValue.text = pendingReceivedLL.ToString();

        bool showLL = power != leaser && power.IsAlly(leaser);

        upButton.gameObject.SetActive(showLL);
        downButton.gameObject.SetActive(showLL);
        lendLeaseValue.gameObject.SetActive(showLL);

        if (GameManager.Instance.IsCurrentTurn(power)) {
            nameField.text = "<u>" + power.name + "</u>";
        } else {
            nameField.text = power.name;
        }

        int income = WorldMapManager.Instance.GetIncome(power);

        if (GameManager.Instance.IsCurrentTurn(power)) {
            incomeField.text = "<u>" + income + "</u>";
        } else {
            incomeField.text = income.ToString();
        }

        int savedIPCs = GameManager.Instance.GetSavedIPCs(power);

        savedRoot.SetActive(savedIPCs > 0);
        savedField.text = savedIPCs.ToString();

        int lendLease = GameManager.Instance.GetLendLease(power);  

        lendLeaseRoot.SetActive(lendLease > 0);
        lendLeaseField.text = lendLease.ToString();
    }

}


