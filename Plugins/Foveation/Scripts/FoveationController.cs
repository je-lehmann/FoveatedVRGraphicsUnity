using UnityEngine;
using System.Collections;
using PupilLabs;
using UnityEngine.XR;

public class FoveationController : MonoBehaviour
{   
    //RenderTexures
    private RenderTexture fovealTexture, periphericalTexture, target;
    
    //Cameras
    private Camera peripherical, foveal, main;

    //FOV & Perspective Helpers
    private float periphericalFrustrumHeight, OffsetX, OffsetY, width, height; 
    private Rect r;
    private float eyeDistance; //represents IPD
    private Matrix4x4 m_left, m_right; //represents hardware specific Projection Matrix
    private Vector2 screenFocusPosition; //represents projected Focuspoint
    private Vector2 focusPoint, fixedLeft, fixedRight; //used for fixed foveated Rendering

    //Mode Control
    public bool leftEye;
    private bool gazeControlledbyMouse, gazeSimulated;
    public enum Mode { Fixed, Mouse, EyeTracked, Butterfly };
    public Mode currentMode;
    private Mode tmpMode;
   
    //User Input
    [Header("User Input")]
    public float reductionFactor = 2f; //= periphericalReductionFactor
    public float fovealReduction = 2f; //= fovealSizeReduction
    
    public bool applyPeriphericalContrast = true;  //= applyContrast
    public bool applyfastBlur = false; //= applyBlur
    public bool reduceRenderTextures; //= can be used to use only one peripherical Render Texture 

    //Gaze Object for simulated Foveation
    [HideInInspector]
    public Transform butterfly; 

    //Shader Material
    [HideInInspector]
    public Material Blend;

    //Eye Tracking
    [HideInInspector]
    public Transform gazePosition;
    [HideInInspector]
    public SubscriptionsController subsCtrl;
    [HideInInspector]
    public CalibrationController calibrationController;
    [HideInInspector]
    public bool trackingReady,eyeTrackingActive = false;
    [HideInInspector] 
    public GameObject Text;
 
    //validate User input to only allow certain foveation values
    void OnValidate(){
        reductionFactor = Mathf.Clamp(reductionFactor, 1f, 8f);
        fovealReduction = Mathf.Clamp(fovealReduction, 2f, 6f); 
    }

    //initialize Render Textures and pass Shader Parameters
    void Start(){
    	
        Debug.Log(XRDevice.model);
        height =  XRSettings.eyeTextureHeight;
        width = XRSettings.eyeTextureWidth;
        
        //Initialize Cameras
        peripherical = this.gameObject.transform.GetChild(0).GetComponent<Camera>();
        foveal = this.gameObject.transform.GetChild(1).GetComponent<Camera>();
        main = GetComponent<Camera>();  
        CalcViewFrustum();
        
        //set Projectionmatrix of parent object
        m_left = main.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
        m_right = main.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
           
        //Initialize Textures with correct width and height and bind them to cameras
        fovealTexture = new RenderTexture((int)(width/fovealReduction), (int)(height/fovealReduction), 16, RenderTextureFormat.ARGB32); 
        foveal.targetTexture = fovealTexture;
         
        if (reduceRenderTextures && leftEye){
            periphericalTexture = new RenderTexture((int)(width/(reductionFactor)), (int)(height/(reductionFactor)), 16, RenderTextureFormat.ARGB32); 
            peripherical.targetTexture = periphericalTexture;
        }
        if(!reduceRenderTextures){
            periphericalTexture = new RenderTexture((int)(width/(reductionFactor)), (int)(height/(reductionFactor)), 16, RenderTextureFormat.ARGB32); 
            peripherical.targetTexture = periphericalTexture;

	        if(leftEye){ 
	            peripherical.projectionMatrix = m_left;
	            screenFocusPosition = fixedLeft; 
	        }
	        else{
	            peripherical.projectionMatrix = m_right;
	            screenFocusPosition = fixedRight; 
	        } 
        }
        //Target will be overwritten with the final blended result
        target = new RenderTexture((int)width, (int)height, 16, RenderTextureFormat.ARGB32);
        Shader.SetGlobalFloat("_iResolutionX", width);
        Shader.SetGlobalFloat("_iResolutionY", height);

        SetMode(currentMode);
        
    }

