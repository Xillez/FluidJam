using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// References:
// - https://www.firespark.de/resources/downloads/implementation%20of%20a%20methode%20for%20hydraulic%20erosion.pdf

//[ExecuteInEditMode]
public class FluidErosionManager : MonoBehaviour
{
	//public bool drawDrops;
	public int iterations;
	public Terrain terrain;
	List<Drop> drops = new List<Drop>();
	Vector2Int nrDrops;
	Vector3 dropSeparation;
	// ---- Drop settings
	Bounds terrainBounds;
	[Range(0.0f, 2.0f)]
	//public float rainDensity;
	public float dropRadius;
	//[Range(0.0f, 1.0f)]
	//public float sedimentCarryAmount;
	[Range(0.0f, 1.0f)]
	public float evaporationSpeed;

	// ---- Terrain settings
	//public float sedimentWeight;
	//public float sedimentRigidity;

	void Start()
	{
		if (this.terrain == null)
		{
			Debug.Log("No terrain data specified!");
			return;
		}

		/*if (this.rainDensity <= 0.0f)
		{
			Debug.Log("Rain density can't be 0 or under!");
			return;
		}*/
		
		// Get bounds of terrain
		this.terrainBounds = terrain.terrainData.bounds;

		this.nrDrops = new Vector2Int((int) (this.terrainBounds.size.x / this.dropRadius), (int) (this.terrainBounds.size.z / this.dropRadius));

		// Calculate horizontal seperation
		this.dropSeparation = new Vector3(this.terrainBounds.size.x / this.nrDrops.x, 0, this.terrainBounds.size.z / this.nrDrops.y);

		// Place all rain drops
		for (int z = 0; z < 30; ++z)
		{
			for (int x = 0; x < 30; ++x)
			{
				// Calculate horizontal position
				Vector3 pos = new Vector3(terrainBounds.min.x + (dropSeparation.x * x), 0, terrainBounds.min.z + (dropSeparation.z * z));
				// Apply the terrain hight at the current location
				pos.y = terrain.SampleHeight(pos);

				Drop drop = new Drop(pos, Vector3.down, 1.0f, this.dropRadius);
				drop.lifetime = Random.Range(5.0f, 10.0f);
				drop.currentLifetime = 0.0f;
				// Add new drop at position to list
				this.drops.Add(drop);
			}
		}
	}
	
	void Update()
	{
        //RunErosion();
	}

	public void RunErosion()
	{
		for (int i = 0; i < this.iterations; ++i)
        {
			for (int drop = 0; drop < this.drops.Count; ++drop)
			{
				// Skip if evaporated
				if (this.drops[drop].radius <= 0.0f)
					continue;

				Vector2Int heightmapRes = new Vector2Int(this.terrain.terrainData.heightmapWidth, this.terrain.terrainData.heightmapHeight);

				// Calc drop position in relation to terrain length
				Vector2 normalizedTerrainPosition = new Vector2((this.drops[drop].position.x - this.terrainBounds.min.x) / this.terrainBounds.size.x, (this.drops[drop].position.z - this.terrainBounds.min.z) / this.terrainBounds.size.z);

				// ---- Erosion ----
				Vector2Int dropIndex = new Vector2Int((int) (normalizedTerrainPosition.x * heightmapRes.x), (int)(normalizedTerrainPosition.y * heightmapRes.y));

				if (dropIndex.x > 0 && dropIndex.x < heightmapRes.x && dropIndex.y > 0 && dropIndex.y < heightmapRes.y)
				{
					float[,] heights = this.terrain.terrainData.GetHeights(dropIndex.x, dropIndex.y, 1, 1);

					/*for(int y = 0; y < 3; ++y)
					{
						for (int x = 0; x < 3; ++x)
						{
							heights[]
						}
					}*/

					heights[0, 0] -= 0.0005f;

					this.terrain.terrainData.SetHeights(dropIndex.x, dropIndex.y, heights);
				}


				// ---- Drop updation ----

				// Add steepness to velocity for 1/2 time step
				this.drops[drop].velocity += terrain.terrainData.GetSteepness(normalizedTerrainPosition.x, normalizedTerrainPosition.y) * Time.deltaTime * 0.5f;

				// Get normal of terrain at current position
				Vector3 normalAtDropPos = terrain.terrainData.GetInterpolatedNormal(normalizedTerrainPosition.x, normalizedTerrainPosition.y);

				// Get "downhill" direction (Gradient decent)
				this.drops[drop].direction = Vector3.Cross(normalAtDropPos, -Vector3.Cross(Vector3.up, normalAtDropPos));

				// Update position with velocity and correct height to "hug" terrain
				this.drops[drop].position += this.drops[drop].direction * this.drops[drop].velocity * Time.deltaTime;
				this.drops[drop].position.y = terrain.SampleHeight(this.drops[drop].position);

				// Apply drag
				this.drops[drop].velocity *= 0.9f;

				// Update lifetime and evaporation amount on radius
				this.drops[drop].currentLifetime -= Time.deltaTime;
				this.drops[drop].radius -= this.evaporationSpeed * Time.deltaTime;
			}
        }
	}

	private void OnDrawGizmos()
	{
        /*if (this.drops != null)
        {
            for (int i = 0; i < this.drops.Count; ++i)
            {
				Gizmos.DrawSphere(this.drops[i].position, this.drops[i].radius);
			}
		}*/
	}
}
