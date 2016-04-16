using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
    public Vector3 offset = new Vector3(0, 5, 0);

	void Update ()
    {
        if (target != null)
            transform.position = target.position + offset;
	}
}
