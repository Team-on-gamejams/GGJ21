using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Client {
	public PetType wantedPet;
	public AccessoryType wantedAccessory;
	public string dialogText;

	public Client() {

	}

	public Client(PetType wantedPet, AccessoryType wantedAccessory, string dialogText) {
		this.wantedPet = wantedPet;
		this.wantedAccessory = wantedAccessory;
		this.dialogText = dialogText;
	}
}
