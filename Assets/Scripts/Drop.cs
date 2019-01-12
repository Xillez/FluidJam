using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop
{
	public Vector3 position; 
	public Vector3 direction;
	public float velocity;
	public float radius;

	public Drop(Vector3 position, Vector3 direction, float velocity, float radius)
	{
		this.position = position;
		this.direction = direction;
		this.velocity = velocity;
		this.radius = radius;
	}
}
