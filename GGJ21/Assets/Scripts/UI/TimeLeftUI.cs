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

public class TimeLeftUI : MonoBehaviour {
	[Header("Refs")]
	[SerializeField, GetComponent] TextMeshProUGUI textField;

	public void UpdateValue(float timeLeft) {
		textField.text = (Mathf.RoundToInt(timeLeft) / 60).ToString() + "." + (Mathf.RoundToInt(timeLeft) % 60).ToString("00");
	}
}
