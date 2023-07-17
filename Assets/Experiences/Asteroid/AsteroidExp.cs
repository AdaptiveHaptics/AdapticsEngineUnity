using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExp : BaseExpWithProximity
{
    public AdapticsPatternAsset adapticsPattern;
    public Asteroid asteroidPrefab;
    public float minAsteroidSpawnInterval = 1f;
    public float maxAsteroidSpawnInterval = 2f;

    [Header("Optional:")]
    public AdapticsEngineController adapticsEngineController;
    public GameObject handTrackingObj;

    public Spaceship spaceship;

    void Start()
    {
        if (adapticsEngineController == null) adapticsEngineController = FindObjectOfType<AdapticsEngineController>();
        if (handTrackingObj == null) handTrackingObj = adapticsEngineController.PatternTrackingObject;
        
        if (spaceship == null) spaceship = FindObjectOfType<Spaceship>();
    }

    void Update()
    {

    }

    public override void OnEnterProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj)
        {
            spaceship.Reset();
            adapticsEngineController.PlayPattern(adapticsPattern);
        }
    }

    public override void OnExitProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj)
        {
            adapticsEngineController.StopPlayback();
        }
    }


    // This is to show the user params in the inspector's debug view
    private double heartrate;
    private double rumble;
    private double dead;
    private double deadpulse;

    private float nextAsteroidSpawnTime = 0;
    public override void OnStayProximity(Collider other)
    {
        if (other.gameObject == handTrackingObj) {

            if (Time.time > nextAsteroidSpawnTime) {
                nextAsteroidSpawnTime = Time.time + Random.Range(minAsteroidSpawnInterval, maxAsteroidSpawnInterval);
                Asteroid asteroid = Instantiate(asteroidPrefab, transform);
                //spawn randomly in a box {-0.2, 0, 0.3} to {0.2, 0, 0.4}
                asteroid.transform.localPosition = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(0.3f, 0.4f));
            }

            Vector3 handRelative = transform.InverseTransformPoint(other.transform.position);
            spaceship.MoveTo(handRelative.x);

            heartrate = (1 - spaceship.Health) + 1;
            adapticsEngineController.UpdateUserParameter("heartrate", heartrate);

            rumble = spaceship.IsInHitPeriod() ? 25 : 0;
            adapticsEngineController.UpdateUserParameter("rumble", rumble);

            dead = spaceship.IsDead() ? 1 : 0;
            adapticsEngineController.UpdateUserParameter("dead", dead);

            deadpulse = spaceship.DeadPulse() ? 1 : 0;
            adapticsEngineController.UpdateUserParameter("deadpulse", deadpulse);

        }
    }


}
