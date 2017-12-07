using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIStatusDisplayer : MonoBehaviour
{

    public StatusBase Status;
    [SerializeField] private Image _StatusImage;
    [SerializeField] private Image _CDImage;
    [SerializeField] private Text _CDText;
    [SerializeField] private Outline _outline;

    private float _duration;

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

    public void ResetGUIStatus()
    {
        _duration = Status.Duration;
        _CDText.text = ((int)Status.Duration + 1).ToString();
    }

    public void DestroyGUIStatus()
    {
        _CDText.text = "";
        Destroy(gameObject);
    }

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
        Debug.Log(StringHelper.DescriptionBuilder(Status, string.Join("", Status.Description)));
    }

    /** MouseExit, public void Method
	 * This Method is launched with an event trigger when the mouse exits the spell icon on the screen
	 **/
    public void MouseExit()
    {
    }
}
