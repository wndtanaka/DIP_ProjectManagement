using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    //public IEnumerator RecoilKickBack(float duration, float magnitude)
    //{
    //    Vector3 originalPos = transform.localPosition;

    //    float elapsed = 0f;

    //    while (elapsed < duration)
    //    {
    //        float x = Random.Range(-0.5f, 0.1f) * magnitude;
    //        float y = Random.Range(-0.5f, 0.5f) * magnitude;

    //        transform.localPosition = new Vector3(x, y, originalPos.z);

    //        elapsed += Time.deltaTime;

    //        yield return null;
    //    }

    //    transform.localEulerAngles = originalPos;

    //}

    private float recoil;
    private float maxRecoil_x;
    private float maxRecoil_y;
    private float recoilSpeed;

    public void StartRecoil(float recoilParam, float maxRecoil_xParam, float recoilSpeedParam)
    {
        // in seconds
        recoil = recoilParam;
        maxRecoil_x = maxRecoil_xParam;
        recoilSpeed = recoilSpeedParam;
        maxRecoil_y = Random.Range(-maxRecoil_xParam, maxRecoil_xParam);
    }

    void KickBack()
    {
        if (recoil > 0f)
        {
            Quaternion maxRecoil = Quaternion.Euler(maxRecoil_x, maxRecoil_y, 0f);
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
            recoil -= Time.deltaTime;
        }
        else
        {
            recoil = 0f;
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * recoilSpeed / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        KickBack();
    }
}
