using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class JoinRoom : MonoBehaviour
{
    private string appId = "app-b37cbc0e-e4aa-4deb-bfea-b056ef86f916";
    public string roomId = "";
    
    public void Join()
    {
        StartCoroutine(JoinRoomEnum());
    }

    private IEnumerator JoinRoomEnum()
    {
        string url = $"https://api.hathora.dev/rooms/v2/{appId}/connectioninfo/{roomId}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;

                // Parse JSON response
                RoomData room = JsonUtility.FromJson<RoomData>(jsonResponse);

                // Check if the room status is active
                if (room.status == "active")
                {
                    Debug.Log("Active Room ID: " + room.roomId);
                    Debug.Log("Host: " + room.exposedPort.host);
                    Debug.Log("Port: " + room.exposedPort.port);
                }
                else
                {
                    Debug.Log("Room is not active.");
                }
            }
        }
    }

    [System.Serializable]
    public class RoomData
    {
        public string status;
        public string roomId;
        public ExposedPort exposedPort;
        public AdditionalExposedPort[] additionalExposedPorts;
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
}
