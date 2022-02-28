using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using UnityEngine.UI;

public class BohController : MonoBehaviour
{
    public GameObject player;
    public GameObject rightHandGhost;
    public GameObject leftHandGhost;

    public int pulseWidth = 400; //0-400
    public int frequency = 100; //0-100

    //The strengths correspond to the channels in the arduino code, NOT the order of elements in the rightHand[] array. Ex. Element 0 == Channel 1 == 'a' 
    public int[] strengthRightHand; //0-255

    public GameObject[] rightHand;
    public GameObject[] leftHand;

    public Canvas canvas;
    public Image canvasImage;


    private bool prevStimulatedRight = false;
    private bool anyStimulatedRight = false;
    private float startYRight = -1;
    private float cameraOrigYRight = -1;
    private float currentMaxDisplacementRight = -1;

    private bool prevStimulatedLeft = false;
    private bool anyStimulatedLeft = false;
    private float startYLeft = -1;
    private float cameraOrigYLeft = -1;
    private float currentMaxDisplacementLeft = -1;

    private string serialport = "COM19";

    // The order of the letters just corresponds to how the hand elements are assigned to the rightHand[] array, it's all arbitrary
    private string[] letters = { "st", "l", "dk", "c", "n", "fm", "e", "p", "ho", "g", "r", "jq", "i", "b", "a" };

    GameObject hsdIndex3;
    GameObject hsdIndex2;
    GameObject hsdIndex1;

    Sprite[] handSegmentSprites = new Sprite[15];
    GameObject[] handSegmentDisplays = new GameObject[15];
    Image[] handSegmentDisplaysCanvas = new Image[15];
    Sprite empty;
    Sprite baseHand;

    SerialPort stream;

