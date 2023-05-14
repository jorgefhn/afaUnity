using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroneMovement : MonoBehaviour
{
    public float speed = 20.0f;
    private GameObject droneGameObject;
    private Dictionary<string, Vector3> dronePositions = new Dictionary<string, Vector3>();

    
    void Start()
    {
        droneGameObject = gameObject;   
    }


    

    public bool moveTo(Vector3 newPosition){
        
        droneGameObject.transform.position = Vector3.MoveTowards(droneGameObject.transform.position,newPosition,speed*Time.deltaTime);    
        return true;
    }

    

    bool fleeFromAdversary()
    {
        // function to determine if a drone has to flee from its adversary

        return true;

        
    }

     Vector3 decideTarget()
    {

        /*
        // Finding drone1 destiny. We have to add drone2 also
        String d1name = "drone1";

        int d1Start = variableToShare.IndexOf(d1name,0,variableToShare.Length)+d1name.Length+1; //{drone1=(...)}
        int d1Finish = variableToShare.IndexOf(")",0,variableToShare.Length); //{drone1=(...)}
        
        if (d1Finish != -1){
            String substring = variableToShare.Substring(d1Start+1,d1Finish-d1Start-1);
        
            string[] vArray = substring.Split(',');

            CultureInfo ci = CultureInfo.InvariantCulture;
            Vector3 v = new Vector3(float.Parse(vArray[0],ci),
                                    float.Parse(vArray[1],ci),
                                    float.Parse(vArray[2],ci));
            return v;



        */
        return new Vector3(0,0,0);

        }




    


    
}
