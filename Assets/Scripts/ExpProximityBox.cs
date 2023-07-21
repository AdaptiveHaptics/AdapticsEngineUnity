using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpProximityBox : MonoBehaviour
{

    BaseExpWithProximity exp;
    /// OnTriggerExit doesnt get called if the hand tracking collider gets destroyed/disabled/active =false, etc. We just need to handle whatever the leap lib does when hand tracking gets lost
    HashSet<Collider> collidersInTrigger = new();

    private void Start()
    {
        exp = GetComponentInParent<BaseExpWithProximity>();
    }

    private void Update()
    {
        collidersInTrigger.RemoveWhere(c => {
            if (!c.gameObject.activeInHierarchy)
            {
                exp.OnExitProximity(c);
                return true;

            } else
            {
                return false;
            }
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        collidersInTrigger.Add(other);
        exp.OnEnterProximity(other);
    }
    private void OnTriggerExit(Collider other)
    {
        collidersInTrigger.Remove(other);
        exp.OnExitProximity(other);
    }
    private void OnTriggerStay(Collider other)
    {
        exp.OnStayProximity(other);
    }
}
