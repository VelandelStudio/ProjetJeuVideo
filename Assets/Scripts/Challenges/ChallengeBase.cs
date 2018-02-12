using UnityEngine;
using System.IO;
/** ChallengeBase, public abstract class
 * @Implements IDisplayable
 * This class is the mother class of all challenges in our game. 
 * As a IDisplayable, it is able to display some information on the screen.
 * Also, Challenges are able to load themselves from a json files and this class checks when a challenge is Succeeded.
 **/
public abstract class ChallengeBase : MonoBehaviour, IDisplayable
{
    protected ChallengeData challengeData;
    protected RoomBehaviour roomBehavior;

    public bool isSucces = false;

    public string Name { get; protected set; }
    public string[] OtherValues { get; protected set; }
    public string[] Description { get; protected set; }
    public bool IsLoaded { get; protected set; }
    
    /** Awake, protected virtual void
     * We use this method to load the challenge itself from a json file.
     **/
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

    /** ConditionToSucced, public abstract bool
     * As an abstract method, we force the child element to have a Condition to success.
     **/
    public abstract bool ConditionToSucced();
    
    /** GiveReward, public abstract void
     * As an abstract method, we force the child element to have a Reward to pass when the challenge is succeeded.
     **/
    public abstract void GiveReward();
    
    /** Update, protected virtual void
     * In the Update method, wh check if the challenge is a Success.
     * If it is, we launch the GiveReward Method and Destroy this script.
     **/
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

    /** GetDescriptionGUI, public string
     * As a Displayable, we use this method to return the Description to be displayed on the screen.
     **/
    public string GetDescriptionGUI()
    {
        return StringHelper.DescriptionBuilder(this);
    }
    
    /** ChallengeData, protected inner class
     * This inner class was made to serve ChallengeBase only. We use this class as a support to load JSON datas of Challenges.
     **/
    [System.Serializable]
    protected class ChallengeData
    {
        public string ScriptName;
        public string Name;
        public string[] OtherValues;
        public string[] Description;
    }
}
