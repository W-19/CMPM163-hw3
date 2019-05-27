// A modified version of the provided TreeBend script that pulses the lights in reaction to audio

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour {

	public int partition;

	Renderer rend;
	void Start () {
		rend = GetComponent<Renderer> ();     
        rend.material.shader = Shader.Find("Custom/Phong + Outline");
	}
	

	void Update () {

		int numPartitions = 256; // partition the "histogram" into 10 segments
		float[] aveMag = new float[numPartitions];
		float partitionIndx = 0;
		int numDisplayedBins = 512 / 2; 

		for (int i = 0; i < numDisplayedBins; i++) 
		{
			if(i < numDisplayedBins * (partitionIndx + 1) / numPartitions){
				aveMag[(int)partitionIndx] += AudioPeer.spectrumData [i] / (512/numPartitions);
			}
			else{
				partitionIndx++;
				i--;
			}
		}

		
		for(int i = 0; i < numPartitions; i++)
		{
			aveMag[i] = (float)0.5 + aveMag[i]*100;
			if (aveMag[i] > 100) {
				aveMag[i] = 100;
			}
		}

		float currentValue = rend.material.GetFloat("_Emissiveness");
		float mag = Mathf.Clamp(-2.0f + aveMag[partition] * 2, 0, 1.5f);
		// Per-frame jumps in emmisiveness are limited to +- 0.2
		if(Math.Abs(currentValue - mag) > 0.15f){
			mag = currentValue > mag ? currentValue - 0.15f : currentValue + 0.15f;
		}

        rend.material.SetFloat("_Emissiveness", mag);


	}


}

