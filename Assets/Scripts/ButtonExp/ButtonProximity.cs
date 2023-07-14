using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonProximity : MonoBehaviour
{

    ButtonExp buttonExp;

    private void Start()
    {
        buttonExp = GetComponentInParent<ButtonExp>();
    }

    private void OnTriggerEnter(Collider other)
    {
        buttonExp.OnEnterProximity(other);
    }
    private void OnTriggerExit(Collider other)
    {
        buttonExp.OnExitProximity(other);
    }
    private void OnTriggerStay(Collider other)
    {
        buttonExp.OnStayProximity(other);
    }
}
