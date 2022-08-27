using HarmonyLib;

namespace COTL_API.Saves;

[HarmonyPatch]
internal static class APIDataManager
{
    internal static string DATA_PATH = "cotl_api_data.json";

    internal static COTLDataReadWriter<APIData> _dataReadWriter = new();

    internal static APIData apiData;

    static APIDataManager()
    {
        COTLDataReadWriter<APIData> dataFileReadWriter = _dataReadWriter;
        dataFileReadWriter.OnReadCompleted += delegate (APIData data)
        {
            apiData = data;
        };

        COTLDataReadWriter<APIData> dataFileReadWriter2 = _dataReadWriter;
        dataFileReadWriter2.OnCreateDefault += delegate
        {
            apiData = new APIData();
        };

        Load();
    }

    [HarmonyPatch(typeof(SaveAndLoad), nameof(SaveAndLoad.Save))]
    [HarmonyPostfix]
    internal static void Save()
    {
        _dataReadWriter.Write(apiData, DATA_PATH);
    }

    internal static void Load()
    {
        _dataReadWriter.Read(DATA_PATH);
    }

}