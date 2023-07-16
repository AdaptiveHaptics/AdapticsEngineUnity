using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExp : ProximityEvExperience
{
    public Asteroid asteroidPrefab;

    [Header("Optional:")]
    public AdapticsEngineController adapticsEngineController;
    public GameObject handTrackingObj;

    void Start()
    {
        if (adapticsEngineController == null) adapticsEngineController = FindObjectOfType<AdapticsEngineController>();
        if (handTrackingObj == null) handTrackingObj = adapticsEngineController.PatternTrackingObject;
    }

    void Update()
    {

    }

    public override void OnEnterProximity(Collider other)
    {
        Debug.Log("Proximity Enter");
    }

    public override void OnExitProximity(Collider other)
    {
        Debug.Log("Proximity Exit");
    }

    public override void OnStayProximity(Collider other)
    {
        Debug.Log("Proximity Stay");
    }


}
