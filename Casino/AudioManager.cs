using NAudio.Wave;

namespace Casino;

public static class AudioManager {
    private static AudioFileReader? _currentSound;
    public static Thread PlayAudio(string fileName) {
        _currentSound = new AudioFileReader(fileName);
        Thread t = new (AudioCode);
        t.Start();
        return t;
    }

    private static void AudioCode() {
        using WaveOutEvent outputDevice = new();
        
        outputDevice.Init(_currentSound);
        outputDevice.Play();
        while (outputDevice.PlaybackState == PlaybackState.Playing)
        {
            Thread.Sleep(1000);
        }
    }
}