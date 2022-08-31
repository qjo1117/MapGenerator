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

		// ���� �ν����Ͱ� �����Ǿ��� autoUpdate�� �������� ��� ���� �ٽ� �����.
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
