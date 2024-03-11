﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomLOD : MonoBehaviour
 {
     public float LodBiasFactor = 1F;
     float originalLodBias = 0F;
 
     private void OnPreCull()
     {
         originalLodBias = QualitySettings.lodBias;
         QualitySettings.lodBias = originalLodBias * LodBiasFactor;
     }
 
     private void OnPostRender()
     {
         QualitySettings.lodBias = originalLodBias;
     }
 }