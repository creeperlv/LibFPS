
using LibFPS.AnimationSystem;
using UnityEditor;
using UnityEngine;

namespace LibFPS.Editor.AnimationSystem
{
	public class TransformSyncBaker : EditorWindow
	{
		[MenuItem("LibFPS/AnimationSystem/Transform Sync Baker")]
		public static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(TransformSyncBaker));
		}
		TransformSync SYNC;
		string msg = "";
		public void OnGUI()
		{
			GUILayout.Label("<color=#2288EE>AnimationSystem</color>", new GUIStyle() { fontSize = 36 , richText = true });
			GUILayout.Label("<color=#2288EE>Transform Sync Baker</color>" , new GUIStyle() { fontSize = 24 , richText = true });

			SYNC = (TransformSync)EditorGUILayout.ObjectField("Transform Sync To Bake" , SYNC , typeof(TransformSync) , true);
			if (GUILayout.Button("Bake"))
			{
				if (SYNC != null)
				{
					SYNC.CalcDelta();
					msg = "Done.";
				}
				else
				{
					msg = "You must choose a TransformSync Component.";

				}
			}
			GUILayout.Label(msg , new GUIStyle() { });
		}

	}
}
