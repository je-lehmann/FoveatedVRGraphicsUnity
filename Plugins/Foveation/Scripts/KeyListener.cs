using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeyListener : MonoBehaviour
{
	public FoveationController ctrlR, ctrlL;
    public GameObject classicCam;
    public GameObject UI;
    private GameObject UIInstance;
    private bool locked;
    Scene scene;
   // public FPS fps;
    public Animator anim;


    void SwitchScenes(){
        scene = SceneManager.GetActiveScene();
        int i = scene.buildIndex;
        if(i == 2){
            i = -1;
        }
        SceneManager.LoadScene(i+1);
    }
    
    void Update()
    {
       // if(UIInstance != null)
            //UIInstance.gameObject.GetComponent<RectTransform>().rotation = Camera.main.transform.rotation;

        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }

         if(Input.GetKeyDown(KeyCode.F9)){
            
            if(anim.enabled){
            	anim.enabled = false;
            	//fps.enabled = false;
            	
            }
            else{
            	anim.enabled = true;
            	//fps.enabled = true;
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Tab)){
            SwitchScenes();
        }

        if(Input.GetKeyDown(KeyCode.F12)){
            classicCam.SetActive(true);
            ctrlL.gameObject.transform.parent.gameObject.SetActive(false);
            Debug.Log("Classic Rig active");
          	LogEvent("Classic Rig active");
        }
         if(Input.GetKeyDown(KeyCode.F11)){
            classicCam.SetActive(false);
            ctrlL.gameObject.transform.parent.gameObject.SetActive(true);
            Debug.Log("Foveated Rig active");
            LogEvent("Foveated Rig active");
        }

        if(Input.GetKeyDown(KeyCode.F1)){
            ctrlR.SetMode(FoveationController.Mode.Mouse);
            ctrlL.SetMode(FoveationController.Mode.Mouse);
            ctrlL.currentMode = FoveationController.Mode.Mouse;
            ctrlR.currentMode = FoveationController.Mode.Mouse;
            Debug.Log("Mouse");
            LogEvent("Mode: Mouse");
        }

        if(Input.GetKeyDown(KeyCode.F2)){
            ctrlR.SetMode(FoveationController.Mode.EyeTracked);
            ctrlL.SetMode(FoveationController.Mode.EyeTracked);
            ctrlL.currentMode = FoveationController.Mode.EyeTracked;
            ctrlR.currentMode = FoveationController.Mode.EyeTracked;
         
            Debug.Log("EyeTracked");
            LogEvent("Mode: EyeTracked");
        }

        if(Input.GetKeyDown(KeyCode.F3)){
            ctrlR.SetMode(FoveationController.Mode.Fixed);
            ctrlL.SetMode(FoveationController.Mode.Fixed);
            ctrlL.currentMode = FoveationController.Mode.Fixed;
            ctrlR.currentMode = FoveationController.Mode.Fixed;
            Debug.Log("Fixed");
            LogEvent("Mode: Fixed");   
        }

        if(Input.GetKeyDown(KeyCode.F4)){
            ctrlR.SetMode(FoveationController.Mode.Butterfly);
            ctrlL.SetMode(FoveationController.Mode.Butterfly);
            ctrlL.currentMode = FoveationController.Mode.Butterfly;
            ctrlR.currentMode = FoveationController.Mode.Butterfly;
            Debug.Log("Butterfly"); 
            LogEvent("Mode: Butterfly");   
        }

        if (Input.GetKeyDown(KeyCode.F5)){
        	if(ctrlR.applyPeriphericalContrast == false){
        		ctrlR.applyPeriphericalContrast = true;
				ctrlL.applyPeriphericalContrast = true;
                Debug.Log("Applied Contrast");
                LogEvent("Applied Contrast Preservation");  
        	}
        	else{
        		ctrlR.applyPeriphericalContrast = false;
				ctrlL.applyPeriphericalContrast = false;
                Debug.Log("Removed Contrast");
                LogEvent("Removed Contrast Preservation");    
        	}          
        }

        if (Input.GetKeyDown(KeyCode.F6)){
            if(ctrlR.applyfastBlur == false){
                ctrlR.applyfastBlur = true;
                ctrlL.applyfastBlur = true;
                Debug.Log("Applied Blur2");
                LogEvent("Applied Blur");  
            }
            else{
                ctrlR.applyfastBlur = false;
                ctrlL.applyfastBlur = false;
                Debug.Log("Removed Blur2");
                LogEvent("Removed Blur");  
            }  
        }

        if (Input.GetKeyDown(KeyCode.J)){
            ctrlR.reductionFactor = Mathf.Clamp(ctrlR.reductionFactor -= 0.2f, 1f, 8f);
            ctrlL.reductionFactor = Mathf.Clamp(ctrlL.reductionFactor -= 0.2f, 1f, 8f);
            SetNewTextures();
            Debug.Log("PERIPHERICAL REDUCTION: ");
            Debug.Log(ctrlL.reductionFactor);
            LogEvent("Peripherical Resolution Reduction:\n" + System.Math.Round(ctrlL.reductionFactor,2));  
        }

        if (Input.GetKeyDown(KeyCode.K)){
            ctrlR.reductionFactor = Mathf.Clamp(ctrlR.reductionFactor += 0.2f, 1f, 8f);
            ctrlL.reductionFactor = Mathf.Clamp(ctrlL.reductionFactor += 0.2f, 1f, 8f);
           SetNewTextures();
            Debug.Log("PERIPHERICAL REDUCTION: ");
            Debug.Log(ctrlL.reductionFactor); 
            LogEvent("Peripherical Resolution Reduction:\n" + System.Math.Round(ctrlL.reductionFactor,2));         
        }

        if (Input.GetKeyDown(KeyCode.N)){ 
            ctrlR.fovealReduction = Mathf.Clamp(ctrlR.fovealReduction -= 0.2f, 2f, 6f); 
            ctrlL.fovealReduction = Mathf.Clamp(ctrlL.fovealReduction -= 0.2f, 2f, 6f);
            SetNewTextures();
            Debug.Log("FOVEAL REDUCTION: ");
            Debug.Log(ctrlL.fovealReduction);
            LogEvent("Foveal Size Reduction:\n" + System.Math.Round(ctrlL.fovealReduction,2));           
        }

        if (Input.GetKeyDown(KeyCode.M)){
            ctrlR.fovealReduction = Mathf.Clamp(ctrlR.fovealReduction += 0.2f, 2f, 6f); 
            ctrlL.fovealReduction = Mathf.Clamp(ctrlL.fovealReduction += 0.2f, 2f, 6f);
            SetNewTextures();
            Debug.Log("FOVEAL REDUCTION: ");
            Debug.Log(ctrlL.fovealReduction); 
            LogEvent("Foveal Size Reduction:\n" + System.Math.Round(ctrlL.fovealReduction,2));       
        }
    }
    
    public void SetNewTextures(){
       ctrlL.UpdateRenderTextures();
       ctrlR.UpdateRenderTextures();
    }

    //UI Controll
    public void LogEvent(string s){
    	if(locked){
    		UIInstance.GetComponentInChildren<Text>().text = s;
    		return;
    	}
    	locked = true;
    	UIInstance = (GameObject) Instantiate(UI, ctrlL.transform.parent);
		StopCoroutine("ResetText");
        UIInstance.GetComponentInChildren<Text>().text = s;
        StartCoroutine("ResetText");
        
    } 

    IEnumerator ResetText(){
        yield return new WaitForSeconds(5f);
        UIInstance.GetComponentInChildren<Text>().text = "";
        Destroy(UIInstance);
        locked = false;
    } 

}
