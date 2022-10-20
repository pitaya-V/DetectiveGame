using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

[InitializeOnLoad]
public static class SceneViewContextMenu
{
    [Obsolete]
    static SceneViewContextMenu()
	{
		SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	private static void OnSceneGUI( SceneView sceneView )
	{
		var e = Event.current;

		if ( e == null ) return;
		if ( e.type != EventType.MouseUp ) return;
		if ( e.button != 1 ) return;

		e.Use();

		ShowSpriteSelectionMenu(  );
	}

	private static void ShowSpriteSelectionMenu( )
	{
		var groups = SceneViewRaycast( )
			.GroupBy( c => c.gameObject.scene.name )
			.ToArray()
			;

		var isOneScene = GetAllScenes().Count( c => c.isLoaded ) <= 1;

		var menu = new GenericMenu();

		var nameTable = new Dictionary<string, int>();

		foreach ( var group in groups )
		{
			foreach ( var n in group )
			{
				var name = n.name;
				var sceneName = n.gameObject.scene.name;
				var nameWithSceneName = sceneName + "/" + name;
				var isSameName = nameTable.ContainsKey( nameWithSceneName );
				var text = isOneScene ? name : nameWithSceneName;
				if ( isSameName )
				{
					var count = nameTable[ nameWithSceneName ]++;
					text += " [" + count.ToString() + "]";
				}
				text += GetName(n.gameObject);
				var content = new GUIContent( text );
				menu.AddItem( content, false, () => OnSelect( n ) );
				if ( !isSameName )
				{
					nameTable.Add( nameWithSceneName, 1 );
				}
			}
		}
		menu.ShowAsContext();
	}

	private static string GetName(GameObject go)
	{
		var coms = go.GetComponents<MonoBehaviour> ();
		if(coms==null || coms.Length == 0)
		{
			return "";
		}
		Type type = null;
		for (int i = 0; i < coms.Length; i++) {
			if(coms[i].GetType() == typeof(ScrollRect))
			{
				type = typeof(ScrollRect);
				break;
			}
			else if(coms[i].GetType() == typeof(Button))
			{
				type = typeof(Button);
				break;
			}
			else if(coms[i].GetType() == typeof(Canvas))
			{
				type = typeof(Canvas);
				break;
			}
			else if(coms[i].GetType() == typeof(Toggle))
			{
				type = typeof(Toggle);
				break;
			}
			else if(coms[i].GetType() == typeof(Slider))
			{
				type = typeof(Slider);
				break;
			}
			else if(coms[i].GetType() == typeof(Dropdown))
			{
				type = typeof(Dropdown);
				break;
			}
			else if(coms[i].GetType() == typeof(InputField))
			{
				type = typeof(InputField);
				break;
			}
			else if(coms[i].GetType() == typeof(Text))
			{
				type = typeof(Text);
				break;
			}
			else if(coms[i].GetType() == typeof(Image))
			{
				type = typeof(Image);
				break;
			}

		}
		if(type == null)
		{
			type = coms [0].GetType ();
		}
		return "<"+type.Name+">";
	}

	private static void OnSelect( RectTransform rectTransform )
	{
		Selection.activeTransform = rectTransform;
		EditorGUIUtility.PingObject( rectTransform );
	}

	private static IEnumerable<RectTransform> SceneViewRaycast()
	{
		return GetAllScenes()
			.Where( c => c.isLoaded )
			.SelectMany( c => c.GetRootGameObjects() )
			.Where( c => c.activeInHierarchy )
			.SelectMany( c => c.GetComponentsInChildren<RectTransform>() )
			.Where( c =>
				{
					Camera cam = SceneView.currentDrawingSceneView.camera;
					Vector3 mousepos = Event.current.mousePosition;
					mousepos.z = -cam.worldToCameraMatrix.MultiplyPoint (c.position).z;
					mousepos.y = cam.pixelHeight - mousepos.y;
					mousepos = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint (mousepos);
					var result = RectTransformUtility.RectangleContainsScreenPoint(c, mousepos);
					return result;
				} )
			;
	}

	private static IEnumerable<Scene> GetAllScenes()
	{
		for ( int i = 0; i < SceneManager.sceneCount; i++ )
		{
			yield return SceneManager.GetSceneAt( i );
		}
	}
}
