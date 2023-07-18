using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Asteroid : MonoBehaviour
{
    public int numPoints = 10;  // Number of points on the asteroid
    public float minRadius = 0.012f;  // Minimum distance from center for a point
    public float maxRadius = 0.07f;  // Maximum distance from center for a point

    public float destroyDistance = 0.4f;  // Distance from the camera at which the asteroid is destroyed

    public float minMoveSpeed = 0.1f;  // Minimum speed at which the asteroid moves down the screen
    public float maxMoveSpeed = 0.3f;  // Maximum speed at which the asteroid moves down the screen

    private float moveSpeed = 0.05f;  // Speed at which the asteroid moves down the screen

    private float lastMinRadius;
    private float lastMaxRadius;
    private LineRenderer lineRenderer;
    private MeshCollider meshCollider;

    private void OnValidate()
    {
        if (
            lineRenderer && (
                lineRenderer.positionCount != numPoints || 
                lastMinRadius != minRadius ||
                lastMaxRadius != maxRadius
            )
        ) {
            GeneratePoints();
        }
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;

        meshCollider = GetComponent<MeshCollider>();

        GeneratePoints();
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    private void Update()
    {
        if (!Application.isPlaying) return; // don't run in edit mode

        // move down at a constant speed
        transform.localPosition += moveSpeed * Time.deltaTime * Vector3.back;
        // destroy if too far away
        if (transform.localPosition.z < -destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void GeneratePoints()
    {
        lastMinRadius = minRadius;
        lastMaxRadius = maxRadius;

        Vector3[] points = new Vector3[numPoints + 1]; // +1 for the center point (ignored by the line renderer)
        float angleStep = 360f / numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            // Generate a random radius within our min/max bounds
            float radius = Random.Range(minRadius, maxRadius);

            // Convert angle and radius to Cartesian coordinates
            float angle = Mathf.Deg2Rad * i * angleStep;
            points[i] = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
        }
        points[numPoints] = new Vector3(0, 0, 0); // center point

        lineRenderer.positionCount = numPoints;
        lineRenderer.SetPositions(points);

        // Update the mesh collider
        int[] triangles = new int[numPoints * 3];
        for (int i = 0; i < numPoints; i++)
        {
            triangles[i * 3] = numPoints;
            triangles[i * 3 + 1] = i;
            triangles[i * 3 + 2] = (i + 1) % numPoints;
        }
        Mesh mesh = new()
        {
            vertices = points,
            triangles = triangles
        };
        meshCollider.sharedMesh = mesh;
    }

}
