using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonStyleExample : BaseButtonStyle {

    public MeshRenderer meshRenderer;

    public Color hoverColor;
    public Color normalColor;
    public Color selectedColor;

    public override void OnButtonDown(ButtonHitInfo hitInfo)
    {
        base.OnButtonDown(hitInfo);
        meshRenderer.material.color = selectedColor;
    }

    public override void OnButtonUp()
    {
        base.OnButtonUp();
        meshRenderer.material.color = normalColor;
    }

    public override void OnButtonHoverDown()
    {
        base.OnButtonHoverDown();
        meshRenderer.material.color = hoverColor;
    }

    public override void OnButtonHoverUp()
    {
        base.OnButtonHoverUp();
        meshRenderer.material.color = normalColor;
    }

}
