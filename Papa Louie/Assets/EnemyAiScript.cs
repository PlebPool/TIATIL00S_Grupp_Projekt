using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAiScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Vector3 walkPoint;
    private bool _walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    private bool _alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float fov;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var t = transform;
        var pos = t.position;

        var fwd = t.forward * 10;

        playerInSightRange = Physics.CheckSphere(pos, 10, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(pos, 1, whatIsPlayer);

        var targetDir = player.position - t.position;
        var angle = Vector3.Angle(targetDir, t.forward);
        var rayToPlayer = false;
        if (angle < fov && playerInSightRange)
        {
            Debug.DrawLine(pos, player.position, Color.red);
            rayToPlayer = Physics.Raycast(pos, targetDir, whatIsPlayer);
        }
        
        Debug.DrawRay(pos, t.forward * 10, Color.gray);
        
        switch (rayToPlayer)
        {
            case false when !playerInAttackRange:
                // Patrol();
                break;
            case true when !playerInAttackRange:
                // Chase();
                break;
        }

        if (rayToPlayer && playerInAttackRange) Attack();
    }

    private void Patrol()
    {
        if (!_walkPointSet) SearchWalkPoint();

        if (_walkPointSet) agent.SetDestination(walkPoint);
        var distanceToWalkPoint = transform.position - walkPoint;
        
        // Checking if walk point is reached.
        if (distanceToWalkPoint.magnitude < 1f) _walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Getting random Z and X values in walk range.
        var randomZ = Random.Range(-walkPointRange, walkPointRange);
        var randomX = Random.Range(-walkPointRange, walkPointRange);

        // Setting walk point.
        var pos = transform.position;
        walkPoint = new Vector3(pos.x + randomX, pos.y, pos.z + randomZ);
        
        // Checking if walk point is on ground.
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            _walkPointSet = true;
        
        
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        // Make sure enemy does not move
        agent.SetDestination(transform.position);
        transform.LookAt(player);
    }
}
