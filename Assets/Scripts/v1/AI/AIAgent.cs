using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float maxDistance = 5f;
    public bool updatePosition = true;
    public bool updateRotation = true;
    [HideInInspector]
    public Vector3 velocity;

    private Vector3 force;
    private List<SteeringBehaviour> behaviours;
    private NavMeshAgent nav;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        behaviours = new List<SteeringBehaviour>(GetComponents<SteeringBehaviour>());
    }
    void ComputeForces()
    {
        force = Vector3.zero;
        for (int i = 0; i < behaviours.Count; i++)
        {
            SteeringBehaviour b = behaviours[i];
            if (!b.isActiveAndEnabled)
            {
                continue;
            }
            force += b.GetForce() * b.weighting;
            if (force.magnitude > maxSpeed)
            {
                force = force.normalized * maxSpeed;
                break;
            }
        }
    }
    void ApplyVelocity()
    {
        velocity += force * Time.deltaTime;
        nav.speed = velocity.magnitude;
        if (velocity.magnitude > 0 && nav.updatePosition)
        {
            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
            Vector3 nextPos = transform.position + velocity;
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(nextPos, out navHit, maxDistance, -1))
            {
                nav.SetDestination(navHit.position);
            }
        }

    }
    void Update()
    {
        nav.updatePosition = updatePosition;
        nav.updateRotation = updateRotation;
        ComputeForces();
        ApplyVelocity();
    }
}
