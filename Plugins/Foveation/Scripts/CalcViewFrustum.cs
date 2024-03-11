using UnityEngine;

public class CalcViewFrustum : MonoBehaviour
{
	public float frustumHeight;
    void Start()
    {
        var camera = GetComponent<Camera>();
        Vector3[] frustumCorners = new Vector3[4];
        camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCorners);

        for (int i = 0; i < 2; i++)
        {
            frustumHeight = (frustumCorners[1]-frustumCorners[0]).y;
        }

    }
}