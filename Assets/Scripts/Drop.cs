using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop
{
	public Vector3 position; 
	public Vector3 direction;
	public float velocity;
	public float radius;
	public float mass;
	public float lifetime;
	public float currentLifetime;

	public Drop(Vector3 position, Vector3 direction, float velocity = 1.0f, float radius = 0.1f, float mass = 0.1f)
	{
		this.position = position;
		this.direction = direction;
		this.velocity = velocity;
		this.radius = radius;
	}
}
