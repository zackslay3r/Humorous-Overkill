﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// require text component
[RequireComponent(typeof(Text))]
public class timer : MonoBehaviour
{
    public float elapsedTime = 0;
    private bool isTiming = true;
    private Text myText;

    [Range(0, 5)]
    public int precision;

    [Tooltip("Change this to change the string displayed in front of the time")]
    public string displayString;

    // runs once when this timer is created at the start of the main scene
    void Start()
    {
        // don't destroy the timer when loading other scenes
        //DontDestroyOnLoad(this.gameObject);

        // get text component
        myText = GetComponent<Text>();

        // elapsed time is currently 0
        elapsedTime = 0.0f;
    }

    void Update()
    {
        // add to elapsed game time (clamp at 1 hour)
        if (isTiming)
        {
            elapsedTime = Mathf.Min(elapsedTime + Time.deltaTime, 60 * 60 - 1);
        }
    }

    // update the text each frame to display the time
    void OnGUI()
    {
        // find minutes
        float minutes = Mathf.Floor(elapsedTime / 60.0f);

        // find seconds
        float seconds = elapsedTime - (minutes * 60.0f);

        string currentDisplayString = displayString + minutes.ToString() + ":" + seconds.ToString("F" + precision.ToString()).Replace(".", ":");
        myText.text = currentDisplayString;
    }

    // allow future scripts to toggle whether the timer should be timing
    // pause menu, etc.
    public void toggleTiming()
    {
        isTiming = !isTiming;
    }

    // returns the elapsed time
    public float getElapsedTime()
    {
        return elapsedTime;
    }
}