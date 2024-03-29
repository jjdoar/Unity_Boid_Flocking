﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FlockBehavior
{
	public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
	{
		// If no neighbors, maintain current alignment
		if(context.Count == 0)
		{
			return agent.transform.forward;
		}

		// Add all points and average
		Vector3 avoidanceMove = Vector3.zero;
		int nAvoid = 0;
		foreach(Transform item in context)
		{
			if(Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SqrAvoidanceRadius)
			{
				nAvoid++;
				avoidanceMove += agent.transform.position - item.position;
			}
		}
		if(nAvoid > 0)
		{
			avoidanceMove /= nAvoid;
		}

		return avoidanceMove;
	}
}
