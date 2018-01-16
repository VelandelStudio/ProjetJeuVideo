using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentStatus : StatusBase
{

    private const float _targetTransparancy = 0.2f;
    private Shader _oldShader;
    private Color _oldColor;

    private Renderer _objectRenderer;
    private Color _newColor;
    private float _transparency;


    private void Awake()
    {
        PreWarm();
    }
    public override void OnStatusApplied()
    {
        _objectRenderer = GetComponentInParent<Renderer>();
        if (_objectRenderer == null
        || _objectRenderer.material == null
        || _objectRenderer.material.color == null)
        {
            base.DestroyStatus();
        }

        if (_oldShader == null)
        {
            _transparency = _targetTransparancy;
            _oldShader = _objectRenderer.material.shader;
            _oldColor = _objectRenderer.material.color;
            _objectRenderer.material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");

            _newColor = _objectRenderer.material.color;
            _newColor.a = _transparency;
            _objectRenderer.material.color = _newColor;
        }
    }

    public override void StatusTickBehaviour() { }

    public override void DestroyStatus()
    {
        _objectRenderer.material.shader = _oldShader;
        _objectRenderer.material.color = _oldColor;
        base.DestroyStatus();
    }
}