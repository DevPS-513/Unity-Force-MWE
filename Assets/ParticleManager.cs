using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ParticleManager : MonoBehaviour
{

    private float cycleInterval = 0.01f;

    private List<ChargedParticle> chargedParticles;
    private List<MovingChargedParticle> movingChargedParticles;
    private List<PlayerController> player;

    public float x_force = 0;
    public float z_force = 0;

    public float x_normalizer = 1.3f;
    public float z_normalizer = 1.3f;


    public SerialPort joyp = new SerialPort("\\\\.\\COM14", 9600);

    public float max_PWM_x_force = 133f;

    public float max_PWM_z_force = 133f;

    public float mag_force = 733;

    public int collision_flag = 0;

    void Start()
    {
        max_PWM_z_force = max_PWM_z_force * 0.7f;

        joyp.Open();
        joyp.ReadTimeout = 1; // Something to help Unity not glitch on the timing

        joyp.Write("wake");

        chargedParticles = new List<ChargedParticle>(FindObjectsOfType<ChargedParticle>());
        movingChargedParticles = new List<MovingChargedParticle>(FindObjectsOfType<MovingChargedParticle>());

        player = new List<PlayerController>(FindObjectsOfType<PlayerController>());

        foreach (MovingChargedParticle mcp in movingChargedParticles)
        {
            StartCoroutine(Cycle(mcp));
        }

    }

    public void UpdateColor()
    {


    }

    public IEnumerator Cycle(MovingChargedParticle mcp)
    {
        while (true)
        {
            ApplyMagneticForce(mcp);
            yield return new WaitForSeconds(cycleInterval);
        }
    }

    private void ApplyMagneticForce(MovingChargedParticle mcp)
    {
        Vector3 newForce = Vector3.zero;

        foreach (ChargedParticle cp in chargedParticles)
        {
            if (mcp == cp)
                continue;

            float distance = Vector3.Distance(mcp.transform.position, cp.gameObject.transform.position);
            float force = mag_force * mcp.charge * cp.charge / Mathf.Pow(distance, 2);

            if (distance<0.45)
            {
                max_PWM_x_force = 0;
                max_PWM_z_force = 0;

                collision_flag = 1;

                print("COLLISION");
            }
         

            Vector3 direction = mcp.transform.position - cp.transform.position;
            direction.Normalize();

           

            newForce += force * direction * cycleInterval;

            if (float.IsNaN(newForce.x))
                newForce = Vector3.zero;

            mcp.rb.AddForce(newForce);
            //print(newForce);

            x_force = -1 * newForce.x / x_normalizer * max_PWM_x_force;

            z_force = newForce.z / z_normalizer * max_PWM_z_force;
            //print("drive_x " + (int) x_force);
            //    print("\n|");
            // print("drive_x " +(int) z_force);

            if (collision_flag == 0)
            {
                joyp.Write("drive_x " + (int)x_force);
                joyp.Write("drive_y " + (int)z_force);
            }

            if (collision_flag == 1)
            {
                joyp.Write("drive_x 0" );
                joyp.Write("drive_y 0");
            }



        }
    }

    void OnApplicationQuit()
    { joyp.Write("sleep"); }




}