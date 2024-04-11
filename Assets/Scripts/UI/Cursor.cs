using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    private Zone currentlySelectedZone = null;

    private void LateUpdate() {
        
        Ray selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);


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

        if (currentlySelectedZone != hoveredZone) {

            if (currentlySelectedZone != null) {
                currentlySelectedZone.SetHoverState(false);

                // add deSELECTION of adjacencies and hazard adjacenices
            }

            if (hoveredZone != null) {

                // add SELECTION of adjacencies and hazard adjacenices
                hoveredZone.SetHoverState(true);
                
            } else {

                Debug.Log("no zone selected");
            }
            

            currentlySelectedZone = hoveredZone;
        }
       

    }

}
