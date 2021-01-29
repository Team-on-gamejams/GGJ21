using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Client", menuName = "Client")]
public class Client : ScriptableObject {
	public PetType wantedPet;
	public AccessoryType wantedAccessory;
	[Multiline] public string dialogText;
}
