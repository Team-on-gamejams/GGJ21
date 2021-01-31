using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ClientMover : MonoBehaviour {
	[Header("Sprites"), Space]
	[SerializeField] ClientData[] clients;

	[Header("Refs"), Space]
	[SerializeField] SpriteRendererAnimator clientAnimator;
	[SerializeField] SpriteRendererAnimator clientInactiveAnimator;
	[SerializeField] SpriteRenderer clientInactiveSprite;

	int currRandomId = 0;

	[Serializable]
	struct ClientData {
		public Sprite[] sprites;
	}


	private void OnEnable() {
		SetRandomSprite();
	}

	public void StartNewClient() {
		LeanTweenEx.ChangeAlpha(clientInactiveSprite, 0.0f, 0.2f);
	}

	public void EndClient() {
		LeanTweenEx.ChangeAlpha(clientInactiveSprite, 1.0f, 0.2f)
		.setOnComplete(()=> {
			SetRandomSprite();
		});
	}

	void SetRandomSprite() {
		if(currRandomId == clients.Length) {
			currRandomId = 0;
		}

		if(currRandomId == 0) {
			clients.Shuffle();
		}

		ClientData data = clients[currRandomId];
		Sprite[] sprites = data.sprites;

		clientAnimator.SetSpritesDublicateInner(sprites);
		clientInactiveAnimator.SetSpritesDublicateInner(sprites);

		++currRandomId;
	}
}
