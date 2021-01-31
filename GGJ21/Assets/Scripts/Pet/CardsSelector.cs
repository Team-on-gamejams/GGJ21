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
	public bool IsCanSelect {
		get => isCanSelect;
		set {
			bool oldValue = isCanSelect;
			isCanSelect = value;

			if (!oldValue && value) {
				if (isMouseOverLeft)
					SelectLeftCardStart();
				else if (isMouseOverRight)
					SelectRightCardStart();
			}
				
		}
	}
	bool isCanSelect = false;
	bool isMouseOverLeft = false;
	bool isMouseOverRight = false;

	public Action OnSelectLeft;
	public Action OnSelectRight;

	[Header("Refs")]
	[SerializeField] PetCard leftCard;
	[SerializeField] PetCard rightCard;
	[Space]
	[SerializeField] Transform hideLeftAnchor;
	[SerializeField] Transform hideRightAnchor;
	[SerializeField] Transform openLeftAnchor;
	[SerializeField] Transform openRightAnchor;
	[SerializeField] Transform selectLeftAnchor;
	[SerializeField] Transform selectRightAnchor;

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

		leftCard.transform.position = hideLeftAnchor.position;
		rightCard.transform.position = hideRightAnchor.position;
	}

	private void OnDestroy() {
		actions.Player.SelectLeft.started -= SelectLeftCardStarted;
		actions.Player.SelectRight.started -= SelectRightCardStarted;
		actions.Player.SelectLeft.canceled -= SelectLeftCard;
		actions.Player.SelectRight.canceled -= SelectRightCard;
	}

	private void Update() {
		if (IsCanSelect && selectTime > 0) {
			selectTime -= Time.deltaTime;

			if(selectTime <= 0) {
				selectTime = 0;
				if(leftCard.IsSelected)
					SelectLeftCardEnd();
				if(rightCard.IsSelected)
					SelectRightCardEnd();
			}
		}
	}

	#region KEYBOARD
	public void SelectLeftCardStarted(InputAction.CallbackContext context) {
		if (!IsCanSelect || rightCard.IsSelected)
			return;

		selectTime = timeToDeselect;
		SelectLeftCardStart();
	}

	public void SelectRightCardStarted(InputAction.CallbackContext context) {
		if (!IsCanSelect || leftCard.IsSelected)
			return;

		selectTime = timeToDeselect;
		SelectRightCardStart();
	}

	public void SelectLeftCard(InputAction.CallbackContext context) {
		if (!IsCanSelect || rightCard.IsSelected)
			return;

		SelectLeftCard();

		if (selectTime != 0) {
			selectTime = 0;
			IsCanSelect = false;
		}
	}

	public void SelectRightCard(InputAction.CallbackContext context) {
		if (!IsCanSelect || leftCard.IsSelected)
			return;

		SelectRightCard();

		if (selectTime != 0) {
			selectTime = 0;
			IsCanSelect = false;
		}
	}

	#endregion

	#region Mouse
	public void MouseSelectLeftCardStart() {
		isMouseOverLeft = true;
		SelectLeftCardStart();
	}

	public void MouseSelectRightCardStart() {
		isMouseOverRight = true;
		SelectRightCardStart();
	}

	public void MouseSelectLeftCardEnd() {
		isMouseOverLeft = false;
		SelectLeftCardEnd();
	}

	public void MouseSelectRightCardEnd() {
		isMouseOverRight = false;
		SelectRightCardEnd();
	}
	#endregion

	#region BASE
	public void SelectLeftCardStart() {
		if (!IsCanSelect) {
			return;
		}

		Debug.Log("Hover On Left");
		leftCard.Select(selectLeftAnchor);
	}

	public void SelectRightCardStart() {
		if (!IsCanSelect) {
			return;
		}


		Debug.Log("Hover On Right");
		rightCard.Select(selectRightAnchor);
	}

	public void SelectLeftCard() {
		if (!IsCanSelect || !leftCard.IsSelected)
			return;

		Debug.Log("Select Left");
		SelectLeftCardEnd();
		OnSelectLeft();
	}

	public void SelectRightCard() {
		if (!IsCanSelect || !rightCard.IsSelected)
			return;

		Debug.Log("Select Right");
		SelectRightCardEnd();
		OnSelectRight();
	}

	public void SelectLeftCardEnd() {
		Debug.Log("Not Hover On Left");
		leftCard.Deselect(openLeftAnchor);
	}

	public void SelectRightCardEnd() {
		Debug.Log("Not Hover On Right");
		rightCard.Deselect(openRightAnchor);
	}
	#endregion

	#region Animation
	public void PlayShowAnimation() {
		leftCard.transform.position = hideLeftAnchor.position;
		rightCard.transform.position = hideRightAnchor.position;

		LeanTween.cancel(leftCard.gameObject, true);
		LeanTween.cancel(rightCard.gameObject, true);

		leftCard.transform.localEulerAngles = leftCard.transform.localEulerAngles.SetZ(0.0f);
		rightCard.transform.localEulerAngles = leftCard.transform.localEulerAngles.SetZ(0.0f);

		LeanTween.move(leftCard.gameObject, openLeftAnchor, 0.2f).setEase(LeanTweenType.easeOutBack);
		LeanTween.move(rightCard.gameObject, openRightAnchor, 0.2f).setEase(LeanTweenType.easeOutBack);
	}

	public void PlayHideAnimation() {
		LeanTween.cancel(leftCard.gameObject, true);
		LeanTween.cancel(rightCard.gameObject, true);

		LeanTween.move(leftCard.gameObject, hideLeftAnchor, 0.2f).setEase(LeanTweenType.easeInBack);
		LeanTween.move(rightCard.gameObject, hideRightAnchor, 0.2f).setEase(LeanTweenType.easeInBack);
	}
	#endregion
}
