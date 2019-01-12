using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
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
				//Debug.Log(pos);
				// Add new drop at position to list
				this.drops.Add(new Drop(pos, Vector3.down, 1.0f, this.dropRadius));
			}
		}
	}
	
	void Update()
	{
        RunErosion();
	}

	public void RunErosion()
	{
		//for (int i = 0; i < this.iterations; ++i)
        //{
        for (int drop = 0; drop < this.drops.Count; ++drop)
        {
            Vector2 normalizedTerrainPosition = new Vector2(this.terrainBounds.size.x / (this.drops[drop].position.x - this.terrainBounds.min.x), this.terrainBounds.size.z / (this.drops[drop].position.z - this.terrainBounds.min.z));
            this.drops[drop].velocity = terrain.terrainData.GetSteepness(normalizedTerrainPosition.x, normalizedTerrainPosition.y);
            Vector3 normalAtDropPos = terrain.terrainData.GetInterpolatedNormal(normalizedTerrainPosition.x, normalizedTerrainPosition.y);
            this.drops[drop].direction = Vector3.Cross(normalAtDropPos, -Vector3.Cross(Vector3.up, normalAtDropPos));
            //Debug.DrawLine(this.drops[drop].position, /*this.drops[drop].position +*/ this.drops[drop].direction * this.drops[drop].velocity);


           /* this.drops[drop].position += this.drops[drop].direction * this.drops[drop].velocity;// * Time.deltaTime;
            this.drops[drop].position.y = terrain.SampleHeight(this.drops[drop].position);*/
        }
        //}
	}

	private void OnDrawGizmos()
	{
        if (this.drops != null)
        {
            for (int i = 0; i < this.drops.Count / 8.0f; ++i)
            {
                //Gizmos.DrawSphere(this.drops[i].position, this.drops[i].radius);
                Debug.DrawLine(this.drops[i].position, /*this.drops[i].position + */this.drops[i].direction * this.drops[i].velocity);
            }
        }
	}
}
