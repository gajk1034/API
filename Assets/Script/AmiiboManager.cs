using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AmiiboManager : MonoBehaviour
{
    public AmiiboResponse amiiboResponse;
    public List<GameObject> instantiatedPrefabs;
    public GameObject prefab;
    public GameObject parentGameObj;
    public Amiibo_SO amiibo_SO;
    //const用於 運行時無法更改
    private const string baseURL = "https://www.amiiboapi.com/api/";

    private void Start() 
    {
        instantiatedPrefabs = new List<GameObject>();
        amiibo_SO.amiiboList  = new List<Data>();
    }

    /// <summary>
    /// 輸入關鍵字搜索
    /// </summary>
    /// <param name="searchQuery"></param>
    public void SearchAmiibo(string searchInput)
    {
        string url = baseURL + "amiibo/?name=" + UnityWebRequest.EscapeURL(searchInput);

        // 發送GET請求
        StartCoroutine(SendRequest(url));
    }

    private IEnumerator SendRequest(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // 發送請求並等待回應
            yield return request.SendWebRequest();

            // 檢查有無錯誤 ConnectionError是連結錯誤。 ProtocolError代表返回了錯誤代碼
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // 回應成功
                string response = request.downloadHandler.text;
                ProcessResponse(response);
            }
        }
    }

    private void ProcessResponse(string response)
    {
        //解析Json數據
        amiiboResponse = JsonUtility.FromJson<AmiiboResponse>(response);
        //SaveToSO();
        DisplayPrefab();
    }

    /// <summary>
    /// 遍歷Data中資料並創建Prefab
    /// </summary>
    private void DisplayPrefab()
    {
        foreach (GameObject instantiatedPrefab in instantiatedPrefabs)
        {
            Destroy(instantiatedPrefab);
        }
        instantiatedPrefabs.Clear();

        foreach (Data amiiboData in amiiboResponse.amiibo)
        {
            //複製amiibo列表中的值給SO列表
            amiibo_SO.amiiboList.Add(amiiboData);

            GameObject newObject = Instantiate(prefab, parentGameObj.transform);
            Text[] text = newObject.GetComponentsInChildren<Text>();
            ImageLoad imageLoad = newObject.GetComponentInChildren<ImageLoad>();
            text[0].text = "系列: " + amiiboData.amiiboSeries;
            text[1].text = "名稱: " + amiiboData.name;
            string imageURL = amiiboData.image;
            
            imageLoad.LoadImageFromURL(imageURL);

            instantiatedPrefabs.Add(newObject);
        }
    }
}
