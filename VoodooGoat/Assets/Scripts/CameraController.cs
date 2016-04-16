using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;

	void Update () {
        //transform.LookAt(target);
        transform.position = target.position + new Vector3(0,10,-6.5f);
	}
}
