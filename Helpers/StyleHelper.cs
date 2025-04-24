using Microsoft.Maui.Controls.Shapes;

namespace RestoGestApp.Helpers;

public static class StyleHelper
{
    // Colors
    public static readonly Color PrimaryColor = Color.Parse("#FEBE10");
    public static readonly Color SecondaryColor = Color.Parse("#333333");
    public static readonly Color AccentColor = Color.Parse("#FF5252");
    public static readonly Color BackgroundColor = Color.Parse("#f8f8f8");
    public static readonly Color SurfaceColor = Colors.White;
    public static readonly Color TextPrimaryColor = Color.Parse("#333333");
    public static readonly Color TextSecondaryColor = Color.Parse("#666666");
    public static readonly Color TextTertiaryColor = Color.Parse("#999999");
    
    // Button Styles
    public static Style CreatePrimaryButtonStyle()
    {
        var style = new Style(typeof(Button));
        style.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = PrimaryColor });
        style.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = Colors.White });
        style.Setters.Add(new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold });
        style.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = 10 });
        style.Setters.Add(new Setter { Property = Button.HeightRequestProperty, Value = 50 });
        return style;
    }
    
    public static Style CreateSecondaryButtonStyle()
    {
        var style = new Style(typeof(Button));
        style.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Transparent });
        style.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = PrimaryColor });
        style.Setters.Add(new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold });
        style.Setters.Add(new Setter { Property = Button.BorderColorProperty, Value = PrimaryColor });
        style.Setters.Add(new Setter { Property = Button.BorderWidthProperty, Value = 1 });
        style.Setters.Add(new Setter { Property = Button.CornerRadiusProperty, Value = 10 });
        style.Setters.Add(new Setter { Property = Button.HeightRequestProperty, Value = 50 });
        return style;
    }
    
    public static Style CreateTextButtonStyle()
    {
        var style = new Style(typeof(Button));
        style.Setters.Add(new Setter { Property = Button.BackgroundColorProperty, Value = Colors.Transparent });
        style.Setters.Add(new Setter { Property = Button.TextColorProperty, Value = PrimaryColor });
        style.Setters.Add(new Setter { Property = Button.FontAttributesProperty, Value = FontAttributes.Bold });
        style.Setters.Add(new Setter { Property = Button.BorderWidthProperty, Value = 0 });
        style.Setters.Add(new Setter { Property = Button.PaddingProperty, Value = new Thickness(0) });
        return style;
    }
    
    // Card Style
    public static Border CreateCard(View content)
    {
        if (content == null)
            throw new ArgumentNullException(nameof(content));
            
        var border = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            Stroke = Colors.Transparent,
            BackgroundColor = SurfaceColor,
            Padding = new Thickness(15),
            Content = content
        };
        
        border.Shadow = new Shadow
        {
            Brush = Brush.Black,
            Offset = new Point(0, 2),
            Opacity = 0.1f,
            Radius = 4
        };
        
        return border;
    }
    
    // Entry Style
    public static Style CreateEntryStyle()
    {
        var style = new Style(typeof(Entry));
        style.Setters.Add(new Setter { Property = Entry.BackgroundColorProperty, Value = Color.Parse("#f5f5f5") });
        style.Setters.Add(new Setter { Property = Entry.TextColorProperty, Value = TextPrimaryColor });
        style.Setters.Add(new Setter { Property = Entry.PlaceholderColorProperty, Value = TextTertiaryColor });
        style.Setters.Add(new Setter { Property = Entry.HeightRequestProperty, Value = 50 });
        // Entry doesn't have a PaddingProperty in .NET MAUI
        // style.Setters.Add(new Setter { Property = Entry.PaddingProperty, Value = new Thickness(15, 0) });
        return style;
    }
}
