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

public class PetCard : MonoBehaviour {
	public bool IsSelected { private set; get; }

	[Header("Refs")]
	[SerializeField, GetComponentInChildren] Image cardBack;

	public void Select() {
		if (IsSelected)
			return;
		IsSelected = true;

		cardBack.color = Color.yellow;
	}

	public void Deselect() {
		if (!IsSelected)
			return;
		IsSelected = false;

		cardBack.color = Color.white;
	}
}
