﻿using OpenTK.Audio.OpenAL;

namespace Hypercube.Client.Audio.Realisations.OpenAL;

public sealed partial class OpenAlAudioManager 
{
    private class AudioSource : IAudioSource
    {
        private readonly int _source;
        
        public IAudioGroup? Group { get; }
        
        public bool Playing => State == ALSourceState.Playing;

        public bool Looping
        {
            get => GetSource(ALSourceb.Looping);
            set => SetSource(ALSourceb.Looping, value);
        }

        public float Pitch
        {
            get => GetSource(ALSourcef.Pitch);
            set => SetSource(ALSourcef.Pitch, value);
        }

        public float Gain
        {
            get => GetSource(ALSourcef.Gain);
            set => SetSource(ALSourcef.Gain, value);
        }

        private ALSourceState State => (ALSourceState) AL.GetSource(_source, ALGetSourcei.SourceState);
        
        public AudioSource(int source)
        {
            _source = source;
        }
        
        public void Start()
        {
            AL.SourcePlay(_source);
        }

        public void Stop()
        {
            AL.SourceStop(_source);
        }

        public void Pause()
        {
            AL.SourcePause(_source);
        }
        
        public void Restart()
        {
            AL.SourceRewind(_source);
            AL.SourcePlay(_source);
        }
        
        public void Dispose()
        {
            AL.DeleteSource(_source);
        }

        private void SetSource(ALSourceb target, bool value)
        {
            AL.Source(_source, target, value);
        }
        
        private void SetSource(ALSourcef target, float value)
        {
            AL.Source(_source, target, value);
        }
        
        private bool GetSource(ALSourceb target)
        {
            return AL.GetSource(_source, target);
        }
        
        private float GetSource(ALSourcef target)
        {
            return AL.GetSource(_source, target);
        }
    }
}