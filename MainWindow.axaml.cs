using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Reactive;
using Vector = Avalonia.Vector;

namespace slime;

public partial class MainWindow : Window
{
    PixelSize size = new(1000, 1000);
    private SimulationCore SimCore;

    public MainWindow()
    {
        InitializeComponent();
        Opened += OnOpened;
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        var wb = new WriteableBitmap(
            size,
            new Vector(96,96),
            PixelFormat.Bgra8888,
            AlphaFormat.Premul);

        SimCore = new SimulationCore(wb, wb.PixelSize);
        Image.Width = Math.Clamp(size.Width, 0, ClientSize.Width);
        Image.Height = Math.Clamp(size.Height, 0, ClientSize.Height);
        Image.Source = wb;
        SimCore.StartRenderLoop();
        this.GetObservable(ClientSizeProperty).Subscribe( new AnonymousObserver<Size>(sz =>
        {
            Image.Width = Math.Clamp(size.Width, 0, sz.Width);
            Image.Height = Math.Clamp(size.Height, 0, sz.Height);
        }));
    }
    
}