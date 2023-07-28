using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField ipInput;
    [SerializeField] private string gameScene;

    // Start is called before the first frame update
    void Start()
    {
        hostButton.onClick.AddListener(HostGame);
        joinButton.onClick.AddListener(JoinGame);
    }

    private void JoinGame()
    {
        Debug.Log(ipInput.text);
        //get ip input and start client
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ipInput.text,  // The IP address is a string
            (ushort)12345 // The port number is an unsigned short
        );
        NetworkManager.Singleton.StartClient();
    }

    private void HostGame()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            "127.0.0.1",  // The IP address is a string
            (ushort)12345, // The port number is an unsigned short
            "0.0.0.0" // The server listen address is a string.
        );
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
