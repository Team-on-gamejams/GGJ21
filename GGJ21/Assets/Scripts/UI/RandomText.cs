using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class RandomText : MonoBehaviour {
	[SerializeField] string[] texts;

	[Header("Refs"), Space]
	[SerializeField] TextMeshProUGUI textField;

	private void OnEnable() {
		textField.text = texts.Random();
	}
}
