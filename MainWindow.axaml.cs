using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Reactive;

namespace slime;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Opened += OnOpened;
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        var source = new PixelBuffer();
        source.TestPattern();
        var wb = new WriteableBitmap(
            source.PixelSize,
            new Vector(96,96),
            PixelFormat.Bgra8888,
            AlphaFormat.Premul);
        Image.Width = Math.Clamp(source.Size.Width, 0, ClientSize.Width);
        Image.Height = Math.Clamp(source.Size.Height, 0, ClientSize.Height);
        Image.Source = wb;
        source.TestPattern();
        source.CopyTo(wb);
        
        this.GetObservable(ClientSizeProperty).Subscribe( new AnonymousObserver<Size>(sz =>
        {
            Image.Width = Math.Clamp(source.Size.Width, 0, sz.Width);
            Image.Height = Math.Clamp(source.Size.Height, 0, sz.Height);
        }));
    }
    
}