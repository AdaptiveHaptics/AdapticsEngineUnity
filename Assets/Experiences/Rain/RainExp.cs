using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainExp : BaseExpWithProximity
{
    public AdapticsPatternAsset adapticsPattern;

    [Header("Optional:")]
    public AdapticsEngineController adapticsEngineController;
    public GameObject handTrackingObj;
    public ExpProximityBox expProximityBox;

    void Start()
    {
        if (adapticsEngineController == null) adapticsEngineController = FindObjectOfType<AdapticsEngineController>();
        if (handTrackingObj == null) handTrackingObj = adapticsEngineController.PatternTrackingObject;       
        if (expProximityBox == null) expProximityBox = GetComponentInChildren<ExpProximityBox>();
    }

    private void Update()
    {
        //Debug.Log(expProximityBox.transform.position);
    }


    override public void OnEnterProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj)
        {
            adapticsEngineController.PlayPattern(adapticsPattern);
        }
    }

    override public void OnExitProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj)
        {
            adapticsEngineController.StopPlayback();
        }
    }

    public override void OnStayProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj)
        {
            // get relative xz position of hand tracking object to the box, where the origin is the bottom left corner of the box
            Vector3 handTrackingObjPos = handTrackingObj.transform.position;
            Vector3 boxPos = expProximityBox.transform.position;
            Vector3 boxSize = expProximityBox.transform.lossyScale;
            var relpos = (handTrackingObjPos - (boxPos - boxSize / 2));
            relpos = new Vector3(relpos.x / boxSize.x, relpos.y / boxSize.y, relpos.z / boxSize.z);

            var droplet_size = 2 * relpos.z + 0.5;
            var rainfall_speed = 2 * relpos.x + 0.5;

            Debug.Log("droplet_size: " + droplet_size + ", rainfall_speed: " + rainfall_speed);

            adapticsEngineController.UpdateUserParameter("droplet_size", droplet_size);
            adapticsEngineController.UpdateUserParameter("rainfall_speed", rainfall_speed);
        }
    }
}
