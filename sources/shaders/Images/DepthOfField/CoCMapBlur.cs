﻿using SiliconStudio.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconStudio.Paradox.Effects.Images
{
    /// <summary>
    /// Blurs a Circle of Confusion map.
    /// </summary>
    /// <remarks>
    /// This is useful to avoid strong CoC changes leading to out-of-focus silhouette outline appearing in 
    /// front of another out-of-focus object.
    /// Internally it uses a special gaussian blur aware of the depth.
    /// </remarks>
    public class CoCMapBlur : ImageEffect
    {
        private ImageEffect cocBlurEffect;

        private float radius;

        private int tapCount;

        private Vector2[] tapWeights;

        private bool weightsDirty = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoCMapBlur"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CoCMapBlur(DrawEffectContext context)
            : base(context)
        {
            cocBlurEffect = new ImageEffectShader(context, "CoCMapBlurEffect");
            Radius = 5f;
        }

        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>The radius.</value>
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Radius cannot be < 0");
                }

                weightsDirty = (radius != value);
                radius = value;
            }
        }

        protected override void DrawCore()
        {
            // Updates the weight array if necessary
            if (weightsDirty || tapCount == 0)
            {
                weightsDirty = false;
                Vector2[] gaussianWeights = GaussianUtil.Calculate1D((int)radius, 2f, true);
                tapCount = gaussianWeights.Length;
                tapWeights = gaussianWeights;
            }

            var originalTexture = GetSafeInput(0);
            var outputTexture = GetSafeOutput(0);

            cocBlurEffect.Parameters.Set(DepthAwareDirectionalBlurKeys.Count, tapCount);
            cocBlurEffect.Parameters.Set(CoCMapBlurShaderKeys.Radius, radius);
            cocBlurEffect.Parameters.Set(CoCMapBlurShaderKeys.OffsetsWeights, tapWeights);
            var tapNumber = 2 * tapCount - 1;

            // Blur in one direction
            var blurAngle = 0f;
            cocBlurEffect.Parameters.Set(CoCMapBlurShaderKeys.Direction, new Vector2((float)Math.Cos(blurAngle), (float)Math.Sin(blurAngle)));

            var firstBlurTexture = NewScopedRenderTarget2D(originalTexture.Description);
            cocBlurEffect.SetInput(0, originalTexture);
            cocBlurEffect.SetOutput(firstBlurTexture);
            cocBlurEffect.Draw("CoCMapBlurPass1_tap{0}_radius{1}", tapNumber, (int)radius);

            // Second blur pass to ouput the final result
            blurAngle = MathUtil.PiOverTwo;
            cocBlurEffect.Parameters.Set(CoCMapBlurShaderKeys.Direction, new Vector2((float)Math.Cos(blurAngle), (float)Math.Sin(blurAngle)));

            cocBlurEffect.SetInput(0, firstBlurTexture);
            cocBlurEffect.SetOutput(outputTexture);
            cocBlurEffect.Draw("CoCMapBlurPass2_tap{0}_radius{1}", tapNumber, (int)radius);
        }

    }
}
