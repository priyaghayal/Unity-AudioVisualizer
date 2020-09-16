using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512Cubes : MonoBehaviour
{
	public GameObject _sampleCubePrefab;		    		//To instantiate prefab
	GameObject[] _sampleCube =new GameObject[512];			//Create trray of game object
	public float _maxScale;									//To make frequecy more

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0;i<512;i++){
        	GameObject _instanceSampleCube = (GameObject)Instantiate (_sampleCubePrefab);//Cubes changing with spectrum data type casted
        	_instanceSampleCube.transform.position = this.transform.position;			//Center position for cube object 
        	_instanceSampleCube.transform.parent = this.transform;						//Cube will be child of the object in which class is running
        	_instanceSampleCube.name = "sampleCube" + i;								//Change name of instance of game object
        	this.transform.eulerAngles = new Vector3(0,-0.703125f*i,0);					//Create circle of cubes:Cube angle(for rotation in y axis)=360/512=0.703125
        	_instanceSampleCube.transform.position=Vector3.forward*100;					//Transform position 
        	_sampleCube [i] = _instanceSampleCube;										//Set samplecubes in correct position in array
        }
    }

    // Update is called once per frame
    void Update()
    {
        float half;
        for(int i=0;i<512;i++){
            half = (Audio1._samples[i]* _maxScale) + 2;
        	if(_sampleCube!=null){
        		_sampleCube[i].transform.localScale = new Vector3(10,half,10);
        	}																			//Samples of the array 

        }
    }
}
