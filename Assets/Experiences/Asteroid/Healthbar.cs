using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public GameObject healthbarGreen;
    public GameObject healthbarGray;

    private void Start()
    {
        Debug.Log("Healthbar start");
        SetHealth(1);
    }

    public void SetHealth(float percRaw)
    {
        var perc = Mathf.Clamp01(percRaw);
        healthbarGreen.transform.localScale = new Vector3(perc, 1, 1);
        healthbarGreen.transform.localPosition = new Vector3(-0.5f + perc / 2, 0, 0);
        healthbarGray.transform.localScale = new Vector3(1 - perc, 1, 1);
        healthbarGray.transform.localPosition = new Vector3(0.5f - (1 - perc) / 2, 0, 0);
    }
}
