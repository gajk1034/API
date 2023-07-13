using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoad : MonoBehaviour
{
    Image image;
    private void Start() 
    {
        image = this.gameObject.GetComponent<Image>();
    }
    
    /// <summary>
    /// 輸入網址查詢圖片
    /// </summary>
    /// <param name="url"></param>
    public void LoadImageFromURL(string url)
    {
        StartCoroutine(GetTextureFromURL(url));
    }

    private IEnumerator GetTextureFromURL(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                image.sprite = sprite;
            }
            else
            {
                Debug.Log("Image download failed: " + webRequest.error);
            }
        }
    }
}
