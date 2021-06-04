using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this script will be attached to the Boid prefab and its job is to handle the movement of each individual BOID
//because this is an object oriented program, each boid will think for itself and react to its own individual
//understanding of the world
public class Boid : MonoBehaviour
{
    public Rigidbody            rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        //set random initial position based on spawn radius
        pos = Random.insideUnitSphere * Spawner.SpawnerSingleton.spawnRadius;

        //set a random initial velocity
        Vector3 vel = Random.onUnitSphere * Spawner.SpawnerSingleton.Velocity;
        rigid.velocity = vel;

        LookAhead();

        //give the void a random color
        /* r = 0 - 1  -> 0.1231
         * g = 0 - 1  -> 0.2343
         * b = 0 - 1  -> 0.7898
         * r+g+b
         */
        Color randCol = Color.black;
        while (randCol.r+randCol.g+randCol.b<1f) 
        {
            randCol = new Color(Random.value, Random.value, Random.value);
        }
        Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rends)
        {
            rend.material.color = randCol;
        }
        TrailRenderer tRend = GetComponent<TrailRenderer>();
        tRend.material.SetColor("_TintColor", randCol);
    }

    private void LookAhead()
    {
        //orient the Boid to look at the direction it is flying
        transform.LookAt(pos+rigid.velocity);
    }

    public Vector3 pos 
    {
        get { return transform.position; }
        set { transform.position = value; }
    }
   
    private void FixedUpdate()
    {
        Vector3 Vel = rigid.velocity;
        Spawner localSpawner = Spawner.SpawnerSingleton;

        //Attraction behaviour -> go to the attractor
        Vector3 delta = Attractor.POS - pos;
        //check wether we are attracted or avoiding the atractor
        bool isAttracted = (delta.magnitude > localSpawner.attractPushDistance);
        Vector3 velAttract = delta.normalized * localSpawner.Velocity;

        //apply all the velocities
        float fdt = Time.fixedDeltaTime;

        if (isAttracted)
        {
            Vel = Vector3.Lerp(Vel, velAttract, localSpawner.attractPull * fdt);
        }
        else 
        {
            Vel = Vector3.Lerp(Vel, -velAttract, localSpawner.attractPush * fdt);
        }

        //set vel to the velocity set in the spawner singleton
        Vel = Vel.normalized * localSpawner.Velocity;
        //assign this velocity to the rigidbody
        rigid.velocity = Vel;
        //look in the direction oif the new velocity
        LookAhead();

    }

}
