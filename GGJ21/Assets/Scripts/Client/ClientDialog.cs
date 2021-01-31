using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class ClientDialog : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] float showHideTime = 0.2f;
	[SerializeField] float timeForOneChar = 0.05f;
	[SerializeField] AudioClip writeAudioClip;

	[Header("Refs"), Space]
	[SerializeField] CanvasGroup cg;
	[SerializeField] TextMeshProUGUI textField;

	private void Awake() {
		textField.text = "";
		cg.alpha = 0.0f;
	}

	public void ShowText(string text) {
		LeanTween.cancel(cg.gameObject);
		LeanTweenEx.ChangeAlpha(cg, 1.0f, showHideTime);

		AudioSource writeAS = AudioManager.Instance.PlayLoop(writeAudioClip, channel: AudioManager.AudioChannel.Sound);

		textField.text = text;
		textField.maxVisibleCharacters = 0;
		LeanTween.value(cg.gameObject, 0, text.Length, text.Length * timeForOneChar)
		.setOnUpdate((float c)=>{ 
			textField.maxVisibleCharacters = Mathf.CeilToInt(c);
		})
		.setOnComplete(()=> {
			AudioManager.Instance.ChangeASVolume(writeAS, 0.0f, 0.1f);
			Destroy(writeAS.gameObject, 0.2f);
		});

	}

	public void Hide() {
		LeanTween.cancel(cg.gameObject, true);
		LeanTweenEx.ChangeAlpha(cg, 0.0f, showHideTime);
	}
}
