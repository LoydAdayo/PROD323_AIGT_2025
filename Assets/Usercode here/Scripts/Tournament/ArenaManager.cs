using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    // This will be populated during the tournament
    [SerializeField]
    private GameObject[] prefabsOfAgents;

    public GameObject[] spawnedAgents;

    // Contains all spawn points in the scene
    private GameObject[] spawnPoints;

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("spawn point");

        spawnedAgents = new GameObject[prefabsOfAgents.Length];
        SpawnAgents();
    }

    void Start()
    {
        InvokeRepeating("RespawnStuckAgent", 20f, 5f);
    }

    private void SpawnAgents()
    {
        prefabsOfAgents = ShuffleAgents(prefabsOfAgents);

  
        for (int index = 0; index < spawnPoints.Length; ++index)
        {
            spawnedAgents[index] = Instantiate(prefabsOfAgents[index], spawnPoints[index].transform.position, Quaternion.identity);
        }
  
    }
    
    private void RespawnStuckAgent()
    {
        foreach(GameObject agent in spawnedAgents)
        {
            if(agent.GetComponent<Rigidbody>().linearVelocity.magnitude < 1f)
            {
                int ranIndex = Random.Range(0, spawnPoints.Length);
                
                agent.transform.position = spawnPoints[ranIndex].transform.position;
            }
        }
    }

    private GameObject[] ShuffleAgents(GameObject[] agents)
    {
        System.Random rand = new System.Random();
        int length = agents.Length;

        // Shuffle: Fisher-Yates
        while(length > 1)
        {
            length--;
            int index = rand.Next(length + 1);
            GameObject temp = agents[index];
            agents[index] = agents[length];
            agents[length] = temp;
        }

        return agents;
    }

}
