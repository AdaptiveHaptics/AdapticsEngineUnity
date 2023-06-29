using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWAK : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        transform.position += movement * Time.deltaTime * speed;
    }

}
