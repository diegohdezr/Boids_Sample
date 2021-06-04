using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*this script will also be attached to the boid prefab, and it keeps track of which other boids are
 * nearby. key to each boids individual understanding of the world is its knowledge of which other boids
 * are close enough to worry about
*/
public class Neighborhood : MonoBehaviour
{
    [Header("Set dynamically in runtime")]
    public List<Boid>           neighbors;
    private SphereCollider      coll;

    private void Start()
    {
        neighbors = new List<Boid>();
        coll = GetComponent<SphereCollider>();
        coll.radius = Spawner.SpawnerSingleton.neighbourDistance / 2;
    }
    private void FixedUpdate()
    {
        if (coll.radius != Spawner.SpawnerSingleton.neighbourDistance / 2) 
        {
            coll.radius = Spawner.SpawnerSingleton.neighbourDistance / 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b != null) 
        {
            if (neighbors.IndexOf(b) == -1) 
            {
                //if the boid that we collided with is not a registered neighbor, make it a neighbor
                neighbors.Add(b);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Boid b = other.GetComponent<Boid>();
        if (b != null)
        {
            if (neighbors.IndexOf(b) != -1)
            {
                //if the boid that we collided with is not a registered neighbor, make it a neighbor
                neighbors.Remove(b);
            }
        }
    }

    public Vector3 avgPos
    {
        get 
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) { return avg; }
            for (int i = 0; i < neighbors.Count; i++) 
            {
                avg += neighbors[i].pos;
            }
            avg /= neighbors.Count;
            return avg;
        }
        
    }

    public Vector3 avgVel
    {
        get 
        {
            Vector3 avg = Vector3.zero;
            if (neighbors.Count == 0) return avg;
            for (int i = 0; i < neighbors.Count; i++) 
            {
                avg += neighbors[i].rigid.velocity;
            }
            avg /= neighbors.Count;
            return avg;
        }
    }

    public Vector3 avgClosePos 
    {
        get 
        {
            
            Vector3 avg = Vector3.zero;
            Vector3 delta;
            int nearCount = 0;
            for (int i = 0; i < neighbors.Count; i++)
            {
                delta = neighbors[i].pos - transform.position;
                if (delta.magnitude <= Spawner.SpawnerSingleton.CollissionDist) 
                {
                    avg += neighbors[i].pos;
                    nearCount++;
                }
            }
            if (nearCount == 0) return avg;
            avg /= nearCount;
            return avg;
        }
    }

}
