using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseExpWithProximity : MonoBehaviour
{
    abstract public void OnEnterProximity(Collider other);
    abstract public void OnExitProximity(Collider other);
    abstract public void OnStayProximity(Collider other);
}
