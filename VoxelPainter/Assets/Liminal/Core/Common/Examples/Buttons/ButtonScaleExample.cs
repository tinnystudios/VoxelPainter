using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScaleExample : BaseButton {

    public override void OnButtonUp()
    {
        base.OnButtonUp();
        transform.localScale = transform.localScale * 1.2F;
    }

}
