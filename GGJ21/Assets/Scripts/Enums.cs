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
	Cat = 1,
	Dog = 2,
}

public enum AccessoryType : byte {
	None = 0,
	Collar = 1,
}
