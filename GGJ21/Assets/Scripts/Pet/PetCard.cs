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


	#region Data
	public void SetCard(PetType _petType, AccessoryType _accessoryType) {
		petType = _petType;
		accessoryType = _accessoryType;

		PetData petData;
		PetSpriteData petSprites;
		Sprite accessorySprite;

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

	[Serializable]
	struct PetData {
		public PetSpriteData[] sprites;
		public Sprite[] accessories;
	}

	[Serializable]
	struct PetSpriteData {
		public Sprite[] sprites;
	}
	#endregion

	#region Selection
	
	public void Select() {
		if (IsSelected)
			return;
		IsSelected = true;

		cardBack.color = Color.yellow;
	}

	public void Deselect() {
		if (!IsSelected)
			return;
		IsSelected = false;

		cardBack.color = Color.white;
	}
	#endregion
}
