using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour 
{
 
	
	
	
	
	
	
	
	
	
	

	public  float updateInterval = 0.5F;

	private float accum   = 0; 
	private int   frames  = 0; 
	private float timeleft; 
	private Text FPS_Text;

	void Start()
	{
	timeleft = updateInterval;  
	FPS_Text = transform.GetComponent<Text>();
	}

	void Update()
	{
	timeleft -= Time.deltaTime;
	accum += Time.timeScale/Time.deltaTime;
	++frames;

	
	if( timeleft <= 0.0 )
	{
		    
		float fps = accum/frames;
		string format = System.String.Format("{0:F2} FPS",fps);
				FPS_Text.text = format;

		if(fps < 25)
					FPS_Text.color = Color.yellow;
		else 
			if(fps < 15)
						FPS_Text.color = Color.red;
			else
					FPS_Text.color = Color.green;

		    timeleft = updateInterval;
		    accum = 0.0F;
		    frames = 0;
	}
	}
}