    //Update Focuspoint, Contrast, Blur & Render Textures
    void Update(){
        //calculate Stereo Separation between users eyes to offset camera origins and blend masks
        eyeDistance = Vector3.Distance(InputTracking.GetLocalPosition(XRNode.LeftEye), InputTracking.GetLocalPosition(XRNode.RightEye));
        //represents stereo masking for a focuspoint with infinite distance in the middle of the eyetextures
        fixedLeft = new Vector2 (width/2 + (eyeDistance/2) * width, height/2);
        fixedRight = new Vector2 (width/2 - (eyeDistance/2) * width, height/2);
        //apply stereo offset to camera nodes  
        if(leftEye){
            peripherical.transform.localPosition = new Vector3(-eyeDistance/2f, 0f, 0f);
            foveal.transform.localPosition = new Vector3(-eyeDistance/2f, 0f, 0f);
        }    
        else{
            peripherical.transform.localPosition = new Vector3(eyeDistance/2f, 0f, 0f);
            foveal.transform.localPosition = new Vector3(eyeDistance/2f, 0f, 0f);
        }

        if(currentMode != tmpMode){
            SetMode(currentMode);
            tmpMode = currentMode;
        }

        //Calculates the Foveation Matrix
        SetMatrix();

        //Focus Control according to selected Mode:
        // Eye Tracking before Gaze Mapping finished/ Fixed Foveation
        if(!gazeControlledbyMouse && !eyeTrackingActive && !gazeSimulated){  
        	FocusOnInfinity(); 
        }
        // Eye Tracking
        if(!gazeControlledbyMouse && eyeTrackingActive && !gazeSimulated && trackingReady){
        
            if(leftEye){
                Vector3 screenPos = main.WorldToScreenPoint(gazePosition.position, Camera.MonoOrStereoscopicEye.Left);
                screenFocusPosition = new Vector2(screenPos.x, screenPos.y);
               
            }
            else{
                Vector3 screenPos = main.WorldToScreenPoint(gazePosition.position, Camera.MonoOrStereoscopicEye.Right);
                screenFocusPosition = new Vector2(screenPos.x, screenPos.y);
            } 
                            
        }
        // Gaze controlled by mouse position
        if(gazeControlledbyMouse && !eyeTrackingActive && !gazeSimulated){
        
        	if(leftEye){
            screenFocusPosition = new Vector2(
                Mathf.Clamp(Input.mousePosition.x + (eyeDistance/2) * width, 0, width),
                Mathf.Clamp(Input.mousePosition.y, 0, height));
        	}
        	else{
        	screenFocusPosition = new Vector2(
                Mathf.Clamp(Input.mousePosition.x - (eyeDistance/2) * width, 0, width),
                Mathf.Clamp(Input.mousePosition.y, 0, height));
        	}
        }
        // Gaze controlled by GazeTarget, for example a Butterfly
        if(!gazeControlledbyMouse && !eyeTrackingActive && gazeSimulated){
            
            if(leftEye){
                Vector3 screenPos = main.WorldToScreenPoint(butterfly.position, Camera.MonoOrStereoscopicEye.Left);
                screenFocusPosition = new Vector2(screenPos.x, screenPos.y);
               
            }
            else{
                Vector3 screenPos = main.WorldToScreenPoint(butterfly.position, Camera.MonoOrStereoscopicEye.Right);
                screenFocusPosition = new Vector2(screenPos.x, screenPos.y);
            } 
        }
        
    	//Set focuspoints for left and right eye for the blend mask
        Blend.SetFloat("_eyeX", focusPoint.x/width);
        Blend.SetFloat("_eyeY", focusPoint.y/height);
        //Set shader foveation parameters
        //Shader.SetGlobalFloat("_reductionFactor", reductionFactor); 
        Shader.SetGlobalFloat("_fovealReduction", fovealReduction);

        //Contrast and Blur have been replaced by Unity Implementation
     /*   if(applyPeriphericalContrast)
            Shader.SetGlobalInt("_applyPeriphericalContrast", 1); 
        else
            Shader.SetGlobalInt("_applyPeriphericalContrast", 0); */

        if(applyfastBlur)
            peripherical.GetComponent<BlurOptimized>().enabled = true;
        else
            peripherical.GetComponent<BlurOptimized>().enabled = false; 

        if(applyPeriphericalContrast)
            peripherical.GetComponent<ContrastEnhance>().enabled = true;
        else
            peripherical.GetComponent<ContrastEnhance>().enabled = false; 
                
        //Set resolution & textures for blend shader
       // Shader.SetGlobalFloat("_iResolutionX", width);
       // Shader.SetGlobalFloat("_iResolutionY", height);
        Blend.SetTexture("_FovealTex", fovealTexture);

        //Set Textures for Shader
        if (!reduceRenderTextures){
            Blend.SetTexture("_PeriphericalTex", periphericalTexture);
        }
        else if (reduceRenderTextures && leftEye){
            Blend.SetTexture("_PeriphericalTex", periphericalTexture);
        }
        else if (reduceRenderTextures && !leftEye){
            Blend.SetTexture("_PeriphericalTex", this.gameObject.transform.parent.GetChild(0).GetComponent<FoveationController>().periphericalTexture);
        }
    }
    public void UpdateRenderTextures(){
        //allows for Resolution Changes on Runtime
        //replaces the rendertextures with new foveation values
        fovealTexture = new RenderTexture((int)(width/fovealReduction), (int)(height/fovealReduction), 16, RenderTextureFormat.ARGB32); // controll reductionFactorial size
        foveal.targetTexture = fovealTexture;
         
         if (reduceRenderTextures && leftEye){
            periphericalTexture = new RenderTexture((int)(width/(reductionFactor)), (int)(height/(reductionFactor)), 16, RenderTextureFormat.ARGB32); //New Resolution can be used instead of dynamic scaling
            peripherical.targetTexture = periphericalTexture;
        } 
        if(!reduceRenderTextures){
            periphericalTexture = new RenderTexture((int)(width/(reductionFactor)), (int)(height/(reductionFactor)), 16, RenderTextureFormat.ARGB32); //New Resolution can be used instead of dynamic scaling
            peripherical.targetTexture = periphericalTexture;
                
            if(leftEye){ 
                peripherical.projectionMatrix = m_left;
            }
            else{
                peripherical.projectionMatrix = m_right;
            } 
        }    
    }

