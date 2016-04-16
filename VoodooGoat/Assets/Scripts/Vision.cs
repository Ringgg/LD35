using UnityEngine;

public class Vision : MonoBehaviour
{
    public static RaycastHit hitInfo;

    public static bool CanSee(Transform t1, Transform t2, float range)
    {
        if (!Physics.Raycast(new Ray(t1.position, t2.position - t1.position),
            out hitInfo,
            range))
            return false;
        return hitInfo.collider.transform == t2;
    }
}
