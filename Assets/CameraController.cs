using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Board target;
    private Camera cam;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 camPoint = cam.ViewportToWorldPoint(Vector3.zero);
        Vector3 boardPoint = target.transform.position - new Vector3(0.5f, 0.5f) -Vector3.up;
        Vector3 offset = camPoint - boardPoint;
        cam.transform.position -= offset + Vector3.forward * 10;
	}
}
