using Leap.Interaction.Internal.InteractionEngineUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExp : ProximityEvExperience
{
    public GameObject handTrackingObj;
    public AdapticsPatternAsset buttonAdapticsPattern;
    public GameObject ProximityMeter;
    public GameObject ActivationMeter;
    public GameObject buttonVisible;

    [Header("Optional:")]
    public AdapticsEngineController adapticsEngineController;


    private GameObject buttonOriginalPos;

    private void Start()
    {
        if (adapticsEngineController == null) adapticsEngineController = FindObjectOfType<AdapticsEngineController>();

        buttonOriginalPos = Instantiate(buttonVisible, buttonVisible.transform.parent);
        buttonOriginalPos.name = "Button_ORIGINALPOSITION";
        MeshRenderer meshRenderer = buttonOriginalPos.GetComponent<MeshRenderer>();
        if (meshRenderer != null) meshRenderer.enabled = false;


        if (handTrackingObj == null) throw new System.Exception("handTrackingObj is null");
        if (buttonAdapticsPattern == null) throw new System.Exception("buttonPattern is null");
        if (ProximityMeter == null) throw new System.Exception("proximity_meter is null");
        if (ActivationMeter == null) throw new System.Exception("activation_meter is null");

        this.SetMeter(ProximityMeter, 0);
        this.SetMeter(ActivationMeter, 0);
    }

    override public void OnEnterProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj)
        {
            adapticsEngineController.PlayPattern(buttonAdapticsPattern);
        }
    }
    override public void OnExitProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj)
        {
            activated = false;
            buttonVisible.transform.position = buttonOriginalPos.transform.position;
            this.SetMeter(ProximityMeter, 0);
            this.SetMeter(ActivationMeter, 0);
            adapticsEngineController.StopPlayback();
        }
    }

    private bool activated = false;
    override public void OnStayProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj)
        {
            if (!activated)
            {
                var activatio_perc = 0.0;
                var proximity_perc = 0.0;

                // calculate distance from hand to button collider
                var button_orig_collider = buttonOriginalPos.GetComponent<Collider>();
                var closest_point_button = button_orig_collider.ClosestPoint(handTrackingObj.transform.position);
                var distance_button = Vector3.Distance(closest_point_button, handTrackingObj.transform.position);
                proximity_perc = 1.0 - ((distance_button - 0.005) / 0.08); // 0.01 and 0.08 are the min and max distance from hand to button collider

                if (proximity_perc > 1.0)
                {
                    // Get the button's original position
                    Vector3 buttonPosition = buttonVisible.transform.position;

                    // Get the hand's position
                    Vector3 handPosition = handTrackingObj.transform.position;

                    // Get the button's height
                    MeshRenderer buttonRenderer = buttonVisible.GetComponent<MeshRenderer>();
                    float buttonHeight = buttonRenderer.bounds.size.y;

                    var new_y_pos = handPosition.y - buttonHeight / 2;
                    // Only move the button down if the new position is below the original button position
                    if (new_y_pos < buttonOriginalPos.transform.position.y)
                    {
                        activatio_perc = (buttonOriginalPos.transform.position.y - new_y_pos) / buttonHeight;                        

                        Vector3 newPosition = new Vector3(buttonPosition.x, new_y_pos, buttonPosition.z);
                        buttonVisible.transform.position = newPosition;

                        if (activatio_perc > 1.0) activated = true;
                    }
                } else
                {
                    buttonVisible.transform.position = buttonOriginalPos.transform.position;
                }

                adapticsEngineController.UpdateUserParameter("proximity", proximity_perc);
                this.SetMeter(ProximityMeter, proximity_perc);
                var activation_pattern_formula = activatio_perc * 15 + 15;
                adapticsEngineController.UpdateUserParameter("activation", activation_pattern_formula);
                this.SetMeter(ActivationMeter, activatio_perc);
            }
        }
    }

    private void SetMeter(GameObject meter, double perc)
    {
        var perc_clamped = Math.Min(1.0, Math.Max(0.0, perc));
        meter.SetActive(perc_clamped > 0.0);
        var ls = meter.transform.localScale;
        ls.x = (float)perc_clamped;
        meter.transform.localScale = ls;
        var lp = meter.transform.localPosition;
        lp.x = (float)(0.5 - perc_clamped / 2);
        meter.transform.localPosition = lp;
    }
}
