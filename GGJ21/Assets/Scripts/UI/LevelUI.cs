using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Nrjwolf.Tools.AttachAttributes;
using Random = UnityEngine.Random;

public class LevelUI : MonoBehaviour {
	[Header("Refs")]
	[SerializeField, GetComponent] TextMeshProUGUI textField;

	public void UpdateValue(string text) {
		textField.text = text;
	}
}
