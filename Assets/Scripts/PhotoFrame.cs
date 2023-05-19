using UnityEngine;

public class PhotoFrame : MonoBehaviour
{
    [SerializeField] private PhotoCategory category;
    [SerializeField] private MeshRenderer frame;
    [SerializeField] private Texture photo;

    private void Start()
    {
        if (photo == null)
        {
            return;
        }
        var mat = new Material(frame.material);
        mat.mainTexture = photo;
        frame.material = mat;
    }
}

public enum PhotoCategory
{
    Hunting,
    Agriculture,
    Fishing,
    Industry
}
