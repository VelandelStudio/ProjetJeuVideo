using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUIStatusDisplayer, public class
 * Displayer that handles informations when the player receive a Status. 
 **/
public class GUIStatusDisplayer : MonoBehaviour
{

    public StatusBase Status;
    [SerializeField] private Image _StatusImage;
    [SerializeField] private Image _CDImage;
    [SerializeField] private Text _CDText;
    [SerializeField] private Outline _outline;

    private float _duration;

    /** AttributeStatusBase, public void method
     * @param : StatusBase
     * This method is launched by the StatusBase when the Status is Applied.
     * We set everything we need t(o display informations of the Status (passed as a parameter) on the screen.
     * If the StatusBase implements IBuff then the outline is green, else, it is red.
     **/
    public void AttributeStatusBase(StatusBase status)
    {
        Status = status;
        _duration = status.Duration;
        _CDImage.fillAmount = 0;
        _CDText.text = ((int)Status.Duration + 1).ToString();
        if (status is IBuff)
        {
            _outline.effectColor = Color.green;
        }
        else
        {
            _outline.effectColor = Color.red;
        }
    }

    /** ResetGUIStatus, public void method
     * This method is launched by the StatusBase when the Status is Reseted.
     **/
    public void ResetGUIStatus()
    {
        _duration = Status.Duration;
        _CDText.text = ((int)Status.Duration + 1).ToString();
    }

    /** DestroyGUIStatus, public void method
     * This method is launched by the StatusBase when the Status is Destroyed.
     **/
    public void DestroyGUIStatus()
    {
        _CDText.text = "";
        CursorBehaviour.CancelTooltip();
        Destroy(gameObject);
    }

    /** Update, protected void method
     * The Update method is used to display the correct CD Graphics element on the screen and
     * calculate, at each frame, what is the remaining time of the StatusBase associated.
     **/
    protected void Update()
    {
        if (Status == null)
        {
            return;
        }

        _duration -= Time.deltaTime;
        _CDText.text = ((int)_duration + 1).ToString();
        _CDImage.fillAmount = 1 - _duration / Status.Duration;
    }

    /** MouseEnter, public void Method
	 * This Method is launched with an event trigger when the mouse enters the spell icon on the screen
	 **/
    public void MouseEnter()
    {
        string description = StringHelper.DescriptionBuilder(Status, string.Join("", Status.Description));
        CursorBehaviour.DisplayTooltip(description);
    }

    /** MouseExit, public void Method
	 * This Method is launched with an event trigger when the mouse exits the spell icon on the screen
	 **/
    public void MouseExit()
    {
        CursorBehaviour.CancelTooltip();
    }
}
