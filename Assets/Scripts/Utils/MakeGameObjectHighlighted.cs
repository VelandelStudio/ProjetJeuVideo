using UnityEngine;


/** MakeGameObjectHighlighted class
 * This script should only be ato-attached by another script on GameObjects.
 * It allows gameObject to be highlighted during 0.2 sec after that, this script is destroyed.
 * In order to properly use this script, you should always launche the method BeHighLighted from another script. In this way, the gameObject will stay Highlighted.
 * Please note that if the gameObject associated does not have a Renderer, this script does NOTHING.
 **/
public class MakeGameObjectHighlighted : MonoBehaviour
{
    private float m_TimeToFallOffInSec = 0.01f;

    private Shader m_OldShader;
    private Renderer objectRenderer;
    /** Start Method
     * The start method is used to get the Renderer instance of the gameObject attached and then launch the first BeHighLighted Method
     **/
    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        BeHighLighted();
    }

    /** FixedUpdate Method
     * This method is used to check if the script should be destroyed or not. 
     * Every FixedUpdate (0.2sec), the TimeToFalleOff is reduced by fixedDeltaTime (0.2f). If the falloff reaches 0, the script is destroyed.
     * The BeHighLighted, fired by another script, ensure that the m_TimeToFallOffInSec does not reach 0.
     **/
    private void FixedUpdate()
    {
        if (objectRenderer == null)
            return;

        if (m_TimeToFallOffInSec <= 0)
        {
            objectRenderer.material.shader = m_OldShader;
            Destroy(this);
        }
        m_TimeToFallOffInSec -= Time.fixedDeltaTime;
    }

    /** BeHighLighted Method
     * This public method is used to save the initial state of the gameObject before it is set to highlighted.
     * Then it applies the highlight Shader to the gameObject. 
     * It also set the count of m_TimeToFallOffInSec by fixedDeltaTime.
     **/
    public void BeHighLighted()
    {
        if (objectRenderer == null)
            return;

        if (m_OldShader == null)
        {
            m_OldShader = objectRenderer.material.shader;
            objectRenderer.material.shader = Shader.Find("Outlined/Diffuse");
        }
        m_TimeToFallOffInSec = Time.fixedDeltaTime;
    }
}
