using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Nrjwolf.Tools.AttachAttributes;
using Random = UnityEngine.Random;

public class ClientLeftUI : MonoBehaviour {
	[Header("Refs")]
	[SerializeField, GetComponent] TextMeshProUGUI textField;

	public void UpdateValue(int curr, int max) {
		textField.text = $"{curr}/{max}";
	}
}
