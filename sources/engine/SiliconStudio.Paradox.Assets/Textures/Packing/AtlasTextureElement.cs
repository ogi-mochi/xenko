using SiliconStudio.Core.Mathematics;
using SiliconStudio.Paradox.Graphics;

namespace SiliconStudio.Paradox.Assets.Textures.Packing
{
    /// <summary>
    /// This represent an element of the atlas texture.
    /// </summary>
    public class AtlasTextureElement
    {
        /// <summary>
        /// The name of the atlas element.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets or sets CPU-resource texture
        /// </summary>
        public Image Texture;

        /// <summary>
        /// The region of the atlas element in its original texture.
        /// </summary>
        public Rectangle SourceRegion;

        /// <summary>
        /// The region of the atlas element in the output atlas texture (it includes the border size!).
        /// </summary>
        public RotableRectangle DestinationRegion;

        /// <summary>
        /// The size of the border around the atlas elements
        /// </summary>
        public int BorderSize;

        /// <summary>
        /// Gets or sets border modes in X axis which applies specific TextureAddressMode in the border of each texture element in a given size of border
        /// </summary>
        public TextureAddressMode BorderModeU;

        /// <summary>
        /// Gets or sets border modes in Y axis which applies specific TextureAddressMode in the border of each texture element in a given size of border
        /// </summary>
        public TextureAddressMode BorderModeV;

        /// <summary>
        /// Gets or sets Border color when BorderModeU is set to Border mode
        /// </summary>
        public Color BorderColor;

        /// <summary>
        /// Create an empty atlas texture element.
        /// </summary>
        public AtlasTextureElement()
        {
        }

        /// <summary>
        /// Create an atlas texture element that contains all the information from the source texture and a default destination region.
        /// </summary>
        /// <param name="name">The reference name of the element</param>
        /// <param name="texture"></param>
        /// <param name="sourceRegion">The region of the element in the source texture</param>
        /// <param name="borderSize">The size of the border around the element in the output atlas</param>
        /// <param name="borderModeU">The border mode along the U axis</param>
        /// <param name="borderModeV">The border mode along the V axis</param>
        /// <param name="borderColor">The color of the border</param>
        public AtlasTextureElement(string name, Image texture, Rectangle sourceRegion, int borderSize, TextureAddressMode borderModeU, TextureAddressMode borderModeV, Color? borderColor = null)
        {
            Name = name;
            Texture = texture;
            SourceRegion = sourceRegion;
            BorderSize = borderSize;
            BorderModeU = borderModeU;
            BorderModeV = borderModeV;
            BorderColor = borderColor ?? Color.Transparent;
            DestinationRegion = new RotableRectangle
            {
                Width = SourceRegion.Width + 2 * borderSize,
                Height = SourceRegion.Height + 2 * borderSize,
            };
        }

        /// <summary>
        /// Clone the current element.
        /// </summary>
        /// <returns>A copy of the current element</returns>
        public AtlasTextureElement Clone()
        {
            return (AtlasTextureElement)MemberwiseClone();
        }
    }
}