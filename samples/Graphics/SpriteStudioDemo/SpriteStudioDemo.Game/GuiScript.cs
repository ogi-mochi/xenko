using SiliconStudio.Core.Mathematics;
using SiliconStudio.Xenko.Engine;
using SiliconStudio.Xenko.Graphics;
using SiliconStudio.Xenko.UI;
using SiliconStudio.Xenko.UI.Controls;
using SiliconStudio.Xenko.UI.Panels;

namespace SpriteStudioDemo
{
    /// <summary>
    /// The GUI script
    /// </summary>
    public class GuiScript : StartupScript
    {
		public SpriteFont Font { get; set; }
		
        public override void Start()
        {
            var font = Font;
            var textBlock = new TextBlock
            {
                Font = font,
                TextSize = 18,
                TextColor = Color.Gold,
                Text = "Shoot : Touch in a vertical section where the Agent resides\n" +
                       "Move : Touch in the screen on the corresponding side of the Agent",
            };
            textBlock.SetCanvasRelativePosition(new Vector3(0.008f, 0.9f, 0));

            Entity.Get<UIComponent>().RootElement = new Canvas { Children = { textBlock } };
        }
    }
}
