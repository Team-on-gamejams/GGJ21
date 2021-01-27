using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class CardsSelector : MonoBehaviour {
	[NonSerialized] public bool isCanSelect = false;

	public Action OnSelectLeft;
	public Action OnSelectRight;

	[Header("Refs")]
	[SerializeField] PetCard leftCard;
	[SerializeField] PetCard rightCard;

	PlayerInputActions actions;

	const float timeToDeselect = 1.0f;
	float selectTime = 0;

	private void Awake() {
		actions = new PlayerInputActions();
		actions.Enable();

		actions.Player.SelectLeft.started += SelectLeftCardStarted;
		actions.Player.SelectRight.started += SelectRightCardStarted;
		actions.Player.SelectLeft.canceled += SelectLeftCard;
		actions.Player.SelectRight.canceled += SelectRightCard;
	}

	private void OnDestroy() {
		actions.Player.SelectLeft.started -= SelectLeftCardStarted;
		actions.Player.SelectRight.started -= SelectRightCardStarted;
		actions.Player.SelectLeft.canceled -= SelectLeftCard;
		actions.Player.SelectRight.canceled -= SelectRightCard;
	}

	private void Update() {
		if (isCanSelect && selectTime > 0) {
			selectTime -= Time.deltaTime;

			if(selectTime <= 0) {
				selectTime = -1;
				isCanSelect = false;
				SelectLeftCardEnd();
				SelectRightCardEnd();
			}
		}
	}

	public void SelectLeftCardStarted(InputAction.CallbackContext context) {
		selectTime = timeToDeselect;
		SelectLeftCardStart();
	}

	public void SelectRightCardStarted(InputAction.CallbackContext context) {
		selectTime = timeToDeselect;
		SelectRightCardStart();
	}

	public void SelectLeftCard(InputAction.CallbackContext context) {
		SelectLeftCard();

		if (selectTime != 0) {
			selectTime = 0;
			isCanSelect = true;
		}
	}

	public void SelectRightCard(InputAction.CallbackContext context) {
		SelectRightCard();

		if(selectTime != 0) {
			selectTime = 0;
			isCanSelect = true;
		}
	}

	public void SelectLeftCardStart() {
		if (!isCanSelect)
			return;

		Debug.Log("Hover On Left");
		leftCard.Select();
	}

	public void SelectRightCardStart() {
		if (!isCanSelect)
			return;

		Debug.Log("Hover On Right");
		rightCard.Select();
	}

	public void SelectLeftCard() {
		if (!isCanSelect || !leftCard.IsSelected)
			return;

		Debug.Log("Select Left");
		OnSelectLeft();
		SelectLeftCardEnd();
	}

	public void SelectRightCard() {
		if (!isCanSelect || !rightCard.IsSelected)
			return;

		Debug.Log("Select Right");
		OnSelectRight();
		SelectRightCardEnd();
	}

	public void SelectLeftCardEnd() {
		Debug.Log("Not Hover On Left");
		leftCard.Deselect();
	}

	public void SelectRightCardEnd() {
		Debug.Log("Not Hover On Right");
		rightCard.Deselect();
	}
}
