using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWAK : MonoBehaviour
{
    public float speed = 2f;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);

        transform.position += movement * Time.deltaTime * speed;
    }

}
