using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class QRCodeManager : MonoBehaviour
{
    public static QRCodeManager Instance { get; private set; }

    private WebCamTexture camTexture;
    private BarcodeReader barcodeReader;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        camTexture = new WebCamTexture();
        barcodeReader = new BarcodeReader();

        if (camTexture != null)
        {
            camTexture.Play();
        }
        else
        {
            Debug.LogError("No Camera Found!");
        }
    }

    void Update()
    {
        if (camTexture != null && camTexture.isPlaying)
        {
            try
            {
                var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
                if (result != null)
                {
                    Debug.Log("QR Code Scanned: " + result.Text);
                    RegisterQRCode(result.Text);
                    camTexture.Stop();
                }
            }
            catch
            {
                Debug.LogError("QR Code Scan Failed!");
            }
        }
    }

    public string GetQRCodeData()
    {
        if (camTexture == null) return null;

        var result = barcodeReader.Decode(camTexture.GetPixels32(), camTexture.width, camTexture.height);
        return result?.Text;
    }

   public void RegisterQRCode(string qrCodeData)
{
    Debug.Log("QR Code Registered: " + qrCodeData);
    // Add logic to store or process the QR Code
}
}
