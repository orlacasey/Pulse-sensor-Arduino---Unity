using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO.Ports;
using UnityEngine.UI;

public class DetectPulseRate : MonoBehaviour 
{
	public  static SerialPort serialPort;
	public  static string streamRead;
	public static int BPM;

	public GameObject sphere;
	public GameObject heartText;

	void Start() 
	{
		OpenConnection();
	}

	public void OpenConnection() 
	{
		serialPort = new SerialPort("/dev/cu.usbmodem1421", 115200, Parity.None, 8, StopBits.One);

		if(serialPort != null) 
		{
			if(!serialPort.IsOpen) 
			{
				serialPort.Close();
				Debug.Log ("Chiudo la porta, perché è già aperta");
			}
			else 
			{
				serialPort.Open();
				serialPort.ReadTimeout = 100;
				Debug.Log("Porta aperta!");
			}
		}
		else 
		{
			if(serialPort.IsOpen)
				Debug.Log("the port is open");
		}
	}

	void FixedUpdate()
	{
		if (serialPort.IsOpen && serialPort.BytesToRead > 0) {
			streamRead = serialPort.ReadLine ();
			BPM = int.Parse (streamRead);

			sphere.transform.localScale = new Vector3 (1, 1, 1);
		}

		Vector3 sphereScale = new Vector3 (BPM / 100.0f, BPM / 100.0f, BPM / 100.0f);

		heartText.GetComponent<Text> ().text = "BPM " + BPM;
		sphere.transform.localScale = Vector3.Lerp (sphere.transform.localScale, sphereScale, 0.1f);

		serialPort.BaseStream.Flush ();

	}


	void OnApplicationQuit() 
	{
		// The port is closed

		if (serialPort != null)
			serialPort.Close();
	}
}
