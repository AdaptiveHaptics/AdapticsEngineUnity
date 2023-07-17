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
         /// todo: OnTriggerExit doesnt get called if the hand tracking collider gets destroyed/disabled/active =false, etc. We just need to handle whatever the leap lib does when hand tracking gets lost
        exp.OnExitProximity(other);
    }
    private void OnTriggerStay(Collider other)
    {
        exp.OnStayProximity(other);
    }
}
