using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;
using OneLine;
using Random = UnityEngine.Random;

public class Level {
	public float secondsForLevel;
	public int clients;

	public Level() {

	}

	public Level(float secondsForLevel, int clients) {
		this.secondsForLevel = secondsForLevel;
		this.clients = clients;
	}
}
