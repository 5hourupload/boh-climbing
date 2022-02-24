using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
	public OSC_motors motors; 
    public OSC_EMS ems;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitDevice(){
        motors.unlockAllMotors();
        // int index, int channel, int intensity, int pulse_count, int pulse_width, float pulse_delay
        // pulse[0] = thumb flex, pulse[1] = thumb extend, pulse[2] = index flex, pulse[3] = index extend, ..., pulse[9] = pinky extend
        ems.Calibrate(0, 2, 4, 40, 500, 0.01f);
        ems.Calibrate(1, 5, 2, 13, 455, 0.0055f);
        ems.Calibrate(2, 5, 2, 13, 455, 0.0055f);
        ems.Calibrate(3, 5, 2, 13, 455, 0.0055f);
        ems.Calibrate(4, 5, 2, 13, 455, 0.0055f);
        ems.Calibrate(5, 5, 2, 13, 455, 0.0055f);
        ems.Calibrate(6, 5, 2, 13, 455, 0.0055f);
        ems.Calibrate(7, 5, 2, 13, 455, 0.0055f);
        ems.Calibrate(8, 5, 2, 13, 455, 0.0055f);
        ems.Calibrate(9, 5, 2, 13, 455, 0.0055f);
    }

    public void LockFinger(int index){
        if (index == 0){
            motors.lockMotor(0);
            motors.lockMotor(1);
        }

        if (index == 1){
            motors.lockMotor(2);
            motors.lockMotor(3);
        }

        if (index == 2){
            motors.lockMotor(4);
            motors.lockMotor(5);
        }

        if (index == 3){
            motors.lockMotor(6);
            motors.lockMotor(7);
        }


        if (index == 4){
            motors.lockMotor(8);
            motors.lockMotor(9);
        }

    }


    public void UnLockFinger(int index){
        if (index == 0){
            motors.unlockMotor(0);
            motors.unlockMotor(1);
        }

        if (index == 1){
            motors.unlockMotor(2);
            motors.unlockMotor(3);
        }

        if (index == 2){
            motors.unlockMotor(4);
            motors.unlockMotor(5);
        }

        if (index == 3){
            motors.unlockMotor(6);
            motors.unlockMotor(7);
        }


        if (index == 4){
            motors.unlockMotor(8);
            motors.unlockMotor(9);
        }

    }

    public IEnumerator FallCoroutine(){
         float timePassed = 0;
         // Actuate the finger for 2 seconds, could change based on trial and error.
         motors.unlockAllMotors();
         while (timePassed < 2)
         {
             ems.SendEMSPulse(1);
             ems.SendEMSPulse(3);
             ems.SendEMSPulse(5);
             ems.SendEMSPulse(7);
             ems.SendEMSPulse(9);
             timePassed += Time.deltaTime;
     
             yield return null;
         }
    }

}
