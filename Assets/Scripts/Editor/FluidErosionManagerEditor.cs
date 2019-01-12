using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FluidErosionManager))]
public class FluidErosionManagerEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		FluidErosionManager myTarget = (FluidErosionManager) target;
		DrawDefaultInspector();
		if (GUILayout.Button("Erode") && myTarget != null)
			myTarget.RunErosion();
	}
}
