using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animations : MonoBehaviour
{
    public RectTransform targetImageRectTransform;
    public Vector3 scaleFactor = new Vector3(1.2f, 1.2f, 1.0f);
    public Vector3 originalScale;
    private bool isScaled = false;

    public Image targetUIImage;
    private bool isFaded = false;

    private Coroutine zoomCoroutine;
    private Coroutine rotateCoroutine;
    private Coroutine moveLeftCoroutine;
    private Coroutine moveRightCoroutine;

    private bool isRotating = false;
    private bool isMovedLeft = false;
    private bool isMovedRight = false;

    private Vector3 originalPosition;

    void Start()
    {
        originalScale = targetImageRectTransform.localScale;
        originalPosition = targetImageRectTransform.localPosition;
    }

    void Update()
    {

    }

    public void ScaleAnim()
    {
        targetImageRectTransform.localScale = isScaled ? originalScale : new Vector3(originalScale.x * scaleFactor.x, originalScale.y * scaleFactor.y, originalScale.z * scaleFactor.z);
        isScaled = !isScaled;
    }

    public void FadeAnim()
    {
        if (isFaded)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
        isFaded = !isFaded;
    }

    public void ToggleZoom()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }

        if (isScaled)
        {
            zoomCoroutine = StartCoroutine(ZoomOut());
        }
        else
        {
            zoomCoroutine = StartCoroutine(ZoomIn());
        }
        isScaled = !isScaled;
    }

    public void ToggleRotate()
    {
        if (isRotating)
        {
            StopRotate();
        }
        else
        {
            StartRotate();
        }
        isRotating = !isRotating;
    }

    public void StartRotate()
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }

        rotateCoroutine = StartCoroutine(RotateImage());
    }

    public void StopRotate()
    {
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
    }

    public void ToggleMoveLeft()
    {
        if (isMovedLeft)
        {
            MoveBackToOriginalPosition();
        }
        else
        {
            MoveLeft();
        }
        isMovedLeft = !isMovedLeft;
    }

    public void ToggleMoveRight()
    {
        if (isMovedRight)
        {
            MoveBackToOriginalPosition();
        }
        else
        {
            MoveRight();
        }
        isMovedRight = !isMovedRight;
    }

    public void MoveLeft()
    {
        if (moveLeftCoroutine != null)
        {
            StopCoroutine(moveLeftCoroutine);
        }

        Vector3 targetPosition = originalPosition - new Vector3(100f, 0f, 0f);
        moveLeftCoroutine = StartCoroutine(MoveToPosition(targetPosition));
    }

    public void MoveRight()
    {
        if (moveRightCoroutine != null)
        {
            StopCoroutine(moveRightCoroutine);
        }

        Vector3 targetPosition = originalPosition + new Vector3(100f, 0f, 0f);
        moveRightCoroutine = StartCoroutine(MoveToPosition(targetPosition));
    }

    public void MoveBackToOriginalPosition()
    {
        if (isMovedLeft)
        {
            MoveLeft();
        }
        else if (isMovedRight)
        {
            MoveRight();
        }
    }

    IEnumerator FadeIn()
    {
        float targetAlpha = 1.0f;
        while (targetUIImage.color.a < targetAlpha)
        {
            Color newColor = targetUIImage.color;
            newColor.a += Time.deltaTime * 2;
            targetUIImage.color = newColor;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float targetAlpha = 0.0f;
        while (targetUIImage.color.a > targetAlpha)
        {
            Color newColor = targetUIImage.color;
            newColor.a -= Time.deltaTime * 2;
            targetUIImage.color = newColor;
            yield return null;
        }
    }

    IEnumerator ZoomOut()
    {
        float targetScale = 0.0f; 
        Vector3 originalPosition = targetImageRectTransform.localPosition;
        while (targetImageRectTransform.localScale.x > targetScale)
        {
            targetImageRectTransform.localScale -= Vector3.one * Time.deltaTime * 2;
            targetImageRectTransform.localPosition = Vector3.Lerp(targetImageRectTransform.localPosition, originalPosition, Time.deltaTime * 2);
            yield return null;
        }
        targetImageRectTransform.localScale = Vector3.one * targetScale;
    }

    IEnumerator ZoomIn()
    {
        float targetScale = 1.0f;
        Vector3 originalPosition = targetImageRectTransform.localPosition;
        while (targetImageRectTransform.localScale.x < targetScale)
        {
            targetImageRectTransform.localScale += Vector3.one * Time.deltaTime * 2;
            targetImageRectTransform.localPosition = Vector3.Lerp(targetImageRectTransform.localPosition, originalPosition, Time.deltaTime * 2);
            yield return null;
        }
        targetImageRectTransform.localScale = Vector3.one * targetScale;
    }

    IEnumerator RotateImage()
    {
        float targetRotation = 360f;
        while (targetImageRectTransform.localEulerAngles.z < targetRotation)
        {
            Vector3 newRotation = targetImageRectTransform.localEulerAngles;
            newRotation.z += Time.deltaTime * 90;
            targetImageRectTransform.localEulerAngles = newRotation;
            yield return null;
        }
        targetImageRectTransform.localEulerAngles = new Vector3(0, 0, targetRotation);
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float duration = 1.0f;
        float elapsedTime = 0.0f;
        Vector3 startPosition = targetImageRectTransform.localPosition;

        while (elapsedTime < duration)
        {
            targetImageRectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetImageRectTransform.localPosition = targetPosition;
    }
}
