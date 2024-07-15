using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;


public class AvailableIPCDisplay : MonoBehaviour {

    [SerializeField] private IPCBreakdownDisplay breakdownDisplaySingle;

    [SerializeField] private IPCBreakdownDisplay[] breakdownDisplayDouble;

    [SerializeField] private GameObject singleRoot;
    [SerializeField] private GameObject doubleRoot;

    public void UpdateDisplay() {

        MajorPowerTurn turn = GameManager.Instance.GetCurrentlyActivePowers();

        singleRoot.SetActive(turn.powers.Count == 1);
        doubleRoot.SetActive(turn.powers.Count == 2);

        if(turn.powers.Count == 1) {
            breakdownDisplaySingle.SetCountry(turn.powers[0]);
        }

        if(turn.powers.Count == 2) {

            for (int i = 0; i < breakdownDisplayDouble.Length; ++i) {

                breakdownDisplayDouble[i].SetCountry(turn.powers[i]);
            }
        }
    }
}


