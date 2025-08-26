using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float tilt = 5f;
    [SerializeField] private float jumpStrength = 5f;

    private Vector3 playerDirection;
    private Vector3 playerRotation;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();

    }


    private void PlayerControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerDirection = Vector3.up * jumpStrength;
        }

        ApplyGravity();
    }

    private void ApplyGravity()
    {
        playerDirection.y += gravity * Time.deltaTime;
        transform.position += playerDirection * Time.deltaTime;

        playerRotation += transform.eulerAngles;
        playerRotation.z = playerDirection.y * tilt;
        transform.eulerAngles = playerRotation;
    }

}
