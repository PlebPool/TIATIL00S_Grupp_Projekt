using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAiScript : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsPlayer, whatIsGround, whatAmI;
    private int _playerLayer;
    private int _groundLayer;

    public Vector3 walkPoint;
    private bool _walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    private bool _alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public float fov;

    private bool _aggro = false;

    private void Start()
    {
        _playerLayer = (int)(Mathf.Log(whatIsPlayer) / Mathf.Log(2));
        _groundLayer = (int)(Mathf.Log(whatIsGround) / Mathf.Log(2));
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var t = transform;
        var pos = t.position;

        playerInSightRange = Physics.CheckSphere(pos, 5, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(pos, 1, whatIsPlayer);
        
        DrawSphere(t.position, 5, Color.red);
        
        var targetDir = player.position - t.position;
        var angle = Vector3.Angle(targetDir, t.forward);
        if (!_aggro)
        {
            var seesPlayer = false;
            if (angle < fov && playerInSightRange)
            {
                Debug.DrawLine(pos, player.position, Color.red);
                if (Physics.Raycast(pos, targetDir, out var hit, Mathf.Infinity, ~whatAmI))
                {
                    seesPlayer = hit.transform.gameObject.layer == _playerLayer;
                }
            }

            _aggro = seesPlayer;
            Debug.DrawRay(pos, t.forward * 10, Color.gray);
        }
        else
        {
            if (playerInAttackRange)
            {
                Attack();
            }
            else if (playerInSightRange)
            {
                Chase();
            }
            else
            {
                if (_aggro)
                {
                    _aggro = false;
                }
                Patrol();
            }
        }
        
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
    
    private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(16);
    private static Vector4[] MakeUnitSphere(int len)
    {
        Debug.Assert(len > 2);
        var v = new Vector4[len * 3];
        for (int i = 0; i < len; i++)
        {
            var f = i / (float)len;
            float c = Mathf.Cos(f * (float)(Math.PI * 2.0));
            float s = Mathf.Sin(f * (float)(Math.PI * 2.0));
            v[0 * len + i] = new Vector4(c, s, 0, 1);
            v[1 * len + i] = new Vector4(0, c, s, 1);
            v[2 * len + i] = new Vector4(s, 0, c, 1);
        }
        return v;
    }
    private static void DrawSphere(Vector4 pos, float radius, Color color)
    {
        Vector4[] v = s_UnitSphere;
        int len = s_UnitSphere.Length / 3;
        for (int i = 0; i < len; i++)
        {
            var sX = pos + radius * v[0 * len + i];
            var eX = pos + radius * v[0 * len + (i + 1) % len];
            var sY = pos + radius * v[1 * len + i];
            var eY = pos + radius * v[1 * len + (i + 1) % len];
            var sZ = pos + radius * v[2 * len + i];
            var eZ = pos + radius * v[2 * len + (i + 1) % len];
            Debug.DrawLine(sX, eX, color);
            Debug.DrawLine(sY, eY, color);
            Debug.DrawLine(sZ, eZ, color);
        }
    }
}
