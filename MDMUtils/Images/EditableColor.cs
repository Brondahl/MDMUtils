using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MDMUtils.Images
{
  public class EditableColor
  {
    private Color currentColor;
    public EditableColor(Color color)
    {
      currentColor = color;
    }

    public int A
    {
      get { return currentColor.A; }
      set { currentColor = Color.FromArgb(value         , currentColor.R, currentColor.G, currentColor.B);}
    }

    public int R
    {
      get { return currentColor.R; }
      set { currentColor = Color.FromArgb(currentColor.A, value         , currentColor.G, currentColor.B);}
    }

    public int G
    {
      get { return currentColor.G; }
      set { currentColor = Color.FromArgb(currentColor.A, currentColor.R, value         , currentColor.B);}
    }

    public int B
    {
      get { return currentColor.B; }
      set { currentColor = Color.FromArgb(currentColor.A, currentColor.R, currentColor.G, value         );}
    }

    public static implicit operator Color (EditableColor x)  
    { 
       return x.currentColor;
    }

    public static explicit operator EditableColor(Color x) 
    { 
       return new EditableColor(x);
    }

    public Color ReduceIntensity(int percentage)
    {
      R = ReduceComponentIntensity(currentColor.R, percentage);
      B = ReduceComponentIntensity(currentColor.B, percentage);
      G = ReduceComponentIntensity(currentColor.G, percentage);

      return currentColor;
    }

    private int ReduceComponentIntensity(int componentValue, int percentage)
    {
      int invertedValue = (255 - componentValue);
      int scaledValue = (invertedValue*(100-percentage)/100);
      int re_invertedValue = (255 - scaledValue);
      return re_invertedValue;
    }
  }
}
