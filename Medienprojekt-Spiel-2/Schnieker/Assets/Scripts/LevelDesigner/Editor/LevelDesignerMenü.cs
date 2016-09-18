using UnityEngine;
using System.Collections;
using UnityEditor;

public class LevelDesignerMenü : Editor {
	// Sorgt dafür das man den Designer unter Creat einfügen kann.
	[MenuItem("GameObject/ Tools/ Level Designer")]
	public static void ShowLevelDesigner(){
		GameObject go = new GameObject();
		go.name = "Level Designer";
		go.AddComponent<LevelDesigner>();

		// Damit er gleich ausgewählt ist
		GameObject[] selected = new GameObject[1];
		selected[0] = go;
		Selection.objects = selected;
	}
}
