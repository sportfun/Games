using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;
using Newtonsoft.Json.Linq;

public class QRGenerator : MonoBehaviour
{
    [Header("Network")]
    [SerializeField] private SocketIO _socketIOComponent;
    [SerializeField] private string _socketIOChannel = "qr";
    [SerializeField] private string _tokenMessageKey = "_id";
    [SerializeField] private Token _token;
    [SerializeField] private Canvas _trainningsCanvas;
    [SerializeField] private ReactionUnityEvent _onSuccessReaction;

    [Header("Client")]
    [SerializeField] private Rect _rect;
    [SerializeField] private Image _qrImage;
    [SerializeField] private string _preGuidCode = "sportsfun";

    public string Text { get; private set; }
    public Guid CurrentGuid { get; private set; }

    private bool _shouldResend;

    private void Awake()
    {
        this._shouldResend = true;
        this.GenerateDisplayQR();
    }

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
    }

    public void OnConnectionSocket()
    {
        if (this._shouldResend)
            this._socketIOComponent.Emit(this._socketIOChannel, JObject.Parse("{ \"qr\":\"" + this.Text + "\"}"));
    }

    public void OnReconnectionSocket()
    {
        this._shouldResend = true;
    }

    public void OnSocketIOResponse(string channel, JObject response)
    {
        if (channel.Equals(this._socketIOChannel))
        {
            this.OnQRResponse(response.Value<string>(this._tokenMessageKey));
        }
    }

    public void OnQRResponse(string response)
    {
        this._token.token = response;
        this._trainningsCanvas.gameObject.SetActive(true);
        if (this._onSuccessReaction != null)
            this._onSuccessReaction.React();
        else
            this.gameObject.SetActive(false);
    }
}
