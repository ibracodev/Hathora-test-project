using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateRooms : MonoBehaviour
{

    private string appId = "app-b37cbc0e-e4aa-4deb-bfea-b056ef86f916";

    private string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkVFcDg4bkpRU25idmVtMFIyTWZTRyJ9.eyJlbWFpbCI6ImlicmFoaW1qZGgyNkBnbWFpbC5jb20iLCJpc3MiOiJodHRwczovL2F1dGguaGF0aG9yYS5jb20vIiwic3ViIjoiZ29vZ2xlLW9hdXRoMnwxMDQ3NjgxMTg0NzkyODExNDMxMjgiLCJhdWQiOlsiaHR0cHM6Ly9jbG91ZC5oYXRob3JhLmNvbSIsImh0dHBzOi8vZGV2LXRjaHA2aW45LnVzLmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE3MTU1NzgzODMsImV4cCI6MTcxNTY2NDc4Mywic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCBvZmZsaW5lX2FjY2VzcyIsImF6cCI6InRXakRodXpQbXVJV3JJOFI5czN5VjNCUVZ3MnRXMHlxIn0.sp-t7n_00jcDBkzhlDVtWYqu_DLfl4fVd7SPKG_wW1ya7OY2RsB1gX1ukaqz6m8H-cbgRvHRP-UUxuouN8MiuIFefuJHKeR7945-uA7VTt16x2h8f9jccsR8X4HZXmHavXzJD45WL5a8diIq3p3e6yS59Fg1pEWEJ5kdTSySrS3TG36JjA2U2R0xuQzENjbqASNCOjiM9e2NCYnUt4RVIuHy7bn7T9aOma_KThSYI46zVHi5NowWhdv7tGOQdzY1dgjf2oF_0AHCUSglOZU6GCjFNRIaCIwYsejBEHycZY4YIRJf92MsVmEyYFaUKuALOkVJ5Ddsu8mho9NHzH6rKg";
    
    public void CreateRoom(){
        StartCoroutine(Room());
    }

    private IEnumerator Room(){

        string url = $"https://api.hathora.dev/rooms/v2/{appId}/create";

        // WWWForm form = new WWWForm();
        // form.AddField("roomConfig", "{\"name\":\"my-room\"}");
        // form.AddField("region", "Seattle");
        string payload = "{\"roomConfig\": \"{\\\"name\\\":\\\"my-room\\\"}\", \"region\": \"Seattle\"}";


        using (UnityWebRequest request = UnityWebRequest.Post(url, payload, "application/json"))
        {
            Debug.Log("payload is: " + payload);
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // Request successful
                Debug.Log("Room created successfully!");
                Debug.Log(request.downloadHandler.text); // Response from the server
            }
        }
    }
}
