using UnityEngine;

public class ColorSampler : MonoBehaviour
{
    public Texture2D paletteTexture;
    public Material material;
    public int paletteGridRows = 1;
    public int paletteGridColumns = 1;

    private void Start()
    {
        float gridWidth = 1f / paletteGridColumns;
        float gridHeight = 1f / paletteGridRows;

        // Randomly select a color from the palette
        int row = Random.Range(0, paletteGridRows);
        int column = Random.Range(0, paletteGridColumns);
        float u = column * gridWidth;
        float v = row * gridHeight;
        Color color = paletteTexture.GetPixelBilinear(u, v);

        // Apply the selected color to the material
        material.color = color;
    }
}
