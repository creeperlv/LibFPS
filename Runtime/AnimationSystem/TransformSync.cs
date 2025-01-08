using System;
using System.Collections.Generic;
using UnityEngine;

namespace LibFPS.AnimationSystem
{
	public class TransformSync : MonoBehaviour
	{
		public List<TrackedTransforms> transforms = new List<TrackedTransforms>();
		public Transform OperateBase;
		public bool UsePreBakedData;
		public bool GlobalPos;
		public void Clean()
		{
			for (int i = transforms.Count - 1; i >= 0; i--)
			{
				TrackedTransforms t = transforms[i];
				if (t.Source == null || t.Target == null)
				{
					transforms.RemoveAt(i);
				}
			}
		}
		public void FillTargetName()
		{
			foreach (TrackedTransforms t in transforms)
			{
				t.TargetName = t.Target.name;
			}
		}
		Transform RecursiveFind(Transform t, string name)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				var item = t.GetChild(i);
				if (item.name == name) return item;
				var o = RecursiveFind(item, name);
				if (o != null)
				{
					return o;
				}
			}
			return null;
		}
		public void MatchTargetByName()
		{
			foreach (TrackedTransforms t in transforms)
			{
				t.Target = RecursiveFind(OperateBase, t.TargetName);
			}
		}
		public void CalcDelta()
		{
			if (GlobalPos)
			{
				foreach (var transform in transforms)
				{
					transform.PositionDelta = transform.Target.position - transform.Source.position;
					transform.RotationDelta = (Quaternion.Euler(transform.Target.eulerAngles - transform.Source.eulerAngles));
				}
			}
			else
			{
				foreach (var transform in transforms)
				{
					transform.PositionDelta = transform.Target.localPosition - transform.Source.localPosition;
					transform.RotationDelta = (Quaternion.Euler(transform.Target.eulerAngles - transform.Source.eulerAngles));
				}
			}
		}
		public void Start()
		{
			if (!UsePreBakedData) CalcDelta();
		}

		public void Update()
		{
			if (GlobalPos)
			{
				foreach (var item in transforms)
				{
					item.Target.position = item.Source.position + item.PositionDelta;
					item.Target.rotation = item.Source.rotation * item.RotationDelta;
				}
			}
			else
			{
				foreach (var item in transforms)
				{
					item.Target.localPosition = item.Source.localPosition + item.PositionDelta;
					item.Target.rotation = item.Source.rotation * item.RotationDelta;
				}
			}
		}
	}
	[Serializable]
	public class TrackedTransforms
	{
		public Transform Source;
		public Transform Target;
		public string TargetName;
		public Vector3 PositionDelta;
		public Quaternion RotationDelta;
	}
}
