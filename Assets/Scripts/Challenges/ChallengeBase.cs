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
    protected Datas challengeData;
    protected RoomBehaviour roomBehavior;

    public bool isSucces = false;

    public string Name { get { return challengeData.Name; } protected set { } }
    public string[] OtherValues { get { return challengeData.OtherValues; } protected set { } }
    public string[] Description { get { return challengeData.Description; } protected set { } }
    public bool IsLoaded { get { return challengeData.IsLoaded; } protected set { } }

    /** Awake, protected virtual void
     * We use this method to load the challenge itself from a json file.
     **/
    protected virtual void Awake()
    {
        challengeData = new Datas(GetType().ToString(), "ChallengeData.json");
        roomBehavior = GetComponent<RoomBehaviour>();
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
}
