using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class PetCard : MonoBehaviour {
	public bool IsSelected { private set; get; }
	public PetType petType { private set; get; }
	public AccessoryType accessoryType { private set; get; }

	[Header("Data")]
	[SerializeField] PetData[] pets;

	[Header("Refs")]
	[SerializeField] Image cardBack;
	[SerializeField] Image petImage;
	[SerializeField] ImageAnimator petImageAnimator;
	[SerializeField] Image accessoryImage;

	PetData petData;
	PetSpriteData petSprites;
	Sprite accessorySprite;


	#region Data
	public void SetCard(PetType _petType, AccessoryType _accessoryType, bool isNeedThis) {
		if (isNeedThis) {
			petType = _petType;
			accessoryType = _accessoryType;
		}
		else {
			if(_petType != PetType.None) {
				do {
					Array pets = Enum.GetValues(typeof(PetType));
					petType = (PetType)pets.GetValue(Random.Range(0, pets.Length));
				} while (petType == _petType);
			}

			if (_accessoryType != AccessoryType.None) {
				do {
					Array accessory = Enum.GetValues(typeof(AccessoryType));
					accessoryType = (AccessoryType)accessory.GetValue(Random.Range(0, accessory.Length));
				} while (accessoryType == _accessoryType);
			}
		}
		

		if(petType == PetType.None) {
			int randomNum = Random.Range(0, pets.Length);
			petData = pets[randomNum];
			petType = (PetType)(randomNum);
		}
		else {
			petData = pets[(int)(petType) - 1];
		}
		petSprites = petData.sprites.Random();

		if (accessoryType == AccessoryType.None) {
			int randomNum = Random.Range(0, petData.accessories.Length + 1);
			if(randomNum != petData.accessories.Length) {
				accessorySprite = petData.accessories[randomNum];
				accessoryType = (AccessoryType)(randomNum);
			}
			else {
				accessorySprite = null;
			}
		}
		else {
			accessorySprite = petData.accessories[(int)(accessoryType) - 1]; ;
		}

		petImageAnimator.SetSpritesDublicateInner(petSprites.sprites);

		accessoryImage.sprite = accessorySprite;
		accessoryImage.color = accessoryImage.color.SetA(accessorySprite == null ? 0 : 1);
	}

	public void OnClick(bool isRight) {
		AudioManager.Instance.Play(isRight ? petData.onRightClickClip.Random() : petData.onWrongClickClip.Random());
	}

	[Serializable]
	struct PetData {
		public PetSpriteData[] sprites;
		public Sprite[] accessories;
		public AudioClip[] onRightClickClip;
		public AudioClip[] onWrongClickClip;
	}

	[Serializable]
	struct PetSpriteData {
		public Sprite[] sprites;
	}
	#endregion

	#region Selection
	
	public void Select(Transform pos) {
		if (IsSelected)
			return;
		IsSelected = true;

		LeanTween.cancel(gameObject, true);

		LeanTween.move(gameObject, pos, 0.2f).setEase(LeanTweenType.easeInQuad);

		float angle = transform.GetComponent<RectTransform>().anchoredPosition.x < 0 ? -20.0f : 20.0f;
		LeanTween.value(gameObject, gameObject.transform.eulerAngles.z >= 300 ? gameObject.transform.eulerAngles.z - 360 : gameObject.transform.eulerAngles.z, angle, 0.2f)
			.setOnUpdate((float z)=> { 
				gameObject.transform.eulerAngles = gameObject.transform.eulerAngles.SetZ(z);
			});
	}

	public void Deselect(Transform pos) {
		if (!IsSelected)
			return;
		IsSelected = false;

		LeanTween.cancel(gameObject, true);

		LeanTween.move(gameObject, pos, 0.2f).setEase(LeanTweenType.easeInQuad);
		LeanTween.value(gameObject, gameObject.transform.eulerAngles.z >= 300 ? gameObject.transform.eulerAngles.z - 360 : gameObject.transform.eulerAngles.z, 0, 0.2f)
			.setOnUpdate((float z) => {
				gameObject.transform.eulerAngles = gameObject.transform.eulerAngles.SetZ(z);
			});
	}
	#endregion
}
