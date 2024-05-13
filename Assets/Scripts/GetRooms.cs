using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetRooms : MonoBehaviour
{
    private string appId = "app-b37cbc0e-e4aa-4deb-bfea-b056ef86f916";
    private string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkVFcDg4bkpRU25idmVtMFIyTWZTRyJ9.eyJlbWFpbCI6ImlicmFoaW1qZGgyNkBnbWFpbC5jb20iLCJpc3MiOiJodHRwczovL2F1dGguaGF0aG9yYS5jb20vIiwic3ViIjoiZ29vZ2xlLW9hdXRoMnwxMDQ3NjgxMTg0NzkyODExNDMxMjgiLCJhdWQiOlsiaHR0cHM6Ly9jbG91ZC5oYXRob3JhLmNvbSIsImh0dHBzOi8vZGV2LXRjaHA2aW45LnVzLmF1dGgwLmNvbS91c2VyaW5mbyJdLCJpYXQiOjE3MTU1NzgzODMsImV4cCI6MTcxNTY2NDc4Mywic2NvcGUiOiJvcGVuaWQgcHJvZmlsZSBlbWFpbCBvZmZsaW5lX2FjY2VzcyIsImF6cCI6InRXakRodXpQbXVJV3JJOFI5czN5VjNCUVZ3MnRXMHlxIn0.sp-t7n_00jcDBkzhlDVtWYqu_DLfl4fVd7SPKG_wW1ya7OY2RsB1gX1ukaqz6m8H-cbgRvHRP-UUxuouN8MiuIFefuJHKeR7945-uA7VTt16x2h8f9jccsR8X4HZXmHavXzJD45WL5a8diIq3p3e6yS59Fg1pEWEJ5kdTSySrS3TG36JjA2U2R0xuQzENjbqASNCOjiM9e2NCYnUt4RVIuHy7bn7T9aOma_KThSYI46zVHi5NowWhdv7tGOQdzY1dgjf2oF_0AHCUSglOZU6GCjFNRIaCIwYsejBEHycZY4YIRJf92MsVmEyYFaUKuALOkVJ5Ddsu8mho9NHzH6rKg";

    public void GetProccessAndRooms()
    {
        StartCoroutine(GetProcess());
    }

    private IEnumerator GetProcess()
    {
        string url = $"https://api.hathora.dev/processes/v2/{appId}/list/latest";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                // Parse JSON response
                string jsonResponse = request.downloadHandler.text;
                ProcessData[] processes = JsonHelper.FromJson<ProcessData>(jsonResponse);

                // Find the process with status "running"
                foreach (var process in processes)
                {
                    if (process.status == "running")
                    {
                        Debug.Log("Process ID with status running: " + process.processId);
                        // Here you can store or further process the process ID
                        StartCoroutine(GetActiveRooms(process.processId));
                    }
                }
            }
        }
    }

    private IEnumerator GetActiveRooms(string processId)
    {   
        string roomURL = $"https://api.hathora.dev/rooms/v2/{appId}/list/{processId}/active";

        using (UnityWebRequest roomRequest = UnityWebRequest.Get(roomURL))
        {
            roomRequest.SetRequestHeader("Authorization", "Bearer " + token);
            yield return roomRequest.SendWebRequest();

            // Check for errors
            if (roomRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(roomRequest.error);
            }
            else
            {
                // Parse JSON response for active rooms
                string roomJsonResponse = roomRequest.downloadHandler.text;
                RoomData[] rooms = JsonHelper.FromJson<RoomData>(roomJsonResponse);

                // Now you have the active rooms for this process, you can further process them as needed
                foreach (var room in rooms)
                {
                    if (room.status == "active"){
                         Debug.Log("Room ID: " + room.roomId);
                    }
                    // Process other room data here
                }
            }
        }
    }

    [System.Serializable]
    public class RoomData
    {
        public string appId;
        public string roomId;
        public string roomConfig;
        public string status;
        public RoomAllocation currentAllocation;
    }

    [System.Serializable]
    public class RoomAllocation
    {
        public string unscheduledAt;
        public string scheduledAt;
        public string processId;
        public string roomAllocationId;
    }



    [System.Serializable]
    public class ProcessData
    {
        public string status;
        public int roomsAllocated;
        public string terminatedAt;
        public string stoppingAt;
        public string startedAt;
        public string createdAt;
        public int roomsPerProcess;
        public AdditionalExposedPort[] additionalExposedPorts;
        public ExposedPort exposedPort;
        public string region;
        public string processId;
        public int deploymentId;
        public string appId;
    }

    [System.Serializable]
    public class AdditionalExposedPort
    {
        public string host;
        public string name;
        public int port;
        public string transportType;
    }

    [System.Serializable]
    public class ExposedPort
    {
        public string host;
        public string name;
        public int port;
        public string transportType;
    }

    // Helper class for deserializing arrays in JSON response
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            // Since JSONUtility doesn't support arrays directly, we need to wrap it in another object
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        // Wrapper class to support deserializing arrays with JsonUtility
        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

}
