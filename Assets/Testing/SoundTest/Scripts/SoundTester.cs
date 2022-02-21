using System;
using System.Collections;
using UnityEngine;
namespace Base.SoundManagement {
    public class SoundTester : MonoBehaviour {
        private IEnumerator Start() {
            yield return new WaitForSeconds(.01f);
            Sound a = Enum_MainMusicCollection.Explosion_sfx_1.GetSound();
            GameObject obj = new GameObject("SoundTester");
            Vector3 pos = Camera.main.transform.position;
            pos.x -= 10;
            obj.transform.position = pos;
            a.SetSoundSource(obj).PlaySound().SetLoop(0);
            yield return new WaitForSeconds(5);
            // a.StopSound();
        }
    }
}