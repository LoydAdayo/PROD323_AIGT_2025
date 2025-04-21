using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    // Place the virtual cameras in this order in the vCam array/inspector:
    // 0 - ArenaAerialCamera
    // 1 - ArenaCameraA
    // 2 - ArenaCameraB
    // 3 - ArenaCameraC
    // 4 - ArenaFinishZoneCamera

    [SerializeField]
    private CinemachineVirtualCamera[] vCams;

    [SerializeField]
    private CinemachineFreeLook flCam;

    // The agents array will be populated during the tournament
    [SerializeField]
    private ArenaManager am;
    private GameObject[] agents;

    private bool inFreeLook = false;

    private int index;

    #region Input Vars
    private InputAction arenaAerialCamAction;
    private InputAction arenaFreeLookCamAction;
    private InputAction arenaCamAAction;
    private InputAction arenaCamBAction;
    private InputAction arenaCamCAction;
    private InputAction arenaFinishZonCamAction;
    private InputAction randomAgentAction;
    #endregion


    private void Start()
    {
        // Get list of all spawned agents
        agents = am.GetComponent<ArenaManager>().spawnedAgents;

        // Choose a random agent for free look camera
        index = Random.Range(0, agents.Length-1);
        flCam.Follow = agents[index].transform;
        flCam.LookAt = agents[index].transform;

        // Input initialization
        arenaAerialCamAction = InputSystem.actions.FindAction("Arena Aerial Camera");
        arenaFreeLookCamAction = InputSystem.actions.FindAction("Free Look Camera");
        arenaCamAAction = InputSystem.actions.FindAction("Arena Camera A");
        arenaCamBAction = InputSystem.actions.FindAction("Arena Camera B");
        arenaCamCAction = InputSystem.actions.FindAction("Arena Camera C");
        arenaFinishZonCamAction = InputSystem.actions.FindAction("Arena Finish Zone Camera");
        randomAgentAction = InputSystem.actions.FindAction("Random Agent");
    }

    void Update()
    {
        if (arenaAerialCamAction.IsPressed()) // ArenaAerialCamera
        {
            foreach (CinemachineVirtualCamera cm in vCams)
            {
                cm.Priority = 10;
            }

            flCam.Priority = 10;
            vCams[0].Priority = 20;
            inFreeLook = false;
        }

        if (arenaCamAAction.IsPressed()) // ArenaCamera_A
        {
            foreach (CinemachineVirtualCamera cm in vCams)
            {
                cm.Priority = 10;
            }

            flCam.Priority = 10;
            vCams[1].Priority = 20;
            inFreeLook = false;
        }

        if (arenaCamBAction.IsPressed()) // ArenaCamera_B
        {
            foreach (CinemachineVirtualCamera cm in vCams)
            {
                cm.Priority = 10;
            }

            flCam.Priority = 10;
            vCams[2].Priority = 20;
            inFreeLook = false;
        }

        if (arenaCamCAction.IsPressed()) // ArenaCamera_C
        {
            foreach (CinemachineVirtualCamera cm in vCams)
            {
                cm.Priority = 10;
            }

            flCam.Priority = 10;
            vCams[3].Priority = 20;
            inFreeLook = false;
        }


        if (arenaFinishZonCamAction.IsPressed()) // ArenaFinishZoneCamera
        {
            foreach (CinemachineVirtualCamera cm in vCams)
            {
                cm.Priority = 10;
            }

            flCam.Priority = 10;
            vCams[4].Priority = 20;
            inFreeLook = false;
        }

        if (arenaFreeLookCamAction.IsPressed()) // FreeLookCamera
        {
            foreach (CinemachineVirtualCamera cm in vCams)
            {
                cm.Priority = 10;
            }

            flCam.Priority = 20;
            inFreeLook = true;
        }

        if (randomAgentAction.IsPressed() && inFreeLook) // Look at a next agent
        {
            index++;
            if(index >= agents.Length)
            {
                index = 0;
            }
            
            flCam.Follow = agents[index].transform;
            flCam.LookAt = agents[index].transform;  
        }
    }
}
