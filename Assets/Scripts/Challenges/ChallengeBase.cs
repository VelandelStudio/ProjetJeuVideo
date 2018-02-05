﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public abstract class ChallengeBase : MonoBehaviour
{
    protected ChallengeData challengeData;

    public bool isSucces = false;

    protected virtual void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "ChallengeData.json");
        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);
            ChallengeData[] data = JsonHelper.getJsonArray<ChallengeData>(jsonFile);
            foreach (ChallengeData challenge in data)
            {
                if (challenge.ScriptName == GetType().ToString())
                {
                    challengeData = challenge;
                    break;
                }
            }

            if (data == null)
            {
                // Nothing for the moment
            }
        }
        else
        {
            Debug.LogError("Cannot load challenge data!");
        }
    }

    public abstract bool ConditionToSucced();

    protected virtual void Update()
    {
        if (ConditionToSucced())
        {
            isSucces = true;
        }
    }

    public void DisplayContent()
    {
        // TO DO....
    }

    [System.Serializable]
    protected class ChallengeData
    {
        public string ScriptName;
        public string Name;
        public string[] OtherValues;
        public string[] Description;
    }
}