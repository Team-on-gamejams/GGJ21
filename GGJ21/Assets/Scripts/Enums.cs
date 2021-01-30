using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using Random = UnityEngine.Random;

public enum PetType : byte { 
	None = 0,
	Cat1 = 1,
	Cat2 = 2,
	Cat3 = 3,
	Dog1 = 4,
	Dog2 = 5,
}

public enum AccessoryType : byte {
	None = 0,
	chains = 1,
	collar = 2,
	eye_path = 3,
	hat = 4,
	ribbon = 5,
	spiked_collar = 6,
	sunglasses = 7,
	sweater = 8,
}
