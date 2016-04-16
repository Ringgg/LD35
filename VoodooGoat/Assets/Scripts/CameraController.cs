using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;

	// Update is called once per frame
	void Update () {
        transform.LookAt(target);
        transform.position = target.position + new Vector3(0,5,-2);
	}
}
