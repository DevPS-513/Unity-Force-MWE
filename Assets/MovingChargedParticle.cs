using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;



public class MovingChargedParticle : ChargedParticle
{

    public float mass = 1;
    public Rigidbody rb;



    void Start()
    {

       

        UpdateColor();
        rb.gameObject.AddComponent<Rigidbody>(); // add rigidbody to ourselves?
        rb.mass = mass;
        rb.useGravity = true;



    }

   



}
