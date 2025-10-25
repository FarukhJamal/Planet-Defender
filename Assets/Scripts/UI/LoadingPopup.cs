using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPopup : MonoBehaviour
{
    [SerializeField] private Transform loadingCircleImage;
    [SerializeField] private float spinSpeed;

    private float currentSpinSpeed;

    private void Start()
    {
        currentSpinSpeed = 0.0f;
    }

    private void Update()
    {
        currentSpinSpeed += spinSpeed * Time.deltaTime;
        loadingCircleImage.transform.localEulerAngles = new Vector3(0f, 0f, currentSpinSpeed);
    }
}
