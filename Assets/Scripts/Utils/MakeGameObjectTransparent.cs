using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGameObjectTransparent : MonoBehaviour {
    private const float m_TargetTransparancy = 0.2f;
    private const float m_FallOff = 0.1f; // returns to 100% in 0.1 sec

    private Shader m_OldShader;
    private Color m_OldColor;
    private float m_Transparency;
    private Renderer objectRenderer;
    private Color newColor;

    private void Start() {
        objectRenderer = GetComponent<Renderer>();
    }


    public void BeTransparent() {
        // reset the transparency;
        m_Transparency = m_TargetTransparancy;
        if (m_OldShader == null) {
            // Save the current shader
            m_OldShader = objectRenderer.material.shader;
            m_OldColor = objectRenderer.material.color;
            objectRenderer.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
        }
    }

    void FixedUpdate() {
        if (m_Transparency < 1.0f) {
            newColor = objectRenderer.material.color;
            newColor.a = m_Transparency;
            objectRenderer.material.color = newColor;
        }
        else {
            // Reset the shader
            objectRenderer.material.shader = m_OldShader;
            objectRenderer.material.color = m_OldColor;
            // And remove this script
            Destroy(this);
        }
        m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_FallOff;
    }
}
