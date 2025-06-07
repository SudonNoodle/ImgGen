using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Size = Avalonia.Size;

namespace slime;

public class PixelBuffer(int height = 1000, int width = 1000) : IDisposable
{
    private readonly byte[] buffer = ArrayPool<byte>.Shared.Rent(height * width * 4);
    int Height { get; } = height;
    int Width { get; } = width;
    
    public PixelSize PixelSize => new PixelSize(Width, Height);
    public Size Size => new Size(Width, Height);
    
    public Span<byte> Image => buffer.AsSpan();
    
    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(buffer);
    }

    private Span<Bgra> Pixels => MemoryMarshal.Cast<byte, Bgra>(buffer);

    public Bgra this[int x, int y]
    {
        get => Pixels[y * Width + x];
        set => Pixels[y * Width + x] = value;
    }

    public void TestPattern()
    {
        var fHeight = (float)Height;
        Parallel.For( 0, Height, (int y) =>
        {
            byte r = (byte)(y * 255 / Height);
            for (int x = 0; x < Width; x++) { this[x, y] = new Bgra((byte)x, (byte)y, r, 255); }
        });
    }

    public void CopyTo(WriteableBitmap wb)
    {
        using var fb = wb.Lock();
        for (int y = 0; y < Height; y++)
        {
            Marshal.Copy(buffer, y * Width * 4, fb.Address + y * fb.RowBytes, Width * 4);
        }
    }
}
