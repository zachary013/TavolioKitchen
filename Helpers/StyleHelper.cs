using Microsoft.Maui.Controls;
using System;

namespace RestoGestApp.Helpers
{
    /// <summary>
    /// Helper class to provide consistent access to application styles from C# code
    /// </summary>
    public static class StyleHelper
    {
        // Font Sizes
        public static double FontSizeSmall => GetDoubleResource("FontSizeSmall", 12);
        public static double FontSizeNormal => GetDoubleResource("FontSizeNormal", 14);
        public static double FontSizeMedium => GetDoubleResource("FontSizeMedium", 16);
        public static double FontSizeLarge => GetDoubleResource("FontSizeLarge", 18);
        public static double FontSizeExtraLarge => GetDoubleResource("FontSizeExtraLarge", 20);
        public static double FontSizeHeader => GetDoubleResource("FontSizeHeader", 24);
        public static double FontSizeTitle => GetDoubleResource("FontSizeTitle", 28);
        public static double FontSizeBrand => GetDoubleResource("FontSizeBrand", 32);

        // Spacing
        public static double SpacingSmall => GetDoubleResource("SpacingSmall", 5);
        public static double SpacingMedium => GetDoubleResource("SpacingMedium", 10);
        public static double SpacingLarge => GetDoubleResource("SpacingLarge", 15);
        public static double SpacingExtraLarge => GetDoubleResource("SpacingExtraLarge", 20);

        // Corner Radius
        public static double ButtonCornerRadius => GetDoubleResource("ButtonCornerRadius", 25);
        public static double SmallButtonCornerRadius => GetDoubleResource("SmallButtonCornerRadius", 20);
        public static double CardCornerRadius => GetDoubleResource("CardCornerRadius", 10);

        // Element Sizes
        public static double ButtonHeight => GetDoubleResource("ButtonHeight", 40);
        public static double SmallButtonHeight => GetDoubleResource("SmallButtonHeight", 35);
        public static double IconButtonSize => GetDoubleResource("IconButtonSize", 40);

        // Styles
        // Button Styles
        public static Style PrimaryButtonStyle => GetStyleResource("PrimaryButtonStyle");
        public static Style RoundedButtonStyle => GetStyleResource("RoundedButtonStyle");
        public static Style PlainButtonStyle => GetStyleResource("PlainButtonStyle");
        public static Style OutlinedButtonStyle => GetStyleResource("OutlinedButtonStyle");
        public static Style BrandButtonStyle => GetStyleResource("BrandButtonStyle");
        public static Style BrandOutlinedButtonStyle => GetStyleResource("BrandOutlinedButtonStyle");
        public static Style SmallButtonStyle => GetStyleResource("SmallButtonStyle");
        public static Style BrandSmallButtonStyle => GetStyleResource("BrandSmallButtonStyle");
        public static Style DestructiveButtonStyle => GetStyleResource("DestructiveButtonStyle");
        public static Style DestructivePlainButtonStyle => GetStyleResource("DestructivePlainButtonStyle");
        public static Style AddToCartButtonStyle => GetStyleResource("AddToCartButtonStyle");
        public static Style QuantityButtonStyle => GetStyleResource("QuantityButtonStyle");
        public static Style BrandQuantityButtonStyle => GetStyleResource("BrandQuantityButtonStyle");

        // Label and Border Styles
        public static Style HeaderLabelStyle => GetStyleResource("HeaderLabel");
        public static Style TitleLabelStyle => GetStyleResource("TitleLabel");
        public static Style BrandLabelStyle => GetStyleResource("BrandLabel");
        public static Style SubtitleLabelStyle => GetStyleResource("SubtitleLabel");
        public static Style PriceLabelStyle => GetStyleResource("PriceLabel");
        public static Style CardBorderStyle => GetStyleResource("CardBorder");
        public static Style CategoryBorderStyle => GetStyleResource("CategoryBorder");

        /// <summary>
        /// Gets a double value from the application resources
        /// </summary>
        /// <param name="resourceKey">The resource key</param>
        /// <param name="defaultValue">Default value if resource not found</param>
        /// <returns>The double value, or the default if not found</returns>
        private static double GetDoubleResource(string resourceKey, double defaultValue)
        {
            if (Application.Current?.Resources.TryGetValue(resourceKey, out var value) == true)
            {
                if (value is double doubleValue)
                {
                    return doubleValue;
                }
            }

            System.Diagnostics.Debug.WriteLine($"Warning: Double resource '{resourceKey}' not found");
            return defaultValue;
        }

        /// <summary>
        /// Gets a style from the application resources
        /// </summary>
        /// <param name="resourceKey">The resource key of the style</param>
        /// <returns>The style, or null if not found</returns>
        private static Style GetStyleResource(string resourceKey)
        {
            if (Application.Current?.Resources.TryGetValue(resourceKey, out var value) == true)
            {
                if (value is Style style)
                {
                    return style;
                }
            }

            System.Diagnostics.Debug.WriteLine($"Warning: Style resource '{resourceKey}' not found");
            return null;
        }
    }
}
