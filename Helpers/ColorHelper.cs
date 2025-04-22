using Microsoft.Maui.Graphics;

namespace RestoGestApp.Helpers
{
    /// <summary>
    /// Helper class to provide consistent access to application colors from C# code
    /// </summary>
    public static class ColorHelper
    {
        // Brand Colors
        public static Color BrandYellow => GetColorFromResource("BrandYellow");
        public static Color BrandYellowDark => GetColorFromResource("BrandYellowDark");
        public static Color BrandYellowLight => GetColorFromResource("BrandYellowLight");
        public static Color BrandBlack => GetColorFromResource("BrandBlack");
        public static Color BrandWhite => GetColorFromResource("BrandWhite");
        public static Color BrandGray => GetColorFromResource("BrandGray");
        
        // Semantic Colors
        public static Color Primary => GetColorFromResource("Primary");
        public static Color PrimaryDark => GetColorFromResource("PrimaryDark");
        public static Color PrimaryLight => GetColorFromResource("PrimaryLight");
        public static Color Secondary => GetColorFromResource("Secondary");
        public static Color Tertiary => GetColorFromResource("Tertiary");
        
        // UI Element Colors
        public static Color ButtonBackground => GetColorFromResource("ButtonBackground");
        public static Color ButtonText => GetColorFromResource("ButtonText");
        public static Color HeaderText => GetColorFromResource("HeaderText");
        public static Color PriceText => GetColorFromResource("PriceText");
        public static Color BorderColor => GetColorFromResource("BorderColor");
        public static Color PageBackground => GetColorFromResource("PageBackground");
        public static Color CardBackground => GetColorFromResource("CardBackground");
        public static Color SuccessText => GetColorFromResource("SuccessText");
        public static Color WarningText => GetColorFromResource("WarningText");
        public static Color ErrorText => GetColorFromResource("ErrorText");
        
        /// <summary>
        /// Gets a color from the application resources
        /// </summary>
        /// <param name="resourceKey">The resource key of the color</param>
        /// <returns>The color, or a fallback color if not found</returns>
        private static Color GetColorFromResource(string resourceKey)
        {
            if (Application.Current?.Resources.TryGetValue(resourceKey, out var value) == true)
            {
                if (value is Color color)
                {
                    return color;
                }
            }
            
            // Fallback to a default color if the resource is not found
            System.Diagnostics.Debug.WriteLine($"Warning: Color resource '{resourceKey}' not found");
            return Colors.Gray;
        }
    }
}
