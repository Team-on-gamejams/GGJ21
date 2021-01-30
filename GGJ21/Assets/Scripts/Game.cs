using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OneLine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour {
	Level Level { get; set; }
	Client Client { get; set; }

	[Header("Data")]
	[SerializeField] [MinMaxSlider(0, 3600)] Vector2 timerRange = new Vector2(60, 360f);
	[SerializeField] [MinMaxSlider(0, 100)] Vector2 clientsRange = new Vector2(5, 20);

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
	[ReadOnlyField, SerializeField] int currLevelId;
	[ReadOnlyField, SerializeField] int currClientId;
	[ReadOnlyField, SerializeField] float currLevelTime;

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
			if (currLevelTime <= 0)
				currLevelTime = 0;

			timeLeftUI.UpdateValue(currLevelTime);

			if (currLevelTime <= 0) {
				EndLevel();
			}
		}

	}

	void StartLevel() {
		Debug.Log("Start level");

		Level = new Level(timerRange.GetRandomValueFloat(), clientsRange.GetRandomValue());

		++currLevelId;
		currClientId = 0;

		isPlaying = enabled = true;
		currLevelTime = Level.secondsForLevel;

		timeLeftUI.UpdateValue(currLevelTime);
		levelUI.UpdateValue($"Level: {currLevelId}");
		clientLeftUI.UpdateValue(0, Level.clients);

		LeanTween.delayedCall(1.0f, () => {
			OnNewClient();
		});
	}

	void EndLevel() {
		Debug.Log("End level");

		isPlaying = enabled = false;
	}

	void StartGame() {
		Debug.Log("Start game");

		currLevelId = 0;
		StartLevel();
	}

	void OnNewClient() {
		if (currClientId == Level.clients) {
			EndLevel();

			++currLevelId;
			StartLevel();
		}
		else {
			bool randomPet = Random.Range(0, 2) == 1;
			Array pets = Enum.GetValues(typeof(PetType));
			Array accessory = Enum.GetValues(typeof(AccessoryType));
			PetType pet = randomPet ? (PetType)pets.GetValue(Random.Range(0, pets.Length)) : PetType.None;
			AccessoryType acs = !randomPet ? (AccessoryType)accessory.GetValue(Random.Range(0, accessory.Length)) : AccessoryType.None;

			Client = new Client(
				pet,
				acs,
				$"[{pet}] [{acs}] - Dialog text"
			);

			cardsSelector.IsCanSelect = true;

			if(Random.Range(0, 2) == 1) {
				cards[0].SetCard(Client.wantedPet, Client.wantedAccessory);
				cards[1].SetCard(PetType.None, AccessoryType.None);
			}
			else {
				cards[0].SetCard(PetType.None, AccessoryType.None);
				cards[1].SetCard(Client.wantedPet, Client.wantedAccessory);
			}

			dialog.ShowText(Client.dialogText);
		}
	}

	void OnSelectLeft() {
		bool isRight = CheckCard(0);
		OnSelectAnyCard(isRight);
	}

	void OnSelectRight() {
		bool isRight = CheckCard(1);
		OnSelectAnyCard(isRight);
	}

	bool CheckCard(int id) {
		bool isRight = true;

		if (Client.wantedPet != PetType.None)
			isRight = Client.wantedPet == cards[id].petType;

		if (isRight && Client.wantedAccessory != AccessoryType.None)
			isRight = Client.wantedAccessory == cards[id].accessoryType;

		Debug.Log($"Is right: {isRight}");
		return isRight;
	}

	void OnSelectAnyCard(bool isRight) {
		if(isRight)
			clientLeftUI.UpdateValue(currClientId + 1, Level.clients);
		dialog.Hide();

		cardsSelector.IsCanSelect = false;

		LeanTween.delayedCall(1.0f, () => {
			if(isRight)
				++currClientId;
			OnNewClient();
		});
	}
}
