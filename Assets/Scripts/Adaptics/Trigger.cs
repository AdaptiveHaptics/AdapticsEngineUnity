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
    [SerializeField] AdapticsEngineMatrix adapticsEngineMatrix;

    void OnTriggerStay(Collider other)
    {
        var delta_vec = transform.position - other.transform.position;
        //Debug.Log("delta_vec: " + delta_vec);
        var size_vec = transform.localScale + other.transform.localScale;
        //Debug.Log("size_vec: " + size_vec);
        var normalized_delta = new Vector3(delta_vec.x / size_vec.x, delta_vec.y / size_vec.y, delta_vec.z / size_vec.z);
        //Debug.Log("normalized_delta: " + normalized_delta.magnitude);
        var dist = 0 + 200.0 * (1.0 - normalized_delta.magnitude);
        //Debug.Log("dist: " + dist);
        if (adapticsEngineMatrix)
        {
            adapticsEngineMatrix.UpdateUserParameter("dist", dist);
        }

    }
}
