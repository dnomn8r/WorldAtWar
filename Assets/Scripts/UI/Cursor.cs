using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cursor : MonoBehaviour {

    private Zone currentlyHoveredZone = null;
    private Zone currentlySelectedZone = null;

    private Zone currentMouseDownZone = null;

    private Vector2? previousPanPosition;
    private void Update() {

        float speed = 100.0f;
        //if(!EventSystem.current.IsPointerOverGameObject()) {

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 5f;

            Vector2 pos = Camera.main.ScreenToWorldPoint(mousePosition);
          
            if (Input.GetMouseButtonDown(2)) {
                previousPanPosition = pos;
            }

            if (Input.GetMouseButton(2)) {

                Vector2 dir = (pos - previousPanPosition.Value) * Time.deltaTime * speed;

                Vector3 delta = new Vector3(dir.x, dir.y, 0);

                HUD.Instance.CameraController.AdjustTargetCameraPosition(-delta);

                previousPanPosition = pos;
        }


        //}

    }

    private void LateUpdate() {

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D[] hitColliders = Physics2D.OverlapPointAll(v);


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

           currentlyHoveredZone = hoveredZone;
        }

        if (hoveredZone != null && Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject()) {

            currentMouseDownZone = hoveredZone;
        }

        if(hoveredZone != null && hoveredZone != currentlySelectedZone && 
            Input.GetMouseButtonUp(0) &&
            !EventSystem.current.IsPointerOverGameObject()) {

            if (currentMouseDownZone == hoveredZone) {
                if (currentlySelectedZone != null) {
                    currentlySelectedZone.SetSelectedState(false);
                }

                currentlySelectedZone = hoveredZone;

                currentlySelectedZone.SetSelectedState(true);

                HUD.Instance.SetSelectedZone(currentlySelectedZone);
            } else {

                Debug.Log("doing move from: " + currentMouseDownZone.name + " to " + hoveredZone.name);
            }
        }


    }

}
