using UnityEngine;
using UnityEngine.AI;

public class RatAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private PlayerStats playerStats;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        player = playerStats.transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }
}
