using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/** GUIStatusDisplayer, public class
 * @Implements IDisplayer 
 * Displayer that handles informations when the player receive a Status. 
 **/
public class GUIStatusDisplayer : MonoBehaviour, IDisplayer
{
    private StatusBase _status;
    [SerializeField] private Image _StatusImage;
    [SerializeField] private Image _CDImage;
    [SerializeField] private Text _CDText;
    [SerializeField] private Outline _outline;

    private float _duration;

    public IDisplayable Displayable
    {
        get { return _status; }
        protected set { }
    }
    /** AttributeDisplayable, public void method
     * @param : IDisplayable
     * This method is launched by the StatusBase when the Status is Applied.
     * We set everything we need to display informations of the Status (passed as a parameter) on the screen.
     * If the StatusBase implements IBuff then the outline is green, else, it is red.
     **/
    public void AttributeDisplayable(IDisplayable displayable)
    {
        _status = (StatusBase)displayable;
        _duration = _status.Duration;
        _CDImage.fillAmount = 0;
        _CDText.text = ((int)_status.Duration + 1).ToString();
        if (_status is IBuff)
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
        _duration = _status.Duration;
        _CDText.text = ((int)_status.Duration + 1).ToString();
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
        if (_status == null)
        {
            return;
        }

        _duration -= Time.deltaTime;
        _CDText.text = ((int)_duration + 1).ToString();
        _CDImage.fillAmount = 1 - _duration / _status.Duration;
    }

    /** MouseEnter, public void Method
	 * This Method is launched with an event trigger when the mouse enters the spell icon on the screen
	 **/
    public void MouseEnter()
    {
        CursorBehaviour.DisplayTooltip(Displayable);
    }

    /** MouseExit, public void Method
	 * This Method is launched with an event trigger when the mouse exits the spell icon on the screen
	 **/
    public void MouseExit()
    {
        CursorBehaviour.CancelTooltip();
    }
}