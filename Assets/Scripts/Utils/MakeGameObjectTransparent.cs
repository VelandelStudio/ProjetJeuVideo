using UnityEngine;

public class MakeGameObjectTransparent : MonoBehaviour {
    private const float m_TargetTransparancy = 0.2f;
    private const float m_TimeToFallOffInSec = 0.1f;
    
    private Shader m_OldShader;
    private Color m_OldColor;
    private float m_Transparency;
    private Renderer objectRenderer;
    private Color newColor;

    /** Start Method
     * The start method is used to get the Renderer instance of the gameObject attached.
     **/
    private void Start() {
        objectRenderer = GetComponent<Renderer>();
    }
    
    /** FixedUpdate Method
     * This method is used in two different ways. 
     * If the gameObject is already tranparent (i.e. if the BeTransparent method is in use) it applies th fading color to the gameObject.
     * If it's not, it set the gameObject to its initial state.
     * Every frame, the item is becoming more and more opaque. In this way, if the BeTransparent is continuously launched, the gameobject will always be transparent.
     * When the BeTransparent method is stopped, the object gradually returns to its initial state.
     * When the initial state is reached, this script is destroyed. 
     * Warning, you should always use this in the FixedUpdate method. The Update methode is launched at various time and make the gameobject tickelling.
     **/
    private void FixedUpdate() {
        if (m_Transparency < 1.0f) {
            newColor = objectRenderer.material.color;
            newColor.a = m_Transparency;
            objectRenderer.material.color = newColor;
        }
        else {
            objectRenderer.material.shader = m_OldShader;
            objectRenderer.material.color = m_OldColor;
            Destroy(this);
        }
        m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_TimeToFallOffInSec;
    }
    
    /** BeTransparent Method
     * This public method is used to save the initial state of the gameObject before it is set to transparent.
     * Then it applies the transparent Shader to the gameObject.
     **/
    public void BeTransparent() {
        m_Transparency = m_TargetTransparancy;
        if (m_OldShader == null) {
            m_OldShader = objectRenderer.material.shader;
            m_OldColor = objectRenderer.material.color;
            objectRenderer.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
        }
    }
}
