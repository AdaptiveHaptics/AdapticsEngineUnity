using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    void OnTriggerEnter(Collider other)
    {
        onTriggerEnter.Invoke();
    }

    [SerializeField] UnityEvent onTriggerExit;
    void OnTriggerExit(Collider other)
    {
        onTriggerExit.Invoke();
    }

    [Header("Used to update `dist` user paramter from computed value. See C# script.")]
    [SerializeField] AdapticsEngineController adapticsEngineController;

    void OnTriggerStay(Collider other)
    {
        var delta_vec = transform.position - other.transform.position;
        //Debug.Log("delta_vec: " + delta_vec);
        var size_vec = (transform.localScale + other.transform.localScale) / 2; // divide by 2 because we want "radius". delta will be from center to center but scale is from edge to edge
        //Debug.Log("size_vec: " + size_vec);
        var normalized_delta = new Vector3(Mathf.Abs(delta_vec.x / size_vec.x), Mathf.Abs(delta_vec.y / size_vec.y), Mathf.Abs(delta_vec.z / size_vec.z));

        // Clamped to avoid values heigher than one
        //var cube_distance = Mathf.Min(normalized_delta.magnitude, 1);
        // Component max (more correct)
        var cube_distance = Mathf.Max(Mathf.Max(normalized_delta.x, normalized_delta.y), normalized_delta.z);

        //Debug.Log(cube_distance);
        var dist = 0 + 200.0 * (1.0 - cube_distance);
        var intensity = (1.0 - cube_distance);
        //Debug.Log("dist: " + dist);
        if (adapticsEngineController)
        {
            adapticsEngineController.UpdateUserParameter("dist", dist);
            adapticsEngineController.UpdateUserParameter("intensity", intensity);
        }

    }
}
