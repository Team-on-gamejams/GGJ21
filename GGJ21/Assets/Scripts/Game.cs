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

public class Game : MonoBehaviour {
	Level Level => levels[currLevel];
	Client Client => levels[currLevel].clients[currClient];

	[Header("Levels")]
	[SerializeField] Level[] levels;

	[Header("UI"), Space]
	[SerializeField] TimeLeftUI timeLeftUI;
	[SerializeField] ClientLeftUI clientLeftUI;
	[SerializeField] LevelUI levelUI;

	[Header("Refs"), Space]
	[SerializeField] PetCard[] cards;
	[SerializeField] CardsSelector cardsSelector;
	[SerializeField] ClientDialog dialog;

	[Header("Prefab Refs"), Space]
	[SerializeField] GameObject clientMoverPrefab;

	[Header("Debug data"), Space]
	[ReadOnly, SerializeField] int currLevel;
	[ReadOnly, ShowNonSerializedField] int currClient;
	[ReadOnly, ShowNonSerializedField] float currLevelTime;

	bool isPlaying = false;

	void Start() {
		cardsSelector.OnSelectLeft += OnSelectLeft;
		cardsSelector.OnSelectRight += OnSelectRight;

		StartGame();
	}

	private void OnDestroy() {
		cardsSelector.OnSelectLeft -= OnSelectLeft;
		cardsSelector.OnSelectRight -= OnSelectRight;
	}

	void Update() {
		if (isPlaying) {
			currLevelTime -= Time.deltaTime;
			if(currLevelTime <= 0) 
				currLevelTime = 0;

			timeLeftUI.UpdateValue(currLevelTime);

			if (currLevelTime <= 0) {
				EndLevel();
			}
		}

	}

	void StartLevel() {
		Debug.Log("Start level");

		if (currLevel == levels.Length) {
			levelUI.UpdateValue("!You win the game!");
			EndGame();
		}
		else {
			isPlaying = enabled = true;
			currLevelTime = Level.secondsForLevel;

			timeLeftUI.UpdateValue(currLevelTime);
			levelUI.UpdateValue($"Level: {currLevel + 1}");
			clientLeftUI.UpdateValue(0, Level.clients.Length);

			LeanTween.delayedCall(1.0f, () => {
				currClient = 0;
				OnNewClient();
			});
		}
	}

	void EndLevel() {
		Debug.Log("End level");

		isPlaying = enabled = false;
	}

	void StartGame() {
		Debug.Log("Start game");

		currLevel = 0;
		StartLevel();
	}
	
	void EndGame() {
		Debug.Log("End game");
	}

	void OnNewClient() {
		if(currClient == Level.clients.Length) {
			EndLevel();

			++currLevel;
			StartLevel();
		}
		else {
			cardsSelector.IsCanSelect = true;

			dialog.ShowText($"[{Client.wantedPet}] [{Client.wantedAccessory}] - {Client.dialogText}");
			//dialog.ShowText(Client.dialogText);
		}
	}

	void OnSelectLeft() {
		OnSelectAnyCard();
	}

	void OnSelectRight() {
		OnSelectAnyCard();
	}

	void OnSelectAnyCard() {
		clientLeftUI.UpdateValue(currClient + 1, Level.clients.Length);
		dialog.Hide();
		
		cardsSelector.IsCanSelect = false;

		LeanTween.delayedCall(1.0f, () => {
			++currClient;
			OnNewClient();
		});
	}
}
