using ZXing;
using Meta.XR;
using UnityEngine;
using System.Collections;
using PassthroughCameraSamples;
using System.Collections.Generic;

public class QrCodeScanner : MonoBehaviour
{
    [SerializeField] private int _scanFrameFrequency;
    [SerializeField] private WebCamTextureManager _webCamTex;
    [SerializeField] private EnvironmentRaycastManager _environmentRaycast;
    
    private bool _isCameraReady;
    private BarcodeReader _barcodeReader = new ();
    
    [System.Serializable]
    public struct QrCodeTarget
    {
        public string QrCodeContent;
        public Transform Object;
    }
    
    [SerializeField] private List<QrCodeTarget> _qrCodeTargets = new();
    private Dictionary<string, Transform> _qrCodeTargetDic = new();
    private string _lastDetectedQr = "";

    private IEnumerator Start()
    {
        foreach (var qrCode in _qrCodeTargets)
        {
            _qrCodeTargetDic.Add(qrCode.QrCodeContent, qrCode.Object);
        }

        while (_webCamTex.WebCamTexture == null)
        {
            yield return null;
        }
        
        _isCameraReady = true;
    }

    private void Update()
    {
        if (!_isCameraReady || Time.frameCount % _scanFrameFrequency != 0)
            return;

        WebCamTexture webCamTexture = _webCamTex.WebCamTexture;
        if (webCamTexture == null || webCamTexture.width <= 16 || webCamTexture.height <= 16)
            return;

        Color32[] camPixels = webCamTexture.GetPixels32();
        Result result = _barcodeReader.Decode(camPixels, webCamTexture.width, webCamTexture.height);

        if (result == null || string.IsNullOrWhiteSpace(result.Text))
            return;

        string scannedText = result.Text.Trim().ToLower();
        Debug.Log($"QR Code detected: '{scannedText}'");

        if (scannedText == _lastDetectedQr)
            return;

        _lastDetectedQr = scannedText;

        if (_qrCodeTargetDic.TryGetValue(scannedText, out Transform obj))
        {
            Debug.Log("Matched QR code to target object.");

            Vector2Int qrCodeCenter = GetQrCodeCenter(result.ResultPoints, webCamTexture.height);
            Debug.Log($"QR Center: {qrCodeCenter}");

            Pose pose = ConvertScreenPointToWorldPoint(qrCodeCenter);
            Debug.Log($"World Pose: {pose.position}, {pose.rotation}");

            //obj.SetPositionAndRotation(pose.position, pose.rotation);
            obj.position = pose.position;
        }
        else
        {
            Debug.LogWarning($"Scanned QR code '{scannedText}' not found in target list.");
        }
    }

    private Vector2Int GetQrCodeCenter(ResultPoint[] resultsPoints, int textureHeight)
    {
        if (resultsPoints == null || resultsPoints.Length == 0)
        {
            return Vector2Int.zero;
        }

        float sumX = 0;
        float sumY = 0;

        foreach (var point in resultsPoints)
        {
            sumX += point.X;
            sumY += point.Y;
        }
        
        float x =  sumX / resultsPoints.Length;
        float y =  sumY / resultsPoints.Length;
        
        int centerX = Mathf.RoundToInt(x);
        int centerY = Mathf.RoundToInt(textureHeight - y);
        
        return new Vector2Int(centerX, centerY);
    }

    private Pose ConvertScreenPointToWorldPoint(Vector2Int screenPoint)
    {
        Ray ray = PassthroughCameraUtils.ScreenPointToRayInWorld(_webCamTex.Eye, screenPoint);

        if (_environmentRaycast.Raycast(ray, out EnvironmentRaycastHit hitInfo))
        {
            Pose pose = new(hitInfo.point, Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
            return pose;
        }
        
        return Pose.identity;
    }
}
