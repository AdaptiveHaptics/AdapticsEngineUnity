using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpProximityBox : MonoBehaviour
{

    BaseExpWithProximity exp;

    private void Start()
    {
        exp = GetComponentInParent<BaseExpWithProximity>();
    }

    private void OnTriggerEnter(Collider other)
    {
        exp.OnEnterProximity(other);
    }
    private void OnTriggerExit(Collider other)
    {
        exp.OnExitProximity(other);
    }
    private void OnTriggerStay(Collider other)
    {
        exp.OnStayProximity(other);
    }
}
