﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRomingCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
        }
		
	}
}
