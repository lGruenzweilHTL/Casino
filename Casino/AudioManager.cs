using NAudio.Wave;

namespace Casino;

public static class AudioManager {
    /// <summary>
    /// Plays the sound at the specified location once
    /// </summary>
    /// <param name="fileName">The location of the sound to play</param>
    /// <returns>The player-thread of the sound</returns>
    /// <exception cref="NullReferenceException">If there is no file at the specified location</exception>
    public static Thread PlayAudio(string fileName) {
        if (!File.Exists(fileName)) throw new NullReferenceException("No file at specified path");

        AudioFileReader sound = new(fileName);
        Thread t = new(AudioCode);
        t.Start();
        return t;

        void AudioCode() {
            using WaveOutEvent outputDevice = new();

            outputDevice.Init(sound);
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing) {
                Thread.Sleep(1000);
            }
        }
    }

    /// <summary>
    /// Plays the sound at the specified location until cancelled
    /// </summary>
    /// <param name="fileName">The location of the sound to play</param>
    /// <param name="cancellationToken">The token to cancel playing. Playing will be stopped as soon as the current iteration is finished</param>
    /// <exception cref="NullReferenceException">If there is no file at the specified location</exception>
    public static void PlayLooping(string fileName, CancellationToken cancellationToken) {
        new Thread(() => {
            while (!cancellationToken.IsCancellationRequested) {
                PlayAudio(fileName).Join();
            }
        }).Start();
    }
}