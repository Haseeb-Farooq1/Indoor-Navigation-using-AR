using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  
using ZXing;
using ZXing.QrCode;

public class QRCodeScanner : MonoBehaviour
{
    public RawImage cameraFeed;  // UI Image to display the camera feed
    public TMP_Text qrResultText; 
    private WebCamTexture webcamTexture;
    private BarcodeReader barcodeReader;  // Fixed correct barcode reader declaration

    void Start()
    {
        // Initialize camera
        webcamTexture = new WebCamTexture();
        cameraFeed.texture = webcamTexture;
        webcamTexture.Play();

        // Initialize QR Code reader
        barcodeReader = new BarcodeReader();
    }

    void Update()
    {
        // Check if the camera is running
        if (webcamTexture.width > 100)
        {
            // Get current frame as a Color32 array
            Color32[] pixels = webcamTexture.GetPixels32();
            int width = webcamTexture.width;
            int height = webcamTexture.height;

            // Decode the QR Code
            var result = barcodeReader.Decode(pixels, width, height);

            if (result != null)
            {
                qrResultText.text = "QR Code: " + result.Text;  // Fixed variable name
                ProcessScannedQRCode(result.Text);
            }
        }
    }

    void ProcessScannedQRCode(string qrData)
    {
        Debug.Log("Scanned QR Code Data: " + qrData);

        // Example QR data format: "Room101, 10, 5, 0"
        string[] parts = qrData.Split(',');

        if (parts.Length == 4)
        {
            string qrID = parts[0].Trim();
            float x, y, z;

            if (float.TryParse(parts[1], out x) &&
                float.TryParse(parts[2], out y) &&
                float.TryParse(parts[3], out z))
            {
                Vector3 qrPosition = new Vector3(x, y, z);

                // Make sure QRCodeManager exists and is implemented correctly
                if (QRCodeManager.Instance != null)
                {
                   QRCodeManager.Instance.RegisterQRCode(qrData);

                }
                else
                {
                    Debug.LogError("QRCodeManager.Instance is null. Make sure QRCodeManager is added to the scene.");
                }
            }
            else
            {
                Debug.LogError("QR Code data is not in the correct format.");
            }
        }
    }
}
