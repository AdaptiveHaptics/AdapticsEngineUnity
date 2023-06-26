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
        var dist = 25 + 100.0 * (1.0 - Vector3.Distance(transform.position, other.transform.position) / 0.4);
        if (adapticsEngineMatrix)
        {
            adapticsEngineMatrix.UpdateUserParameter("dist", dist);
        }

    }
}