    //After Rendering the Rendertextures can be drawn to screen with blending material
    void OnPostRender(){
        GL.LoadPixelMatrix(0, width, height,0);
        Graphics.DrawTexture(new Rect(0, 0, width, height), target, Blend);
    }

    //Set Foveation Mode 
    public void SetMode(Mode mode){
        switch (mode){
              case Mode.Fixed: // Fixed Foveation
                  eyeTrackingActive = false;
                  gazeControlledbyMouse = false;
                  FocusOnInfinity(); 
                  gazeSimulated = false;
                  butterfly.gameObject.SetActive(false);
                  Text.SetActive(false);
                  break;
              case Mode.Mouse: // Manual Foveation
                  eyeTrackingActive = false;
                  gazeControlledbyMouse = true;
                  gazeSimulated = false;
                  butterfly.gameObject.SetActive(false);
                  Text.SetActive(false);
                  break;
              case Mode.EyeTracked: // Eye Foveation
              	  if(!trackingReady){
              	  	FocusOnInfinity(); 
              	  }
              	  reductionFactor = 3f;
              	  fovealReduction = 2f;
              	  UpdateRenderTextures();
                  eyeTrackingActive = true;
                  gazeControlledbyMouse = false;
                  gazeSimulated = false;
                  butterfly.gameObject.SetActive(false);
                  Text.SetActive(true);
                  break;
              case Mode.Butterfly: // Simulated Eye Foveation
                  butterfly.gameObject.SetActive(true);
                  eyeTrackingActive = false;
                  gazeControlledbyMouse = false;
                  gazeSimulated = true;
                  Text.SetActive(false);
                  break;
        }
    }

