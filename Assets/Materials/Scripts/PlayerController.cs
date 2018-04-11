using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;



public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;


    public float speed;
    private float amountToMove;



    // Use this for initialization
    void Start()
    {
     

        // INITIALIZE THE RIGIDBODY
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        amountToMove = speed * Time.deltaTime;
        
    float moveHorizontal = Input.GetAxis("Horizontal");
    float moveVertical = Input.GetAxis("Vertical");
    Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed); // Force to the bal from Input


    
    


    }

  






}
