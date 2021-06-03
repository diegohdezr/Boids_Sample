using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*this script will be attached to main camera. Spawner stores the fields(variables) that are
 * shared by all boids and instantances of the boids prefab
 */
public class Spawner : MonoBehaviour
{
    static public Spawner SpawnerSingleton;
    static public List<Boid> boids;

    [Header("Set in Inspector: Spawning")]
    public GameObject       boidPrefab;
    public Transform        boidAnchor;
    public int              numBoids = 100;
    public float            spawnRadius = 100f;
    public float            spawnDelay = 0.1f;

    [Header("Set in inspector: Boid Behaviour")]
    public float            Velocity = 30f;
    public float            neighbourDistance = 30f;
    public float            CollissionDist = 4f;
    public float            velMatching = 0.25f;
    public float            flockCentering = 0.2f;
    public float            collAvoid = 2f;
    public float            attractPull = 2f;
    public float            attractPush = 2f;
    public float            attractPushDistance = 5f;

    private void Awake()
    {
        //set this GO as the singleton
        SpawnerSingleton = this;
        //start the instantiation of the BOIDS
        boids = new List<Boid>();
        InstantiateBoid();
    }

    private void InstantiateBoid()
    {
        GameObject go = Instantiate(boidPrefab);
        Boid b = go.GetComponent<Boid>();
        b.transform.SetParent(boidAnchor);
        boids.Add(b);
        if (boids.Count < numBoids)
        {
            Invoke("InstantiateBoid", spawnDelay);
        }
    }
}
