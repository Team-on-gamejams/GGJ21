using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using OneLine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject {
	[Header("Time")]
	public float secondsForLevel;
	[OneLine] [Tooltip("Stars - (0, 1, 2, 3)")] public StartData starsTimings;

	[Space]
	[OneLine.Expandable] public Client[] clients;

	[Serializable]
	public struct StartData {
		public float secondsForStar0;
		public float secondsForStar1;
		public float secondsForStar2;
		public float secondsForStar3;
	}
}
