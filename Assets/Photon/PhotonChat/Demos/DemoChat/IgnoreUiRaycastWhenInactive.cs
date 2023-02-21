using UnityEngine;


public class IgnoreUiRaycastWhenInactive : MonoBehaviour
{
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return gameObject.activeInHierarchy;
    }
}