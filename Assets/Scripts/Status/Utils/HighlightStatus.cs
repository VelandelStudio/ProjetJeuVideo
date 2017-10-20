using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightStatus : StatusBase
{
    private Shader _oldShader;
    private Renderer _objectRenderer;

    public override void OnStatusApplied()
    {
        isTickable = false;
        maxDuration = 0.05f;
        tickInterval = 0f;
        delay = 0f;

        _objectRenderer = GetComponentInParent<Renderer>();
        if (_objectRenderer == null)
            base.DestroyStatus();

        if (_oldShader == null)
        {
            _oldShader = _objectRenderer.material.shader;
            _objectRenderer.material.shader = Shader.Find("Outlined/Diffuse");
        }
    }

    public override void StatusTickBehaviour() { }

    public override void DestroyStatus()
    {
        _objectRenderer.material.shader = _oldShader;
        base.DestroyStatus();
    }
}