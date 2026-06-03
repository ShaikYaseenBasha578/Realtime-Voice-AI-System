# Real-Time Voice AI System

A Unity FPS game demo with an integrated real-time voice command system powered by the Hugging Face Speech-to-Text API. Speak into your microphone to control in-game actions — reload your weapon, add ammo, and switch guns — all hands-free.

A drone companion follows the player throughout the scene, giving the visual impression of an AI assistant actively listening to your commands.

---

## Demo

Single-scene FPS environment inspired by aim/shooting training maps (similar to Valorant's practice range). The focus is on the voice command pipeline rather than full game mechanics.

---

## Voice Commands

| Action | Trigger Phrases |
|---|---|
| Add Ammo / Reload | `"add ammo"`, `"reload"`, `"more ammo"` |
| Next Weapon | `"switch gun"`, `"change weapon"`, `"next weapon"` |
| Previous Weapon | `"previous weapon"`, `"last weapon"` |

---

## Controls

| Input | Action |
|---|---|
| `M` key | Toggle recording on/off |
| Start Button (UI) | Begin voice recording |
| Stop Button (UI) | Stop recording and send to API |

Recording captures up to **10 seconds** of audio at **44100Hz**, encodes it as WAV, and sends it to Hugging Face's Automatic Speech Recognition API. The transcribed text is matched against command phrases to trigger the corresponding in-game action.

---

## Tech Stack

- **Engine:** Unity
- **Language:** C#
- **AI:** Hugging Face API — Automatic Speech Recognition
- **Audio:** Unity Microphone API + custom WAV encoder
- **UI:** TextMeshPro

---

## Getting Started

### Prerequisites

- Unity (2021.3 or later recommended)
- [Hugging Face Unity API package](https://github.com/huggingface/unity-api)
- Infima Games Low Poly Shooter Pack (for weapon/inventory scripts)
- A microphone connected to your device

### Installation

1. Clone the repository
```
git clone https://github.com/ShaikYaseenBasha578/realtime-voice-ai-system
```

2. Open the project in Unity

3. Install the Hugging Face Unity API package via the Package Manager

4. Add your Hugging Face API key in the Hugging Face API settings inside Unity

5. Open the main scene and press **Play**

6. Allow microphone access when prompted

7. Press **M** or click **Start** to begin recording a voice command

---

## How It Works

```
Microphone Input
      ↓
WAV Encoding (custom encoder, 44100Hz)
      ↓
Hugging Face ASR API
      ↓
Transcribed Text
      ↓
Command Matching (HandleVoiceCommand)
      ↓
In-Game Action (ammo / weapon switch)
```

---

## Project Status

Active — built as a proof-of-concept for real-time voice-controlled gameplay.
