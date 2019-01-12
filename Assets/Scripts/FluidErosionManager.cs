using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FluidErosionManager : MonoBehaviour
{
	public int iterations;
	public Terrain terrain;
	List<Drop> drops = new List<Drop>();
	// ---- Drop settings
	Bounds terrainBounds;
	public float rainDensity;
	public float dropRadius;
	public float sedimentCarryAmount;

	// ---- Terrain settings
	public float sedimentWeight;
	public float sedimentRigidity;

	void Start()
	{
		if (this.terrain == null)
		{
			Debug.Log("No terrain data specified!");
			return;
		}

		if (this.rainDensity <= 0.0f)
		{
			Debug.Log("Rain density can't be 0 or under!");
			return;
		}
		
		// Get bounds of terrain
		this.terrainBounds = terrain.terrainData.bounds;

		// Calculate horizontal seperation
		Vector3 separation = new Vector3(this.terrainBounds.size.x / (this.terrainBounds.size.x / this.dropRadius), 0, this.terrainBounds.size.z / (this.terrainBounds.size.z / this.dropRadius));

		// Place all rain drops
		for (int z = 0; z < (this.terrainBounds.size.z / separation.z) + 1; ++z)
		{
			for (int x = 0; x < (this.terrainBounds.size.x / separation.x) + 1; ++x)
			{
				// Calculate horizontal position
				Vector3 pos = new Vector3(terrainBounds.min.x + (separation.x * x), 0, terrainBounds.min.z + (separation.z * z));
				// Apply the terrain hight at the current location
				pos.y = terrain.SampleHeight(pos);
				Debug.Log(pos);
				// Add new drop at position to list
				this.drops.Add(new Drop(pos, Vector3.down, 1.0f, this.dropRadius));
			}
		}
	}
	
	void Update()
	{
		
	}

	public void RunErosion()
	{
		//for (int)	
	}

	private void OnDrawGizmos()
	{
		Debug.Log(this.drops);
		/*for (int i = 0; i < this.drops.Count; ++i)
			Gizmos.DrawSphere(this.drops[i].position, this.drops[i].radius);*/
	}
}
