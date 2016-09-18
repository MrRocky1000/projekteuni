using UnityEngine;
using System.Collections;
using UnityEditor;

//Dieses Script gehört zum Script LevelDesigner
[CustomEditor(typeof(LevelDesigner))]
public class LevelDesignerEditor : Editor{

	LevelDesigner script;
	Vector2 oldTilePos = new Vector2();

	//Variablen für den Ziehmodus
	BatchMode mode = BatchMode.None;
   	bool control = false;
	enum BatchMode{
		Creat,
		Delete,
		None
	}

	void OnEnable(){
		script = (LevelDesigner)target;

		//Permanenter LevelDesigner Focus beim erstellen | wenn Game nicht läuft
		if(!Application.isPlaying){
			Tools.current = Tool.View;
		}
	}

	//Ändert die Ansicht im Inspector
	public override void OnInspectorGUI(){
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Tile");
		script.prefab = (GameObject) EditorGUILayout.ObjectField(script.prefab, typeof(GameObject),false);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Tiefe");
		script.tiefe = EditorGUILayout.Slider(script.tiefe, -5,5);
		EditorGUILayout.EndHorizontal();

		if(GUI.changed){
			EditorUtility.SetDirty(target);
		}
	}

	void OnSceneGUI(){
		//Wandelt einen Punkt in einen Strahl um
		Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
		Vector2 tilePos = new Vector2();
		tilePos.x = Mathf.RoundToInt(ray.origin.x);
		tilePos.y = Mathf.RoundToInt(ray.origin.y);

		//If Abfrage zwecks Performance
		if(tilePos != oldTilePos){
			script.gizmoPosition = tilePos;
            SceneView.RepaintAll();
            oldTilePos = tilePos;
		}

		//Events abfangen
		Event current = Event.current;
		//Ist die Steuerungstaste gedrückt
		if(current.keyCode == KeyCode.LeftControl){
			if(current.type == EventType.keyDown){
				control = true;
			}else if(current.type == EventType.keyUp){
				control = false;
				mode = BatchMode.None;
			}
		}

		if(control){
			if(current.type == EventType.mouseDown){
				if(current.button == 0){
					mode = BatchMode.Creat;
				}else if(current.button == 1){
					mode = BatchMode.Delete;
				}
			}
		}

		if(current.type == EventType.mouseDown || (mode != BatchMode.None)){
			string name = string.Format("Tile{0}_{1}_{2}", script.tiefe, tilePos.y, tilePos.x);

			//Linksklick
			if((current.button == 0) || mode == BatchMode.Creat){
				CreatTile(tilePos, name);
			}
			// Rechtsklick
			if((current.button == 1) || mode == BatchMode.Delete){
				DeleteTile(name);
			}
		}

		SetGizmoColor();

		/*Unity brauch bei Gui-Änderungen ein Dirty Flag*/
		if(GUI.changed){
			EditorUtility.SetDirty(target);
		}
	}

	void CreatTile(Vector2 tilePos, string name){
		if(!GameObject.Find(name)){
			Vector3 pos = new Vector3(tilePos.x, tilePos.y, script.tiefe);
			GameObject gameO = (GameObject)Instantiate (script.prefab, pos,Quaternion.identity);
			gameO.name = name;
		}
	}

	void DeleteTile(string name){
		GameObject gameO = GameObject.Find(name);
		if(gameO != null){
			DestroyImmediate (gameO);
		}
	}

	void SetGizmoColor(){
		switch(mode){
			case BatchMode.None:
				script.color = Color.grey;
				break;
			case BatchMode.Creat:
				script.color = Color.green;
				break;
			case BatchMode.Delete:
				script.color = Color.red;
				break;
		}
	}
}
