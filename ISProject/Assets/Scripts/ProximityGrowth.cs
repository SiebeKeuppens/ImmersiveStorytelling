using UnityEngine;

public class ProximityGrowth : MonoBehaviour
{
    [Header("Proximity Settings")]
    [Tooltip("The distance within which the object will start growing")]
    public float detectionRadius = 5f;

    [Tooltip("The speed at which objects grow")]
    public float growthSpeed = 1f;

    [Tooltip("The speed at which objects shrink (can be slower than growth)")]
    public float shrinkSpeed = 0.5f;

    [Tooltip("The maximum scale objects can grow to")]
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);

    [Tooltip("The minimum scale of objects")]
    public Vector3 minScale = Vector3.one;

    [Tooltip("The maximum intensity of lights")]
    public float maxLightIntensity = 10f;
    [Tooltip("The minimum intensity of lights")]
    public float minLightIntensity = 1f;

    [Header("Detection Settings")]
    [Tooltip("Tag of the player object")]
    public string playerTag = "Player";

    [Tooltip("Tag for objects that can grow")]
    public string growableTag = "CanGrow";

    private Transform player;

    void Start()
    {
        // Find the player object by tag
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Find all objects with the CanGrow tag in each update
        GameObject[] growableObjects = GameObject.FindGameObjectsWithTag(growableTag);

        foreach (GameObject obj in growableObjects)
        {
            // Calculate the distance between the player and this specific object
            float distanceToPlayer = Vector3.Distance(player.position, obj.transform.position);

            // Check if the player is within the detection radius of this object
            if (distanceToPlayer <= detectionRadius)
            {
                GrowObject(obj);
            }
            else
            {
                ShrinkObject(obj);
            }
        }
    }

     void GrowObject(GameObject obj)
    {
        // Retrieve or create the original scale tracker
        if (!obj.TryGetComponent(out ObjectScaleTracker tracker))
        {
            tracker = obj.AddComponent<ObjectScaleTracker>();
            tracker.originalScale = obj.transform.localScale;
        }

        // Smoothly interpolate the scale towards the maximum scale
        obj.transform.localScale = Vector3.Lerp(
            obj.transform.localScale, 
            maxScale, 
            growthSpeed * Time.deltaTime
        );

        // Adjust light intensity
        if (obj.TryGetComponent(out Light pointLight))
        {
            pointLight.range = Mathf.Lerp(pointLight.range, maxLightIntensity, growthSpeed * Time.deltaTime);
            pointLight.intensity = pointLight.range;
        }
    }

    void ShrinkObject(GameObject obj)
    {
        // Retrieve the original scale tracker
        if (obj.TryGetComponent(out ObjectScaleTracker tracker))
        {
            // Smoothly interpolate the scale back to the original scale
            obj.transform.localScale = Vector3.Lerp(
                obj.transform.localScale, 
                tracker.originalScale, 
                shrinkSpeed * Time.deltaTime
            );
        }

        // Adjust light intensity but really slowly
        if (obj.TryGetComponent(out Light pointLight))
        {
            pointLight.range = Mathf.Lerp(pointLight.range, minLightIntensity, shrinkSpeed * Time.deltaTime/5);
            pointLight.intensity = pointLight.range;
        }
    }

    // Helper component to store original scale
    private class ObjectScaleTracker : MonoBehaviour
    {
        public Vector3 originalScale;
    }

    // Optional: Visualize the detection radius in the scene view for each object
    void OnDrawGizmosSelected()
    {
        // Find all growable objects in the editor
        GameObject[] growableObjects = GameObject.FindGameObjectsWithTag(growableTag);
        
        foreach (GameObject obj in growableObjects)
        {
            // Draw a wire sphere to show the detection radius in the Unity editor
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(obj.transform.position, detectionRadius);
        }
    }
}