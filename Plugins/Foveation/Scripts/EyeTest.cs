using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;

[ExecuteInEditMode]
public class EyeTest : MonoBehaviour
{

public Camera main;	
public enum Side { Left, Right };

void Start(){
    main = GetComponent<Camera>();
}

void Update(){
		GetProjectionValues();
	}
 
public static Vector3 GetEyePosition(Side eye) {
 
       
 
            Vector3 posLeft;
            InputDevice device = InputDevices.GetDeviceAtXRNode(eye == Side.Left ? XRNode.LeftEye : XRNode.RightEye);
           
                if (device.TryGetFeatureValue(eye == Side.Left ? CommonUsages.leftEyePosition : CommonUsages.rightEyePosition, out posLeft)){
                    return posLeft;
                }
                else return new Vector3(9,1,1);
           
}
public void GetProjectionValues(){
      Matrix4x4 m_left = main.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
      Matrix4x4 m_right = main.GetStereoProjectionMatrix (Camera.StereoscopicEye.Right);
      Debug.Log("Left: ");
      Debug.Log(m_left);
       Debug.Log("Right : ");
      Debug.Log(m_right);
}
}