    // Start is called before the first frame update
    void Start()
    {

        baseHand = Resources.Load<Sprite>("HandSegmentDisplay/hand");

        handSegmentSprites[0] = Resources.Load<Sprite>("HandSegmentDisplay/palm_red");
        handSegmentSprites[1] = Resources.Load<Sprite>("HandSegmentDisplay/index1_red");
        handSegmentSprites[2] = Resources.Load<Sprite>("HandSegmentDisplay/index2_red");
        handSegmentSprites[3] = Resources.Load<Sprite>("HandSegmentDisplay/index3_red");
        handSegmentSprites[4] = Resources.Load<Sprite>("HandSegmentDisplay/middle1_red");
        handSegmentSprites[5] = Resources.Load<Sprite>("HandSegmentDisplay/middle2_red");
        handSegmentSprites[6] = Resources.Load<Sprite>("HandSegmentDisplay/middle3_red");
        handSegmentSprites[7] = Resources.Load<Sprite>("HandSegmentDisplay/ring1_red");
        handSegmentSprites[8] = Resources.Load<Sprite>("HandSegmentDisplay/ring2_red");
        handSegmentSprites[9] = Resources.Load<Sprite>("HandSegmentDisplay/ring3_red");
        handSegmentSprites[10] = Resources.Load<Sprite>("HandSegmentDisplay/pinky1_red");
        handSegmentSprites[11] = Resources.Load<Sprite>("HandSegmentDisplay/pinky2_red");
        handSegmentSprites[12] = Resources.Load<Sprite>("HandSegmentDisplay/pinky3_red");
        handSegmentSprites[13] = Resources.Load<Sprite>("HandSegmentDisplay/thumb2_red");
        handSegmentSprites[14] = Resources.Load<Sprite>("HandSegmentDisplay/thumb3_red");

        empty = Resources.Load<Sprite>("HandSegmentDisplay/empty");

        for (int i = 0; i < 15; i++)
        {

            handSegmentDisplaysCanvas[i] = Instantiate(canvasImage);
            handSegmentDisplaysCanvas[i].sprite = empty;
            handSegmentDisplaysCanvas[i].transform.SetParent(canvas.transform, false);
        }

        try
        {
            stream = new SerialPort(serialport, 115200);
            string[] ports = SerialPort.GetPortNames();

            Debug.Log("# Ports: " + ports.Length);
            for (int i = 0; i < ports.Length; i++)
            {
                Debug.Log(ports[i]);
            }

            stream.ReadTimeout = 50;
            stream.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            stream.Open();
        }
        catch (Exception e)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {


        for (int i = 0; i < 15; i++)
        {
            handSegmentDisplaysCanvas[i].sprite = empty;
        }

        for (int i = 0; i < 15; i++)
        {
            if (colliding(rightHand[i]))
            {
                handSegmentDisplaysCanvas[i].sprite = handSegmentSprites[i]; //Set the Sprite of the Image Component on the new GameObject

            }
        }


        String message1 = pulseWidth + "," + frequency + ",";
        String message2 = "";
        String message3 = "";

        prevStimulatedRight = anyStimulatedRight;
        anyStimulatedRight = false;

        String alphabet = "abcdefghijklmnopqrst";
        for (int i = 0; i < alphabet.Length; i++)
        {
            bool assigned = false;
            char letter = alphabet[i];
            message2 = message2 + strengthRightHand[letter - 'a'] + ",";
            for (int j = 0; j < 15; j++)
            {
                if (letters[j].Contains(letter.ToString()))
                {
                    assigned = true;
                    if (colliding(rightHand[j]))
                    {
                        message3 = message3 + "1,";
                        anyStimulatedRight = true;
                    }
                    else
                    {
                        message3 = message3 + "0,";
                    }
                }
            }
            if (!assigned)
            {
                message3 = message3 + "0,";
            }
        }
        message3 = message3.Substring(0, message3.Length - 1);

        if (anyStimulatedRight)
        {
            message1 = message1 + "1,";
        }
        else
        {
            message1 = message1 + "0,";
        }

        String wholeMessage = message1 + message2 + message3;
        WriteToSerial(wholeMessage);



        prevStimulatedLeft = anyStimulatedLeft;
        anyStimulatedLeft = false;

        for (int i = 0; i < alphabet.Length; i++)
        {
            char letter = alphabet[i];
            for (int j = 0; j < 15; j++)
            {
                if (letters[j].Contains(letter.ToString()))
                {
                    if (colliding(leftHand[j]))
                    {
                        anyStimulatedLeft = true;
                    }
                }
            }
        }


        if (!prevStimulatedRight && anyStimulatedRight)
        {
            startYRight = rightHandGhost.transform.position.y;
            cameraOrigYRight = player.transform.position.y;
            currentMaxDisplacementRight = 0;
        }
        if (anyStimulatedRight)
        {
            float currentY = rightHandGhost.transform.position.y;
            Vector3 p = player.transform.position;
            if ((startYRight - currentY) > currentMaxDisplacementRight)
            {
                player.transform.position = new Vector3(p.x, cameraOrigYRight + (startYRight - currentY) * 1f, p.z);
                currentMaxDisplacementRight = startYRight - currentY;
            }
        }


        if (!prevStimulatedLeft && anyStimulatedLeft)
        {
            startYLeft= leftHandGhost.transform.position.y;
            cameraOrigYLeft = player.transform.position.y;
            currentMaxDisplacementLeft = 0;
        }
        if (anyStimulatedLeft)
        {
            float currentY = leftHandGhost.transform.position.y;
            Vector3 p = player.transform.position;
            if ((startYLeft - currentY) > currentMaxDisplacementLeft)
            {
                player.transform.position = new Vector3(p.x, cameraOrigYLeft + (startYLeft- currentY) * 1f, p.z);
                currentMaxDisplacementLeft = startYLeft- currentY;
            }
        }

    }

    public void WriteToSerial(string message)
    {
        if (!stream.IsOpen) return;
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    private static void DataReceivedHandler(
                    object sender,
                    SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        Debug.Log("Data Received:");
        Debug.Log(indata);
    }

    private bool colliding(GameObject g)
    {
        return g.GetComponent<HPTK.Views.Notifiers.CustomCollisionNotifier>().colliding;
    }
}
