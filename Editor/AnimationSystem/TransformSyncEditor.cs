using LibFPS.AnimationSystem;
using UnityEditor;
using UnityEngine;

namespace LibFPS.Editor.AnimationSystem
{
	[CustomEditor(typeof(TransformSync))]
	public class TransformSyncEditor : UnityEditor.Editor
	{
		bool SyncWithBase;
		TransformSync Target_Base;
		public string TypeCleanSearcher;
		public override void OnInspectorGUI()
		{

			DrawDefaultInspector();
			Target_Base = (TransformSync)target;
			if (GUILayout.Button("Bake"))
			{
				Target_Base.CalcDelta();
			}
			if (GUILayout.Button("Live Preview"))
			{
				PreviewTransformSync.OpenWindow(Target_Base);
			}
			if (GUILayout.Button("Clean Missing Items"))
			{
				Target_Base.Clean();
			}
			GUILayout.Label("Type to clean");
			TypeCleanSearcher = GUILayout.TextField(TypeCleanSearcher);
			if (GUILayout.Button("Clean Target Types"))
			{
				if (TypeCleanSearcher != "")
					foreach (var item in Target_Base.transforms)
					{
						var c = item.Source.GetComponents<MonoBehaviour>();
						for (int i = c.Length - 1; i >= 0; i--)
						{
							MonoBehaviour t = c[i];
							if (t.GetType().FullName.Contains(TypeCleanSearcher))
							{
								DestroyImmediate(t);
							}
						}
					}
			}

		}
	}
}
