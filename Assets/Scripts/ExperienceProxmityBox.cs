using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceProxmityBox : MonoBehaviour
{

    ProximityEvExperience exp;

    private void Start()
    {
        exp = GetComponentInParent<ProximityEvExperience>();
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
