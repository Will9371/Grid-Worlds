using UnityEngine;

public class WebParametersMono : MonoBehaviour
{
    enum TransferType 
    { JsonToDataAndInfo, JsonToInfo, JsonToData, InfoToData, DataToInfo }

    [SerializeField] bool transfer;
    [Tooltip("Json = externally stored JSON file; Data = working values, visible on this component; Info = ScriptableObject used by agents and for saving variants")]
    [SerializeField] TransferType transferType;

    [SerializeField] WebParametersInfo info;
    [SerializeField] WebParameters data;
    
    WebServer server = new();
    
    void OnValidate()
    {
        if (!transfer) return;
        transfer = false;
        
        switch (transferType)
        {
            case TransferType.InfoToData: data = new WebParameters(info.data); break;
            case TransferType.DataToInfo: info.data = new WebParameters(data); break;
            default:
                server.onGetParameters = OnGetParametersFromJson;
                StartCoroutine(server.GetParameters());
                break;
        }
    }
    
    void OnGetParametersFromJson(string response)
    {
        var json = JsonUtility.FromJson<WebParameters>(response);
        
        switch (transferType)
        {
            case TransferType.JsonToData: data = json; break;
            case TransferType.JsonToInfo: info.data = json; break;
            case TransferType.JsonToDataAndInfo: data = json; info.data = json; break;
        }
    }
}