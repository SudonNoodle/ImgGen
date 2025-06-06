using System;
using System.Runtime.InteropServices;

namespace slime;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Bgra
{
    public readonly byte B; public readonly byte G;
    public readonly byte R; public readonly byte A;

    public Bgra(byte b, byte g, byte r, byte a)
    {
        this.B = b; this.G = g;
        this.R = r; this.A = a;
    }

    public Bgra(Bgra bgra)
    {
        B = bgra.B; G = bgra.G;
        R = bgra.R; A = bgra.A;
    }

    public Bgra(byte[] colors)
    {
        if (colors.Length != 4) {throw new 
            ArgumentException("colors must be 4 bytes"); }
        B = colors[0]; G = colors[1];
        R = colors[2]; A = colors[3];
    }

    public Bgra(ReadOnlySpan<byte> colors)
    {
        if (colors.Length != 4) {throw new
            ArgumentException("colors must be 4 bytes"); }
        B = colors[0]; G = colors[1];
        R = colors[2]; A = colors[3];
    }
    
    public uint Packed =>(uint)(B | (G << 8) | (R << 16) | (A << 24));
    public bool Equals(Bgra other) => Packed == other.Packed;
    public override bool Equals(object? obj) => obj is Bgra && Equals((Bgra)obj);
    public override int GetHashCode() => Packed.GetHashCode();
    public static bool operator ==(Bgra left, Bgra right) => left.Equals(right);
    public static bool operator !=(Bgra left, Bgra right) => !left.Equals(right);
}