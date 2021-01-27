using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
	[SerializeField] ClientDialog dialog;

	[Header("Prefab Refs"), Space]
	[SerializeField] GameObject clientMoverPrefab;

	[Header("Debug data"), Space]
	[ReadOnly, SerializeField] int currLevel;
	[ReadOnly, ShowNonSerializedField] int currClient;
	[ReadOnly, ShowNonSerializedField] float currLevelTime;

	bool isPlaying = false;
	bool isCanSelect = false;

	PlayerInputActions actions;

	private void Awake() {
		actions = new PlayerInputActions();
		actions.Enable();

		actions.Player.SelectLeft.started += SelectLeftCardStarted;
		actions.Player.SelectRight.started += SelectRightCardStarted;
		actions.Player.SelectLeft.performed += SelectLeftCard;
		actions.Player.SelectRight.performed += SelectRightCard;
	}

	private void OnDestroy() {
		actions.Player.SelectLeft.started -= SelectLeftCardStarted;
		actions.Player.SelectRight.started -= SelectRightCardStarted;
		actions.Player.SelectLeft.performed -= SelectLeftCard;
		actions.Player.SelectRight.performed -= SelectRightCard;
	}

	void Start() {
		StartGame();
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
		levelUI.UpdateValue("!You win the game!");

		if (currLevel == levels.Length) {
			EndGame();
		}
		else {
			isPlaying = enabled = true;

			currLevelTime = Level.secondsForLevel;
			timeLeftUI.UpdateValue(currLevelTime);
			levelUI.UpdateValue($"Level: {currLevel + 1}");

			currClient = 0;
			OnNewClient();
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
		clientLeftUI.UpdateValue(currClient, Level.clients.Length);

		if(currClient == Level.clients.Length) {
			EndLevel();

			++currLevel;
			StartLevel();
		}
		else {
			isCanSelect = true;
		}
	}

	void SelectLeftCardStarted(InputAction.CallbackContext context) {
		if (!isCanSelect)
			return;

		Debug.Log("Hover On Left");

	}

	void SelectRightCardStarted(InputAction.CallbackContext context) {
		if (!isCanSelect)
			return;

		Debug.Log("Hover On Right");

	}

	void SelectLeftCard(InputAction.CallbackContext context) {
		if (!isCanSelect)
			return;

		Debug.Log("Select Left");
		OnSelectAnyCard();
	}

	void SelectRightCard(InputAction.CallbackContext context) {
		if (!isCanSelect)
			return;

		Debug.Log("Select Right");
		OnSelectAnyCard();
	}

	void OnSelectAnyCard() {
		isCanSelect = false;
		++currClient;
		OnNewClient();
	}
}
