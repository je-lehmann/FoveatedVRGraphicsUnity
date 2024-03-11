using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMatrix : MonoBehaviour
{
	public Matrix4x4 projection;
	public Matrix4x4 projectionL;
	public Matrix4x4 projectionR;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        projection = GetComponent<Camera>().projectionMatrix;
        projectionL = GetComponent<Camera>().GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
        projectionR = GetComponent<Camera>().GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
    }
}
