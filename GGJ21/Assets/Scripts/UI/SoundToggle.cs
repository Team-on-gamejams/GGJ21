using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class SoundToggle : MonoBehaviour {
	[SerializeField] Image SoundImage = null;
	[SerializeField] Sprite[] SoundImageState = null;

	private void OnEnable() {
		SoundImage.sprite = SoundImageState[AudioManager.Instance.IsEnabled ? 1 : 0];
	}

	public void OnSoundClick() {
		AudioManager.Instance.IsEnabled = !AudioManager.Instance.IsEnabled;
		SoundImage.sprite = SoundImageState[AudioManager.Instance.IsEnabled ? 1 : 0];
	}
}
