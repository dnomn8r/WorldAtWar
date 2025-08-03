using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static GameManager;

public class Cursor : MonoBehaviour {

    private Zone currentlyHoveredZone = null;
    private Zone currentlySelectedZone = null;


    private Vector2? previousPanPosition;
    private void Update() {

        float speed = 100.0f;

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

    }

    private class PotentialMoves {

        private Zone startingZone;
        private Dictionary<Zone, List<UnitInstance>> possibleMoves;

        public PotentialMoves(Zone startingZone) {
        
            this.startingZone = startingZone;
            possibleMoves = new Dictionary<Zone, List<UnitInstance>>();
        }

        public void AddPotentialMoves(UnitInstance unit, List<Zone> reachableZones) {

            foreach(Zone zone in reachableZones) {
                if (!possibleMoves.ContainsKey(zone)) {
                    possibleMoves.Add(zone, new List<UnitInstance>());
                }
                possibleMoves[zone].Add(unit);
            }
        }

        public bool IsZonePossible(Zone potentialZone) {
            return possibleMoves.ContainsKey(potentialZone);
        }

        public void HighlightPotentialZones(bool toggle) {

            foreach(Zone zone in possibleMoves.Keys) {
                if (zone != startingZone) {
                    zone.SetHoverState(toggle);
                }
            }
        }
    }

    private PotentialMoves potentialNonCombatMove;
    private float? startPressTime = null;
    private const float MOVE_PRESS_DELAY = 0.25f;

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

            if (currentlySelectedZone != null) {
                currentlySelectedZone.SetSelectedState(false);
            }

            currentlySelectedZone = hoveredZone;

            currentlySelectedZone.SetSelectedState(true);

            HUD.Instance.SetSelectedZone(currentlySelectedZone);

            startPressTime = Time.time;            

        }

        // if held
        if (Input.GetMouseButton(0)) {

            if ((TurnPhase)GameManager.Instance.CurrentPhaseIndex == TurnPhase.NON_COMBAT_MOVEMENT) {

                if ((Time.time > startPressTime + MOVE_PRESS_DELAY) && potentialNonCombatMove == null) {

                    potentialNonCombatMove = new PotentialMoves(currentlySelectedZone);

                    foreach (UnitInstance unit in currentlySelectedZone.GetUnits()) {

                        MajorPower majorPowerUnitOwner = unit.owner as MajorPower;
                        if(majorPowerUnitOwner == null) { continue; } // we don't get to move minor powers

                        // only allow movement by currently active powers units
                        if (GameManager.Instance.GetCurrentlyActivePowers().powers.Contains(majorPowerUnitOwner)) {

                            List<Zone> zonesInRange = new List<Zone>();

                            WorldMapManager.Instance.GetZonesWithinRangeNonCombat(currentlySelectedZone,
                                unit, unit.moveRemaining, ref zonesInRange);

                            potentialNonCombatMove.AddPotentialMoves(unit, zonesInRange);
                        }
                    }

                    potentialNonCombatMove.HighlightPotentialZones(true);
                }
            }
        }


        if (Input.GetMouseButtonUp(0)) {

            if (potentialNonCombatMove != null) {

                if (hoveredZone != null && hoveredZone != currentlySelectedZone &&
                    !EventSystem.current.IsPointerOverGameObject()) {

                    if (potentialNonCombatMove.IsZonePossible(hoveredZone)) {

                        Debug.Log("actual move to " + hoveredZone.name);
                    }
                }

                potentialNonCombatMove.HighlightPotentialZones(false);

                potentialNonCombatMove = null;
            }

            startPressTime = null;
        }


    }

}
