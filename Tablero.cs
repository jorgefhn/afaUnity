using UnityEngine;
using System.Collections;
using System.Threading;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using static UnityMainThreadDispatcher;

public class Tablero : MonoBehaviour 
{

    public bool isRunning = true;

    public String newPositions;
    public List<String> healthPackages,chargePackages,ammoPackages;

    // class to represent drone info

    public class droneInfo
    {
        public int health;
        public int charge;
        public int ammo;
        public String position;

    }
    public class Info
    {
        public droneInfo drone1;
        public droneInfo drone2; 
        public List<String> healthPackages;
        public List<String> chargePackages;
        public List<String> ammoPackages;

        public Info() {
            drone1 = new droneInfo();
            drone2 = new droneInfo();
            healthPackages = new List<String>();
            chargePackages = new List<String>();
            ammoPackages = new List<String>();
        }
    }
    Info gameInfo = new Info();    
    Thread senderThread;
    Thread listenerThread;


    void Start () 
    {

        SpawnRandomObjects();

        // initial drone1 systems at 100
        gameInfo.drone1.health = 100;
        gameInfo.drone1.charge = 100;
        gameInfo.drone1.ammo = 100;

        // initial drone2 systems at 100
        gameInfo.drone2.health = 100;
        gameInfo.drone2.charge = 100;
        gameInfo.drone2.ammo = 100;


        
        
        senderThread = new Thread( Sender ) { IsBackground = true };
        listenerThread = new Thread( Listener ) { IsBackground = true };

        senderThread.Start();
        listenerThread.Start();


    }

    
    Vector3 StringToVector3(string str)
    {
        str = str.Replace("[", "").Replace("]", "");
        string[] components = str.Split(',');
        return new Vector3(float.Parse(components[0]), float.Parse(components[1]), float.Parse(components[2]));
    }

    void Update()
    {

        // healthPositions

        gameInfo.healthPackages = healthPackages;
        gameInfo.chargePackages = chargePackages;
        gameInfo.ammoPackages = ammoPackages;


        // Aqui hay que hacer lo mismo con los ammo packages y charge packages

        
        // get drone2 position
        var drone2 = GameObject.Find("drone2");

        if (drone2){
            Debug.Log("Position of drone2: "+drone2.transform.position);

            var mov2 = drone2.GetComponent<DroneMovement>();

            // Move
            string[] positions = newPositions.Split('?');
            Vector3 drone2Positions = StringToVector3(positions[1]);
            mov2.moveTo(drone2Positions);   
            gameInfo.drone2.position = drone2.transform.position.ToString();

            
        }

        // get drone1 position
        var drone1 = GameObject.Find("drone1");
        if (drone1){
            var mov1 = drone1.GetComponent<DroneMovement>();

            // Move
            string[] positions = newPositions.Split('?');
            Vector3 drone1Positions = StringToVector3(positions[0]);
            mov1.moveTo(drone1Positions);    
            gameInfo.drone1.position = drone1.transform.position.ToString();            
     
        }

        


    }

   

    void OnApplicationQuit()
    {
        isRunning = false;
    }
       

    void Sender()
    {

        Socket s = new Socket(AddressFamily.InterNetwork,SocketType.Dgram, ProtocolType.Udp);
        IPAddress broadcast = IPAddress.Parse("127.0.0.1");



        try
        {
            while (isRunning){
            
                if (gameInfo.drone1.position != null && gameInfo.drone2.position != null){
                    string json = JsonConvert.SerializeObject(gameInfo);
                    Debug.Log("Vamos a enviar: "+json);
                    byte[] data = Encoding.ASCII.GetBytes(json);        
                    IPEndPoint ep = new IPEndPoint(broadcast,11004);
                    s.SendTo(data,ep);
                    Debug.Log("Message sent to the broadcast address");
                    Thread.Sleep(2000); // sleep for 2 seconds


                }

                
            }
           
            
        } 
        catch (SocketException e){
            Debug.Log(e);
        }
        finally 
        {
            s.Close();
        } 

    }

    void Listener()
    {

        UdpClient listener = new UdpClient(11000);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 11000);

        try
        {
            while (isRunning){
                Debug.Log("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                Debug.Log($"Received broadcast from {groupEP} :");

                // Aquí recibe la información de a dónde tienen que ir los drones
                Debug.Log($"{Encoding.ASCII.GetString(bytes,0,bytes.Length)}");


                newPositions = Encoding.ASCII.GetString(bytes,0,bytes.Length);


                Thread.Sleep(2000); // sleep for 2 seconds


                
            }
        }
        catch (SocketException e){
            Debug.Log(e);
        }
        finally 
        {
            listener.Close();
        }

        

    }

    void SpawnRandomObjects()
    {
        // Spawn random objects: health, ammo, battery packages
        System.Random random = new System.Random(5);
        int randomPackages = random.Next(1,5);
        healthPackages = new List<String>();
        chargePackages = new List<String>();
        ammoPackages = new List<String>();


    

        // Health packages 
        for (int i = 0; i < randomPackages; i++)
        {
            // Instantiate random health packages
            Vector3 randomHealthPosition = new Vector3(UnityEngine.Random.Range(-300,300), 10, UnityEngine.Random.Range(-300,300));
            var healthPackage = GameObject.Find("Heal_Up");
            if (healthPackage){
                Instantiate(healthPackage,randomHealthPosition,Quaternion.identity);
            }
            healthPackages.Add(randomHealthPosition.ToString());

            // Instantiate random charge packages
            Vector3 randomChargePosition = new Vector3(UnityEngine.Random.Range(-300,300), 10, UnityEngine.Random.Range(-300,300));
            var chargePackage = GameObject.Find("Power_Up");
            if (chargePackage){
                Instantiate(chargePackage,randomChargePosition,Quaternion.identity);
            }
            chargePackages.Add(randomChargePosition.ToString());

            // Instantiate random ammo packages
            Vector3 randomAmmoPosition = new Vector3(UnityEngine.Random.Range(-300,300), 10, UnityEngine.Random.Range(-300,300));
            var ammoPackage = GameObject.Find("AmmoBox");
            if (ammoPackage){
                Instantiate(ammoPackage,randomAmmoPosition,Quaternion.identity);
            }
            ammoPackages.Add(randomAmmoPosition.ToString());
            
        } 

        // Spawn random obstacles: trees and rocks
        int randomObstacles = random.Next(1,5);
        
    

        // Health packages 
        for (int i = 0; i < randomObstacles; i++)
        {
            // Instantiate random tree
            Vector3 randomTreePosition = new Vector3(UnityEngine.Random.Range(-300,300), -7, UnityEngine.Random.Range(-300,300));
            var tree = GameObject.Find("Tree");
            if (tree){
                Instantiate(tree,randomTreePosition,Quaternion.identity);
            }

            // Instantiate random rock
            Vector3 randomRockPosition = new Vector3(UnityEngine.Random.Range(-300,300), 0, UnityEngine.Random.Range(-300,300));
            var rock = GameObject.Find("Rock");
            if (rock){
                Instantiate(rock,randomRockPosition,Quaternion.identity);
            }
            
            
        } 
    }
}
