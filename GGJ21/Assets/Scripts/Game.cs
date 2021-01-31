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
	[SerializeField] DialogData[] dialogs;
	int currDialogData;

	[Header("Menu"), Space]
	[SerializeField] MenuManager menuManager;
	[SerializeField] WinPopupMenu winPopup;
	[SerializeField] LosePopupMenu losePopup;
	[SerializeField] TutorialOverlayMenu tutorialOverlay;

	[Header("UI"), Space]
	[SerializeField] TimeLeftUI timeLeftUI;
	[SerializeField] ClientLeftUI clientLeftUI;
	[SerializeField] LevelUI levelUI;

	[Header("Refs"), Space]
	[SerializeField] PetCard[] cards;
	[SerializeField] CardsSelector cardsSelector;
	[SerializeField] ClientDialog dialog;
	[SerializeField] ClientMover clientMover;

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

		if(currLevelId == 1) {
			menuManager.Show(tutorialOverlay, false);

			currLevelTime = Level.secondsForLevel = 60 * 5;
			timeLeftUI.UpdateValue(currLevelTime);
		}
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
				menuManager.Show(losePopup, false);
			}
		}

	}

	public void ClickOnEndGamePopup() {
		TransitionManager.Instance.StartTransitonEffectIn(()=> {
			menuManager.HideTopMenu(true);
			StartLevel();
			TransitionManager.Instance.StartTransitonEffectOut();
		});
	}

	void StartLevel() {
		Debug.Log("Start level");
		
		Level = new Level(timerRange.GetRandomValueFloat(), clientsRange.GetRandomValue());

		currDialogData = 0;
		dialogs.Shuffle();

		++currLevelId;
		currClientId = 0;

		isPlaying = enabled = true;
		currLevelTime = Level.secondsForLevel;

		timeLeftUI.UpdateValue(currLevelTime);
		levelUI.UpdateValue($"Level: {currLevelId}");
		clientLeftUI.UpdateValue(0, Level.clients);

		OnNewClient();
	}

	void EndLevel() {
		Debug.Log("End level");

		isPlaying = enabled = false;
		cardsSelector.IsCanSelect = false;
	}

	void StartGame() {
		Debug.Log("Start game");

		currLevelId = 0;
		StartLevel();
	}

	void OnNewClient() {
		clientMover.EndClient();

		if (currClientId == Level.clients) {
			EndLevel();

			++currLevelId;
			menuManager.Show(winPopup, false);
		}
		else {
			LeanTween.delayedCall(0.3f, () => {
				clientMover.StartNewClient();

				LeanTween.delayedCall(0.3f, () => {
					if(currDialogData == dialogs.Length) {
						currDialogData = 0;
						dialogs.Shuffle();
					}

					DialogData data = dialogs[currDialogData];
					PetType wantedPet = data.wantedPet;
					++currDialogData;

					if (wantedPet == PetType.Cat1) {
						switch (Random.Range(0, 4)) {
							case 0:
								wantedPet = PetType.Cat1;
								break;
							case 1:
								wantedPet = PetType.Cat2;
								break;
							case 2:
								wantedPet = PetType.Cat3;
								break;
							case 3:
								wantedPet = PetType.Cat4;
								break;
						}
					}
					else if (wantedPet == PetType.Dog1) {
						switch (Random.Range(0, 3)) {
							case 0:
								wantedPet = PetType.Dog1;
								break;
							case 1:
								wantedPet = PetType.Dog2;
								break;
							case 2:
								wantedPet = PetType.Dog3;
								break;
						}
					}

					Client = new Client(
						wantedPet,
						data.wantedAccessory,
						data.dialogText
					);

					cardsSelector.IsCanSelect = true;

					if (Random.Range(0, 2) == 1) {
						cards[0].SetCard(Client.wantedPet, Client.wantedAccessory);
						cards[1].SetCard(PetType.None, AccessoryType.None);
					}
					else {
						cards[0].SetCard(PetType.None, AccessoryType.None);
						cards[1].SetCard(Client.wantedPet, Client.wantedAccessory);
					}

					dialog.ShowText(Client.dialogText);
				});
			});
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

		cards[id].OnClick(isRight);

		Debug.Log($"Is right: {isRight}");
		return isRight;
	}

	void OnSelectAnyCard(bool isRight) {
		if(isRight)
			clientLeftUI.UpdateValue(currClientId + 1, Level.clients);

		dialog.Hide();

		cardsSelector.IsCanSelect = false;

		if(isRight)
			++currClientId;
		OnNewClient();
	}

	[Serializable]
	struct DialogData {
		public PetType wantedPet;
		public AccessoryType wantedAccessory;
		[Multiline] public string dialogText;
	}
}
