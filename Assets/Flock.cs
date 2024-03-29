﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
	public FlockAgent agentPrefab;
	List<FlockAgent> agents = new List<FlockAgent>();
	public FlockBehavior behavior;

	[Range(10, 500)]
	public int startingCount = 250;
	const float agentDensity = 0.25f;

	[Range(1f, 100f)]
	public float driveFactor = 10f;
	[Range(1f, 100f)]
	public float maxSpeed = 5f;
	[Range(1f, 10f)]
	public float neighborRadius = 1.5f;
	[Range(0f, 1f)]
	public float avoidanceRadiusMultiplier = 0.5f;

	float sqrMaxSpeed;
	float sqrNeighborRadius;
	float sqrAvoidanceRadius;
	public float SqrAvoidanceRadius
	{
		get
		{
			return sqrAvoidanceRadius;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		sqrMaxSpeed = maxSpeed * maxSpeed;
		sqrNeighborRadius = neighborRadius * neighborRadius;
		sqrAvoidanceRadius = sqrNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

		for(int i = 0; i < startingCount; i++)
		{
			Vector3 direction = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
			FlockAgent newAgent = Instantiate(
				agentPrefab,
				Random.insideUnitSphere * startingCount * agentDensity,
				Quaternion.Euler(direction),
				transform
				);
			newAgent.name = "Agent " + i;
			agents.Add(newAgent);
		}
    }

    // Update is called once per frame
    void Update()
    {
        foreach(FlockAgent agent in agents)
		{
			List<Transform> context = GetNearbyObjects(agent);
			Vector3 move = behavior.CalculateMove(agent, context, this);
			move *= driveFactor;
			if(move.sqrMagnitude > sqrMaxSpeed)
			{
				move = move.normalized * maxSpeed;
			}
			agent.Move(move);
		}
    }

	List<Transform> GetNearbyObjects(FlockAgent agent)
	{
		List<Transform> context = new List<Transform>();
		Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
		foreach(Collider c in contextColliders)
		{
			if(c == agent.AgentCollider)
			{
				continue;
			}
			context.Add(c.transform);
		}
		return context;
	}
}
