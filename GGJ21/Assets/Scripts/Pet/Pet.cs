using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Pet", menuName = "Pet")]
public class Pet : ScriptableObject {
	public PetType type;

}
