using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;

public class QRGenerator : MonoBehaviour
{
    [SerializeField] private Rect _rect;
    [SerializeField] private Image _qrImage;
    [SerializeField] private string _preGuidCode = "sportsfun";

    public string Text { get; private set; }
    public Guid CurrentGuid { get; private set; }

    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public Texture2D generateQR(string text)
    {
        var encoded = new Texture2D((int)this._rect.width, (int)this._rect.height);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }

    public void GenerateDisplayQR()
    {
        this.CurrentGuid = Guid.NewGuid();
        this.Text = this._preGuidCode + ":" + this.CurrentGuid.ToString();
        this._qrImage.sprite = Sprite.Create(this.generateQR(this.Text), this._rect, Vector2.zero);

        // TODO: Register GUID with the server before displaying it in case the QR Code is already in use (very low chance but whatever)
    }
}
