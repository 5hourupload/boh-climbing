using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimberPoint : MonoBehaviour
{
    public Control control;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){

      Debug.Log("trigger"+ other.name);

      if (GetComponent<Collider>().name.Contains("thumb")){
          control.LockFinger(0);
      }

      if (GetComponent<Collider>().name.Contains("index")){
          control.LockFinger(1);
      }

      if (GetComponent<Collider>().name.Contains("middle")){
          control.LockFinger(2);
      }

      if (GetComponent<Collider>().name.Contains("ring")){
          control.LockFinger(3);
      }

      if (GetComponent<Collider>().name.Contains("pinky")){
          control.LockFinger(4);
      }

    } 

    private void OnTriggerExit(Collider other){

      Debug.Log("exit"+ other.name);

      if (GetComponent<Collider>().name.Contains("thumb")){
          control.UnLockFinger(0);
      }

      if (GetComponent<Collider>().name.Contains("index")){
          control.UnLockFinger(1);
      }

      if (GetComponent<Collider>().name.Contains("middle")){
          control.UnLockFinger(2);
      }

      if (GetComponent<Collider>().name.Contains("ring")){
          control.UnLockFinger(3);
      }

      if (GetComponent<Collider>().name.Contains("pinky")){
          control.UnLockFinger(4);
      }    

  }
}
