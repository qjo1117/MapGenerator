using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		MapGenerator generator = (MapGenerator)target;

		// 만약 인스펙터가 수정되었고 autoUpdate가 켜져있을 경우 맵을 다시 만든다.
		if(DrawDefaultInspector() == true) {
			if(generator.isUpdated == true) {
				generator.GeneratorMap();
			}
		}

		if(GUILayout.Button("Generate") == true) {
			generator.GeneratorMap();
		}
	}
}
