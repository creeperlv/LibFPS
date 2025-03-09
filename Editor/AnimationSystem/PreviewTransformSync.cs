using LibFPS.AnimationSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LibFPS.Editor.AnimationSystem
{
	public class PreviewTransformSync : EditorWindow
	{
		[MenuItem("LibFPS/AnimationSystem/Transform Sync Preview")]
		public static void ShowWindow()
		{
			GetWindow(typeof(PreviewTransformSync));
		}
		public static void OpenWindow(TransformSync sync)
		{
			var w = GetWindow<PreviewTransformSync>();
			w.target = sync;
		}
		bool IsPreviewing = false;
		public TransformSync target;
		public TransformSync _target;
		struct PackTransform
		{
			internal Vector3 Pos;
			internal Quaternion Rot;

			public PackTransform(Vector3 pos, Quaternion rot)
			{
				Pos = pos;
				Rot = rot;
			}
		}
		List<PackTransform> data;
		bool IsRestored = false;
		void Restore(TransformSync target)
		{
			if (IsRestored) return;
			IsRestored = true;
			for (int i = 0; i < target.transforms.Count; i++)
			{
				TrackedTransforms t = target.transforms[i];
				t.Target.localPosition = data[i].Pos;
				t.Target.localRotation = data[i].Rot;
			}
		}
		public void OnGUI()
		{
			GUILayout.Label("<color=#2288EE>AnimationSystem</color>", new GUIStyle() { fontSize = 28, richText = true });
			GUILayout.Label("<color=#2288EE>Transform Sync Preview</color>", new GUIStyle() { fontSize = 18, richText = true });

			target = (TransformSync)EditorGUILayout.ObjectField("Transform Sync To Preview", target, typeof(TransformSync), true);
			if (_target != target)
			{
				if (IsPreviewing)
				{
					IsPreviewing = false;
					Restore(_target);
				}
				_target = target;
			}
			if (GUILayout.Button(IsPreviewing ? "Stop Preview" : "Start Preview"))
			{
				IsPreviewing = !IsPreviewing;
				if (IsPreviewing)
				{
					data = new List<PackTransform>();
					foreach (var t in target.transforms)
					{
						data.Add(new PackTransform(t.Target.localPosition, t.Target.localRotation));
					}
					IsRestored = false;
				}
				else
				{
					Restore(target);
				}
			}
			//IsPreviewing = EditorGUILayout.Toggle("Is Previewing" , IsPreviewing);
		}
		public void Update()
		{
			if (IsPreviewing)
				if (target != null)
					target.FixedUpdate();
		}
	}
}
