using System;
using System.Drawing;

namespace MDMUtils.Images
{
  public class BitmapScaler
  {
    private readonly int scaleFactor;
    private Bitmap originalBitmap;
    private Bitmap newBitmap;

    public BitmapScaler(int scaleFactor)
    {
      this.scaleFactor = scaleFactor;
    }


    public Bitmap ExactExpand(Bitmap original)
    {
      originalBitmap = original;
      ExactExpandInternal();
      return newBitmap;
    }

    public Bitmap ExactContract(Bitmap original)
    {
      originalBitmap = original;
      ExactContractInternal();
      return newBitmap;
    }

    private void ExactExpandInternal()
    {
      if (scaleFactor == 1) return;

      newBitmap = NewBlankBitmap(originalBitmap.Width*scaleFactor, originalBitmap.Height*scaleFactor);

      for (int x = 0; x < originalBitmap.Width; x++)
      {
        for (int y = 0; y < originalBitmap.Height; y++)
        {
          var originalColor = originalBitmap.GetPixel(x, y);
          if (originalColor.ToArgb() != Color.White.ToArgb())
          {
            SetPixelBlock(originalColor, x, y, scaleFactor);
          }
        }
      }
    }

    private void ExactContractInternal()
    {
      if (scaleFactor == 1) return;

      if (originalBitmap.Width%scaleFactor > 0 ||
          originalBitmap.Height%scaleFactor > 0)
      {
        throw new Exception("Source image will not contract exactly.");
      }

      newBitmap = NewBlankBitmap(originalBitmap.Width/scaleFactor, originalBitmap.Height/scaleFactor);

      for (int x = 0; x < newBitmap.Width; x++)
      {
        for (int y = 0; y < newBitmap.Height; y++)
        {
          Color originalColor = ReadPixelBlock(x, y, scaleFactor);
          if (originalColor.ToArgb() != Color.White.ToArgb())
          {
            newBitmap.SetPixel(x, y, originalColor);
          }
        }
      }
    }

    private Bitmap NewBlankBitmap(int x, int y)
    {
      var imageBase = new Bitmap(x, y);
      using (Graphics grp = Graphics.FromImage(imageBase))
      {
        grp.FillRectangle(Brushes.White, 0, 0, x, y);
      }
      return imageBase;
    }

    private Color ReadPixelBlock(int x, int y, int blockSize)
    {
      int originalX = x*blockSize;
      int originalY = y*blockSize;
      return originalBitmap.GetPixel(originalX, originalY);
    }

    private void SetPixelBlock(Color color, int x, int y, int blockSize)
    {
      int newX = x*blockSize;
      int newY = y*blockSize;
      for (int i = 0; i < blockSize; i++)
      {
        for (int j = 0; j < blockSize; j++)
        {
          int targetX = newX + i;
          int targetY = newY + j;
          newBitmap.SetPixel(targetX, targetY, color);
        }
      }
    }

    public Bitmap QuickScale(Bitmap original, double quickScaleFactor)
    {
      return new Bitmap(original, (int) (original.Width*quickScaleFactor), (int) (original.Height*quickScaleFactor));
    }
  }
}