using UnityEngine;

public class MovementArrow : MonoBehaviour{

    [SerializeField] private SpriteRenderer[] myRenderers;


    public void ToggleVisibility(bool visible) {

        for (int i = 0; i < myRenderers.Length; i++) {
            myRenderers[i].enabled = visible;
        }
    }

}
