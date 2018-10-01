using UnityEngine;
using System.Collections;

//It is common to create a class to contain all of your
//extension methods. This class must be static.
public static class ExtensionMethods
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static void PlayParticleSystemFor(this ParticleSystem ps, float duration)
    {
        ParticleStop pstop = ps.gameObject.AddComponent<ParticleStop>();
        pstop.StartCoroutine(pstop.PlayAndStop(duration));
    }

    public static void Freeze(this MonoBehaviour carrier, float duration){
        carrier.StartCoroutine(FreezeRoutine(duration));
    }

    public static IEnumerator FreezeRoutine(float duration){
        float oldTimeScale = Time.timeScale;
		Time.timeScale = 0f;
		yield return new WaitForSecondsRealtime(duration);
		Time.timeScale = oldTimeScale;
    }
}

[RequireComponent(typeof(ParticleSystem))]
public class ParticleStop : MonoBehaviour{
    public IEnumerator PlayAndStop(float duration){
        ParticleSystem ps = GetComponent<ParticleSystem>();
        yield return new WaitUntil(() => ps != null);
        ps.Play();
        yield return new WaitForSeconds(duration);
        ps.Stop();
        Destroy(this);
    }
}