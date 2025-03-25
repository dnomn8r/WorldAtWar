using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour {

    private Zone currentlyHoveredZone = null;
    private Zone currentlySelectedZone = null;

    private void LateUpdate() {

        //if (EventSystem.current.IsPointerOverGameObject()) {
        //    currentlyHoveredZone = null;
        //    HUD.Instance.SetHoveredZone(currentlyHoveredZone);
        //    return;
        //}

        //Ray selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);


        //int hitCount = Physics.RaycastNonAlloc(selectionRay, hits, 10000);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(v);

        //Debug.DrawRay(selectionRay.origin, selectionRay.direction * 10000, Color.red);

        Zone hoveredZone = null;
        int highestPriority = -10000;

        for (int i = 0; i < hitColliders.Length; i++) {

            Zone zone = hitColliders[i].transform.GetComponentInParent<Zone>();
            
            if(zone != null) {

                SpriteRenderer spriteRenderer = hitColliders[i].transform.GetComponent<SpriteRenderer>();

                if (spriteRenderer != null) {

                    if(highestPriority < spriteRenderer.sortingOrder) {
                        highestPriority = spriteRenderer.sortingOrder;

                        hoveredZone = zone;
                    }
                }

            }

        }

        if (currentlyHoveredZone != hoveredZone) {

            //if (currentlyHoveredZone != null) {
            //    currentlyHoveredZone.SetHoverState(false);
            //}

            //if (hoveredZone != null) {

                //hoveredZone.SetHoverState(true);

                //HUD.Instance.SetHoveredZone(hoveredZone);
                
            //} else {

                //Debug.Log("no zone selected");
            //}
            

            currentlyHoveredZone = hoveredZone;
        }

        if(hoveredZone != null && hoveredZone != currentlySelectedZone && 
            Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject()) {

            if(currentlySelectedZone != null) {
                currentlySelectedZone.SetSelectedState(false);
            }

            currentlySelectedZone = hoveredZone;

            currentlySelectedZone.SetSelectedState(true);

            HUD.Instance.SetSelectedZone(currentlySelectedZone);

        }


    }

}
