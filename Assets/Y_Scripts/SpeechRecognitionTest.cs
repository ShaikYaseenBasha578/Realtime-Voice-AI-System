using System.IO;
using HuggingFace.API;
using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecognitionTest : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Magazine playerMagazine;



    private AudioClip clip;
    private byte[] bytes;
    private bool recording;

    private void Start()
    {
        startButton.onClick.AddListener(StartRecording);
        stopButton.onClick.AddListener(StopRecording);
        stopButton.interactable = false;
    }

    private void Update()
    {
        if (recording && Microphone.GetPosition(null) >= clip.samples)
        {
            StopRecording();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!recording)
                StartRecording();
            else
                StopRecording();
        }
    }

    private void StartRecording()
    {
        text.color = Color.white;
        text.text = "Recording...";
        startButton.interactable = false;
        stopButton.interactable = true;
        clip = Microphone.Start(null, false, 10, 44100);
        recording = true;
    }

    private void StopRecording()
    {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        SendRecording();
    }

    private void SendRecording()
    {
        text.color = Color.yellow;
        text.text = "Sending...";
        stopButton.interactable = false;

        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            text.color = Color.white;
            text.text = EscapeTMP(response);
            HandleVoiceCommand(response.ToLower());
            startButton.interactable = true;
        }, error => {
            text.color = Color.red;
            text.text = EscapeTMP(error);
            startButton.interactable = true;
        });
    }

    private string EscapeTMP(string input)
    {
        return input.Replace("<", "&lt;").Replace(">", "&gt;");
    }


    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
   

    private void HandleVoiceCommand(string command)
    {
        command = command.ToLower();
        if (command.Contains("add ammo") || command.Contains("reload") || command.Contains("more ammo"))
        {
            if (playerMagazine != null)
            {
                playerMagazine.AddAmmo(10);
                Debug.Log("Voice Command: Adding Ammo");
            }
        }
        else if (command.Contains("switch gun") || command.Contains("change weapon") || command.Contains("next weapon"))
        {
            if (playerInventory != null)
            {
                int nextIndex = playerInventory.GetNextIndex();
                playerInventory.Equip(nextIndex);
                Debug.Log("Voice Command: Switched Weapon");
            }
        }
        else if (command.Contains("previous weapon") || command.Contains("last weapon"))
        {
            if (playerInventory != null)
            {
                int prevIndex = playerInventory.GetLastIndex();
                playerInventory.Equip(prevIndex);
                Debug.Log("Voice Command: Switched to Previous Weapon");
            }
        }
        else
        {
            Debug.Log("Voice Command not recognized: " + command);
        }
    }

}
