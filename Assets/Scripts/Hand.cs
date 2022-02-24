using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
  public Climber climber = null;
  public OVRInput.Controller controller = OVRInput.Controller.None;
  public Vector3 Delta{private set; get;} = Vector3.zero;
  public Control control;

  private Vector3 lastPosition = Vector3.zero;
  private GameObject currentPoint = null;
  private List<GameObject> contactPoints = new List<GameObject>();
  private GameObject bar;

  private GameObject closest; 
  private GameObject[] climbPoint;

  private void Awake(){

    bar = GameObject.Find("Bar");
  }

  private void Start(){

  	lastPosition = transform.position;
    // Debug.Log(bar.GetComponent<loadingbar>().imageComp.fillAmount);
  }

  private void Update(){

    // Controller version
  	if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller)){
  		GrabPoint();
  	}

  	if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller)){
  		ReleasePoint();
  	}

    if (bar.GetComponent<loadingbar>().grab == false){
       ReleasePoint();
       // add ems here
       StartCoroutine(control.FallCoroutine());

       // bar.GetComponent<loadingbar>().addValue();
    }

    climbPoint = GameObject.FindGameObjectsWithTag("ClimbPoint");  
    closest = GetClosestPoint(climbPoint);

    if (this.name == "LeftHandAnchor"){

      float L_Dist = Vector3.Distance(this.transform.position, closest.transform.position);
      Debug.Log(closest);
      Debug.Log(L_Dist);
                
      if (L_Dist < 0.25){
          GrabPoint();
          // control.LockFinger(0);
          // control.LockFinger(1);
          // control.LockFinger(2);
          // control.LockFinger(3);
          // control.LockFinger(4);

        } else{
          ReleasePoint();
          // control.UnLockFinger(0);
          // control.UnLockFinger(1);
          // control.UnLockFinger(2);
          // control.UnLockFinger(3);
          // control.UnLockFinger(4);
        }
      }

      if (this.name == "Master.R"){
        float R_Dist = Vector3.Distance(this.transform.position, closest.transform.position);
        
        if (R_Dist < 0.25){
          GrabPoint();
          // control.LockFinger(0);
          // control.LockFinger(1);
          // control.LockFinger(2);
          // control.LockFinger(3);
          // control.LockFinger(4);

        } else{
          ReleasePoint();
          // control.UnLockFinger(0);
          // control.UnLockFinger(1);
          // control.UnLockFinger(2);
          // control.UnLockFinger(3);
          // control.UnLockFinger(4);
        }
      }
    
  }

  private void FixedUpdate(){

  	lastPosition = transform.position;
  }

  private void LateUpdate(){

  	Delta = lastPosition - transform.position;
  }

  private void GrabPoint(){

  	currentPoint = Utility.GetNearest(transform.position, contactPoints);

  	if(currentPoint){

  		climber.SetHand(this);
  	}
  }

  public void ReleasePoint(){

  	if(currentPoint){
  		climber.ClearHand();
  	}

  	currentPoint = null;
  }

  private void AddPoint(GameObject newObject){
    
    if (newObject.CompareTag("ClimbPoint")){
      contactPoints.Add(newObject);
    }

  }

  private void RemovePoint(GameObject newObject){

    if (newObject.CompareTag("ClimbPoint")){
      contactPoints.Remove(newObject);
    }
  }

  private void OnTriggerEnter(Collider other){

    if (other.CompareTag("ClimbPoint")){
      AddPoint(other.gameObject);
      // GrabPoint();
      bar.GetComponent<loadingbar>().minusValue();

    }

  } 

  private void OnTriggerExit(Collider other){

    if (other.CompareTag("ClimbPoint")){
      RemovePoint(other.gameObject);
    }

  }

  GameObject GetClosestPoint(GameObject[] climbPoint){
      // Transform tMin = null;
      float minDist = Mathf.Infinity;
      GameObject output = null; 

      Vector3 currentPos = transform.position;
      foreach (GameObject t in climbPoint)
      {
          float dist = Vector3.Distance(t.GetComponent<Transform>().position, currentPos);
          if (dist < minDist)
          {
              minDist = dist;
              output = t;
          }
      }

      return output;
  }

}
