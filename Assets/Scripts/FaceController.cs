using UnityEngine;
using System.Collections.Generic;

public class FaceController : MonoBehaviour
{
    [Header("References")]
    public SkinnedMeshRenderer faceRenderer;

    [Header("Transition Settings")]
    public float transitionSpeed = 200f; // higher = faster transition

    private Mesh faceMesh;
    private float[] targetWeights;

    void Start()
    {
        if (faceRenderer == null)
        {
            Debug.LogError("FaceController: No SkinnedMeshRenderer assigned.");
            return;
        }

        faceMesh = faceRenderer.sharedMesh;

        if (faceMesh == null)
        {
            Debug.LogError("FaceController: No mesh found on the SkinnedMeshRenderer.");
            return;
        }

        targetWeights = new float[faceMesh.blendShapeCount];

        // Start with current weights as targets
        for (int i = 0; i < faceMesh.blendShapeCount; i++)
        {
            targetWeights[i] = faceRenderer.GetBlendShapeWeight(i);
        }
    }

    void Update()
    {
        if (faceRenderer == null || faceMesh == null) return;

        HandleInput();
        UpdateBlendShapesSmoothly();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetTargets();
            SetTargetShape("EyeClose_L", 100f);
            SetTargetShape("EyeClose_R", 100f);
            SetTargetShape("Smile_L", 100f);
            SetTargetShape("Smile_R", 100f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetTargets();
            SetTargetShape("EyebrowDown_L", 100f);
            SetTargetShape("EyebrowDown_R", 100f);
            SetTargetShape("EyeClose_L", 13f);
            SetTargetShape("EyeClose_R", 13f);
            SetTargetShape("MouthDown_L", 100f);
            SetTargetShape("MouthDown_R", 100f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetTargets();
            SetTargetShape("Smile_L", 100f);
            SetTargetShape("EyeClose_L", 100f);
            SetTargetShape("EyebrowDown_L", 100f);
            SetTargetShape("MouthDown_R", 40f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ResetTargets();
        }
    }

    void UpdateBlendShapesSmoothly()
    {
        for (int i = 0; i < faceMesh.blendShapeCount; i++)
        {
            float currentWeight = faceRenderer.GetBlendShapeWeight(i);
            float newWeight = Mathf.MoveTowards(
                currentWeight,
                targetWeights[i],
                transitionSpeed * Time.deltaTime
            );

            faceRenderer.SetBlendShapeWeight(i, newWeight);
        }
    }

    void SetTargetShape(string shapeName, float weight)
    {
        int index = faceMesh.GetBlendShapeIndex(shapeName);

        if (index >= 0)
        {
            targetWeights[index] = weight;
        }
        else
        {
            Debug.LogWarning("Blend shape not found: " + shapeName);
        }
    }

    void ResetTargets()
    {
        for (int i = 0; i < targetWeights.Length; i++)
        {
            targetWeights[i] = 0f;
        }
    }
}