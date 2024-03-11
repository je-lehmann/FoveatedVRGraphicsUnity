using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;


public class FPS : MonoBehaviour
{
   	public float fps;
	public float ms;
	public float dt = 0.0f;
	public int counter;
	public Animator anim;
	private DateTime start;
	private DateTime end;
	public bool active;
	public AnimationClip clip;

	public List<float> mshistory;

	public void Start(){

		for(int i = 0; i < 1000; i++){
			AnimationEvent evt;
	        evt = new AnimationEvent();
	        evt.time = i * 0.001f;
	        evt.functionName = "addSample";

        	clip.AddEvent(evt);
		}
        
	}

	public void Update()
	{
		if(active){
			/*
			float frameDuration = Time.unscaledDeltaTime;
			ms = frameDuration * 1000.0f; */
			dt += (Time.deltaTime - dt) * 0.1f; 
		
			ms = dt * 1000.0f;
			
		} 
	}

	public void addSample(){
		mshistory.Add(ms);
			counter++;
	}
	

	public void SaveFPS(){
			WriteString();
			active = false;
			counter = 0;
			anim.enabled = false;
			
	}

	public void toogleActive(){
		if(active)
			active = false;
		else
			active = true;
	}

	
   public void WriteString()
     {
         string path = Application.persistentDataPath + "/ms.txt";
 
         //Write some text to the test.txt file
         StreamWriter writer = new StreamWriter(path, true);

          foreach (float f in mshistory)
            {
                writer.WriteLine(f);
            }
         writer.Close();

         mshistory.Clear();
     }
}
