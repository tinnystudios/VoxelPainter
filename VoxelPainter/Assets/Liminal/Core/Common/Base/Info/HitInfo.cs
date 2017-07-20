using UnityEngine;

[System.Serializable]
public class ButtonHitInfo
{
    public Transform initiator; //The Transform that hit button
    public RaycastHit hit; //The raycast that hit the button
}