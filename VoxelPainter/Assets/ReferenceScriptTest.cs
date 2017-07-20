using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ReferenceScriptTest : MonoBehaviour {

    public float curTime;

    void Awake() {
        StartCoroutine(_LerpPosition(result => curTime = result));
    }
    
    IEnumerator _LerpPosition(Action<float> myVariableResult) {
        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            myVariableResult(i);
            yield return null;
        }
    }

}
