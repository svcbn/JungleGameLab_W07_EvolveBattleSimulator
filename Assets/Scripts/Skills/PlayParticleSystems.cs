using UnityEngine;

public class PlayParticleSystems : MonoBehaviour
{

    [SerializeField] private ParticleSystem[] _particleSystems;

    public void Play()
    {
        if( _particleSystems        == null){ Debug.LogWarning("_particleSystems is null");     return; }
        if( _particleSystems.Length == 0   ){ Debug.LogWarning("_particleSystems.Length is 0"); return; }

        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Play();
        }
    }

    public void Stop()
    {
        if( _particleSystems        == null){ Debug.LogWarning("_particleSystems is null");     return; }
        if( _particleSystems.Length == 0   ){ Debug.LogWarning("_particleSystems.Length is 0"); return; }

        foreach (var particleSystem in _particleSystems)
        {
            particleSystem.Stop();
        }
    }
}
