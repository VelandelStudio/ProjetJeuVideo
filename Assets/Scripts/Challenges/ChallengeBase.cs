using UnityEngine;
using System.IO;

public abstract class ChallengeBase : MonoBehaviour, IDisplayable
{
    protected ChallengeData challengeData;
    protected RoomBehaviour roomBehavior;

    public bool isSucces = false;

    public string Name { get; protected set; }
    public string[] OtherValues { get; protected set; }
    public string[] Description { get; protected set; }
    public bool IsLoaded { get; protected set; }

    protected virtual void Awake()
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

        roomBehavior = GetComponent<RoomBehaviour>();

        Name = challengeData.Name;
        Description = challengeData.Description;
        OtherValues = challengeData.OtherValues;
    }

    public abstract bool ConditionToSucced();

    public abstract void GiveReward();

    protected virtual void Update()
    {
        if (!isSucces && ConditionToSucced())
        {
            isSucces = true;

            Debug.Log("YOU IS BOGOSS : this is Succes");
            GiveReward();
            Destroy(this);
        }
    }

    public string GetDescriptionGUI()
    {
        return StringHelper.DescriptionBuilder(this);
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