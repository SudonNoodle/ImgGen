using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using Avalonia;
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
        Bitmap source = new Bitmap("/home/bo/Projects/slime/ffxiv_05192025_151441_926.png");
        Image.Width = Math.Clamp(source.Size.Width, 0, ClientSize.Width);
        Image.Height = Math.Clamp(source.Size.Height, 0, ClientSize.Height);
        Image.Source = source;

        this.GetObservable(ClientSizeProperty).Subscribe( new AnonymousObserver<Size>(sz =>
        {
            Image.Width = Math.Clamp(source.Size.Width, 0, sz.Width);
            Image.Height = Math.Clamp(source.Size.Height, 0, sz.Height);
        }));
    }
    
}