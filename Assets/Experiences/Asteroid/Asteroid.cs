using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Asteroid : MonoBehaviour
{
    public int numPoints = 10;  // Number of points on the asteroid
    public float minRadius = 0.02f;  // Minimum distance from center for a point
    public float maxRadius = 0.06f;  // Maximum distance from center for a point

    private float lastMinRadius;
    private float lastMaxRadius;
    private LineRenderer lineRenderer;

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

        GeneratePoints();
    }

    private void GeneratePoints()
    {
        lastMinRadius = minRadius;
        lastMaxRadius = maxRadius;

        Vector3[] points = new Vector3[numPoints];
        float angleStep = 360f / numPoints;

        for (int i = 0; i < numPoints; i++)
        {
            // Generate a random radius within our min/max bounds
            float radius = Random.Range(minRadius, maxRadius);

            // Convert angle and radius to Cartesian coordinates
            float angle = Mathf.Deg2Rad * i * angleStep;
            points[i] = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
        }

        lineRenderer.positionCount = numPoints;
        lineRenderer.SetPositions(points);
    }

}
