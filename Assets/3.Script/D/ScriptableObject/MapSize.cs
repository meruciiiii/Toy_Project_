using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MapSize", fileName = "MapSize")]
public class MapSize : ScriptableObject
{
	[SerializeField] private Vector3 _LimitMin;
	[SerializeField] private Vector3 _LimitMax;

	public Vector3 LimitMin {
		get {
			return _LimitMin;
		}
	}
	public Vector3 LimitMax {
		get {
			return _LimitMax;
		}
	}
}
