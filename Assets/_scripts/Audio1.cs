using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
[RequireComponent (typeof(AudioSource))]
public class Audio1 : MonoBehaviour
{
	AudioSource _audiosource;

	//for taking the microphone input
	public AudioClip _audioClip;
	public bool _useMicroPhone;
	public string _selectedDevice;

	//FFT valued

	public static float[] _samples =new float[512];		//creating  a circle with 512 sample objects as cubes
	public static float[] _freqBand =new float[8];		//creating 8 frequency bands
	public static float[] _bandbuffer =new float[8];
	float[] _bufferDecrease = new float[8];

    // Start is called before the first frame update
    void Start()
    {
    	_audiosource =GetComponent<AudioSource>();   //to obtain the audio source

     if(_useMicroPhone)
     {
     	if(Microphone.devices.Length > 0)
     	{
     		_selectedDevice = Microphone.devices[0].ToString();
     		_audiosource.clip = Microphone.Start(_selectedDevice,true,10, AudioSettings.outputSampleRate);
     	}
     	else
     	{
     		_useMicroPhone=false;
     	}
     }
     if(!_useMicroPhone)
     {
     	 _audiosource.clip = _audioClip;
     }
    }

    // Update is called once per frame
    void Update()
    {
        if(_audiosource.clip != null)
        {
        GetSpectrumAudioSource();		//invoking the function once per frame
        MakeFrequencyBands();
        BandBuffer();
    	}
    }
    void GetSpectrumAudioSource()
    {
    	_audiosource.GetSpectrumData(_samples,0,FFTWindow.Blackman);	//to obtain the audio spectrum data by using the Fast Fourier Transform
    }
    void BandBuffer()
    {
    	for(int i=0;i<8;i++)
    	{
    		if(_freqBand[i] > _bandbuffer[i])
    		{
    			_bandbuffer[i] = _freqBand[i];
    			_bufferDecrease[i] = 0.005f;
    		}
    		if(_freqBand[i] < _bandbuffer[i])
    		{
    			_bandbuffer[i] -= _bufferDecrease[i];
    			_bufferDecrease[i] *= 1.2f ;
    		}
    	}
    }

     void MakeFrequencyBands(){			//To create 8 frequency bands
       /*
    	* 22050 / 512 =43 hertz per  sample
    	* 7 frequency bands divide over 8 
    	*
    	1. SUB BASS= 20 TO 60 HZ
		2. BASS : 60 TO 250 HZ
		3. LOW MIDRANGE : 250 TO 500 HZ 
		4. MIDRANGE : 500 TO 2000 HZ
		5. UPPER MID RANGE: 2K TO 4K HZ
		6. PRESENCE :  4K TO 6K HZ
		7. BRILLIANCE :6K TO 20K HZ

    	0 - 2 	= 86Hz
    	1 - 4 	= 172Hz - 	87-258 Hz
    	2 - 8 	= 344Hz - 	259-602Hz
    	3 - 16 	= 688Hz - 	603-1290Hz
    	4 - 32 	= 1376 - 	1291-2666Hz
    	5 - 64 	= 2752 - 	2667-5418Hz
    	6 - 128 = 5504Hz -	5419-10922Hz
    	7 - 256 = 11080Hz - 10923-21930Hz
		Total =510
    	*
    	*/

    	int count =0;
    	
    	for(int i=0;i<8;i++){			//Even numbers in for loop
    		float average = 0;
    		int sampleCount = (int)Mathf.Pow(2,i) * 2;	//Create powers of 2 to get frequency bands

    			if(i==7){
    				sampleCount += 2;	//To cover clear 512 bands
    			}
    			for(int j=0;j<sampleCount;j++){
    					average += _samples[count] * (count + 1);	
    					count++;
    			}
    			average/= count;
    			_freqBand[i]= average *10;								//Apply average to frequency bands
    	}
    }
}
