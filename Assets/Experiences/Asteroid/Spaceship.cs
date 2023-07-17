using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public Healthbar healthbar;
    [SerializeField]
    private float damagePerAsteroid = 0.205f;

    [SerializeField]
    [Tooltip("How long the ship will rumble after being hit")]
    private float hitPeriodSeconds = 1f;

    [SerializeField]
    private float moveSpeed = 0.1f;

    public float health { get; private set; } = 1; // 0 = dead, 1 = full health

    private LineRenderer lineRenderer;
    private MeshCollider meshCollider;
    private Color defaultColor;

    private Vector3 targetLocalPosition;
    private Vector3 currVelocity;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        defaultColor = lineRenderer.material.GetColor("_EmissionColor");

        // Update the mesh collider
        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, false);
        meshCollider.sharedMesh = mesh;

        targetLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        if (isDead())
        {
            // blink red
            if (deadPulse()) lineRenderer.material.SetColor("_EmissionColor", Color.red);
            else lineRenderer.material.SetColor("_EmissionColor", Color.black);
        } else
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetLocalPosition, ref currVelocity, moveSpeed);
            if (isInHitPeriod())
            {
                if (Time.time % 0.1f < 0.05f) lineRenderer.material.SetColor("_EmissionColor", Color.clear);
                else lineRenderer.material.SetColor("_EmissionColor", defaultColor);
            } else
            {
                lineRenderer.material.SetColor("_EmissionColor", defaultColor);
            }
        }

        healthbar.SetHealth(health);
    }

    public void MoveTo(float xpos)
    {
        targetLocalPosition = new Vector3(xpos, targetLocalPosition.y, targetLocalPosition.z);
    }


    private float lastHitTime = 0;
    public bool isInHitPeriod()
    {
        return Time.time - lastHitTime < hitPeriodSeconds;
    }
    public bool isDead()
    {
        return health <= 0;
    }
    public bool deadPulse()
    {
        return Time.fixedTime % 2 < 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            //Debug.Log("Asteroid hit!");
            health = Mathf.Max(0, health - damagePerAsteroid);
            lastHitTime = Time.time;
        }
    }

    public void Reset()
    {
        health = 1;
        lineRenderer.material.SetColor("_EmissionColor", defaultColor);
    }
}
