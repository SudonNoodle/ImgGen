using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Size = Avalonia.Size;

namespace slime;

public class PixelBuffer(PixelSize size) : IDisposable
{
    private readonly int height = size.Height;
    private readonly int width = size.Width;
    private readonly byte[] buffer = ArrayPool<byte>.Shared.Rent(size.Height * size.Width * 4);
    public Span<byte> Image => buffer.AsSpan();
    public void Dispose()
    {
        ArrayPool<byte>.Shared.Return(buffer);
    }

    private Span<Bgra> Pixels => MemoryMarshal.Cast<byte, Bgra>(buffer);
    public Bgra this[int x, int y]
    {
        get => Pixels[y * width + x];
        set => Pixels[y * width + x] = value;
    }

    public void TestPattern()
    {
        var fHeight = (float)height;
        Parallel.For( 0, height, (int y) =>
        {
            byte r = (byte)(y * 255 / height);
            for (int x = 0; x < width; x++) { this[x, y] = new Bgra((byte)x, (byte)y, r, 255); }
        });
    }

    public void CopyTo(WriteableBitmap wb)
    {
        using var fb = wb.Lock();
        for (int y = 0; y < height; y++)
        {
            Marshal.Copy(buffer, y * width * 4, fb.Address + y * fb.RowBytes, width * 4);
        }
    }
}