    public void FocusOnInfinity(){
    	if(leftEye)
                screenFocusPosition = fixedLeft;
            else
                screenFocusPosition = fixedRight;
    }   
    //Perspective Calculations 
    //https://docs.unity3d.com/ScriptReference/Camera.CalculateFrustumCorners.html
    //used to calculate frustum height
    public void CalcViewFrustum(){
        Vector3[] frustumCorners = new Vector3[4];
        if(leftEye){
            main.CalculateFrustumCorners(new Rect(0, 0, 1, 1), main.farClipPlane, Camera.MonoOrStereoscopicEye.Left, frustumCorners);
        }
        else{
            main.CalculateFrustumCorners(new Rect(0, 0, 1, 1), main.farClipPlane, Camera.MonoOrStereoscopicEye.Right, frustumCorners);
        }

        periphericalFrustrumHeight = (frustumCorners[1]-frustumCorners[0]).y;
    }

    public float CalcFOV(){
        float fovealFrustumHeight = ((-r.height/height)) * periphericalFrustrumHeight; // r.height / height = 1/fovealReduction (3.5) 
        float calcFOV = 2.0f * Mathf.Atan(fovealFrustumHeight * 0.5f / 1000) * Mathf.Rad2Deg; //distance entspricht far (3.4)
        return calcFOV; 
    }

    //adjust foveal Projectionmatrices to match peripherical asynchronous OpenVR Matrices
    public float CalcStereoOffset(){
        if(leftEye)
           return m_left[0,2] * fovealReduction;
        else
            return m_right[0,2] * fovealReduction;
    }

    public float CalcYOffset(){
        if(leftEye)
           return m_left[1,2] * fovealReduction;
        else
            return m_right[1,2] * fovealReduction;
    } 

      public void SetMatrix(){
        //span a rect in screenkoords that will contain the foveated Texture
        OffsetX = (screenFocusPosition.x - (width/2)); 
        OffsetY = (screenFocusPosition.y - (height/2));
        focusPoint = screenFocusPosition;
        r = new Rect((focusPoint.x + ((fovealReduction-1)*OffsetX)) - (width / (fovealReduction)), 
                     (focusPoint.y + ((fovealReduction-1)*OffsetY)) + (height / (fovealReduction)), 
                     (width / fovealReduction),
                     -(height / fovealReduction));

        //rect coords normalized for view Space
        Vector2 topleft = new Vector2(r.xMin,r.yMin);
        Vector2 bottomright = new Vector2(r.xMax,r.yMax);
        Vector2 center = new Vector2(topleft.x + width/(fovealReduction), topleft.y - height/(fovealReduction));
        center = new Vector2 ((center.x / width), (center.y / height));
    
        //construct nearPlane in NDC
        float left = center.x - 1f; //-0.5f in center
        float right = center.x; // 0.5f
        float top = center.y; // 0.5f
        float bottom = center.y - 1f; //-0.5f
 
        Matrix4x4 m =   Matrix4x4.Frustum(left, right, bottom, top, 0.3f, 1000f);
        //apply calculated FOV and Stereo Parameters to match OpenVR asynchronous Projection Matrix
        float fov  = CalcFOV();
        m[1, 1] = 1f / Mathf.Tan( fov / ( 2f * Mathf.Rad2Deg ) ); //(3.7)
        m[0, 0] = m[1,1] / ((float)(width) / (float)(height));
        m[0, 2] += CalcStereoOffset();
        m[1, 2] += CalcYOffset(); 
        foveal.projectionMatrix = m;
    }

    //Listen for Calibration and Connection Changes
    void OnEnable(){
        subsCtrl.requestCtrl.OnConnected += OnConnected;
        calibrationController.OnCalibrationSucceeded += CalibrationSucceeded;
        calibrationController.OnCalibrationFailed += CalibrationFailed;
    }

    void OnDisable(){
        subsCtrl.requestCtrl.OnConnected -= OnConnected;
        calibrationController.OnCalibrationSucceeded -= CalibrationSucceeded;
        calibrationController.OnCalibrationFailed -= CalibrationFailed;
    }

    private void OnConnected(){
    	SetMode(Mode.EyeTracked);
        trackingReady = false;
    }

    private void CalibrationSucceeded(){
        trackingReady = true;
    }
    
    private void CalibrationFailed(){
    	SetMode(Mode.Fixed);
        trackingReady = false;
    }  
}

