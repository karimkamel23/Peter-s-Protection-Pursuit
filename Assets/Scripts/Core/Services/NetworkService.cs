using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService
{
    // Singleton instance
    private static NetworkService _instance;
    public static NetworkService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NetworkService();
            }
            return _instance;
        }
    }

    // Base server URL (adjust to match your actual server address)
    private string baseUrl = "http://localhost:3000";

    // Check if internet is available
    public bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    // Generic POST method
    public IEnumerator Post<T>(string endpoint, object body, Action<T> onSuccess, Action<string> onError)
    {
        if (!IsInternetAvailable())
        {
            onError?.Invoke("No internet connection");
            yield break;
        }

        string url = baseUrl + endpoint;
        string jsonBody = JsonUtility.ToJson(body);
        
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                try
                {
                    string responseText = request.downloadHandler.text;
                    
                    if (!string.IsNullOrEmpty(responseText))
                    {
                        ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(responseText);
                        if (!string.IsNullOrEmpty(errorResponse.error))
                        {
                            onError?.Invoke(errorResponse.error);
                            yield break;
                        }
                    }
                }
                catch (Exception)
                {
                }
                
                onError?.Invoke("Connection error. Please try again.");
            }
            else
            {
                try
                {
                    string responseText = request.downloadHandler.text;
                    
                    T response = JsonUtility.FromJson<T>(responseText);
                    onSuccess?.Invoke(response);
                }
                catch (Exception e)
                {
                    onError?.Invoke("Error parsing response: " + e.Message);
                }
            }
        }
    }

    // Generic GET method
    public IEnumerator Get<T>(string endpoint, Action<T> onSuccess, Action<string> onError)
    {
        if (!IsInternetAvailable())
        {
            onError?.Invoke("No internet connection");
            yield break;
        }

        string url = baseUrl + endpoint;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                try
                {
                    string responseText = request.downloadHandler.text;
                    
                    if (!string.IsNullOrEmpty(responseText))
                    {
                        ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(responseText);
                        if (!string.IsNullOrEmpty(errorResponse.error))
                        {
                            onError?.Invoke(errorResponse.error);
                            yield break;
                        }
                    }
                }
                catch (Exception)
                {
                }
                
                onError?.Invoke("Connection error. Please try again.");
            }
            else
            {
                try
                {
                    string responseText = request.downloadHandler.text;
                    
                    // Check if response is an array and needs special handling
                    if (responseText.StartsWith("[") && typeof(T).Name == "ProgressResponseWrapper")
                    {
                        // Handle JSON array for progress data
                        string wrappedJson = $"{{\"items\":{responseText}}}";
                        T response = JsonUtility.FromJson<T>(wrappedJson);
                        onSuccess?.Invoke(response);
                    }
                    else
                    {
                        T response = JsonUtility.FromJson<T>(responseText);
                        onSuccess?.Invoke(response);
                    }
                }
                catch (Exception e)
                {
                    onError?.Invoke("Error parsing response: " + e.Message);
                }
            }
        }
    }

    // Delete method
    public IEnumerator Delete(string endpoint, Action onSuccess, Action<string> onError)
    {
        if (!IsInternetAvailable())
        {
            onError?.Invoke("No internet connection");
            yield break;
        }

        string url = baseUrl + endpoint;

        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(request.error);
            }
            else
            {
                onSuccess?.Invoke();
            }
        }
    }

    // Error response class for parsing error messages from server
    [Serializable]
    private class ErrorResponse
    {
        public string error;
    }
} 