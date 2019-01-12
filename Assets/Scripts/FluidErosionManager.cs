using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidErosionManager : MonoBehaviour
{
	public TerrainData terrain;
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
		this.terrainBounds = terrain.bounds;

		// Calculate horizontal seperation
		Vector3 separation = new Vector3(this.terrainBounds.size.x / (this.dropRadius / this.rainDensity), 0, this.terrainBounds.size.z / (this.dropRadius / this.rainDensity));

		// Place all rain drops
		for (int z = 0; z < (this.terrainBounds.size.z / separation.z) + 1; ++z)
		{
			for (int x = 0; x < (this.terrainBounds.size.x / separation.x) + 1; ++x)
			{
				this.drops.Add(new Drop(
						new Vector3(terrainBounds.min.x + (separation.x * x), 0, terrainBounds.min.z + (separation.z * z)), Vector3.down, 1.0f, this.dropRadius
					)
				);
			}
		}
	}
	
	void Update()
	{
		//
	}

	
}
