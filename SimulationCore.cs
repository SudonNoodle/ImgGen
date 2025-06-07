using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace slime;

public class SimulationCore(WriteableBitmap output, PixelSize size, double frameRate = 60.0)
{
    private readonly CancellationTokenSource cts = new();
    private Task? task;
    private PixelBuffer current = new(size);
    private PixelBuffer previous = new(size);
    private readonly Stopwatch frameTimer = new();
    
    public void StartRenderLoop()
    {
        task = Task.Run(() => RenderLoopAsync(output));
    }

    public void StopRenderLoop()
    {
        cts.Cancel();
        task?.Wait();
    }

    private async Task RenderLoopAsync(WriteableBitmap target)
    {
        uint frameCounter = 0;
        while (!cts.IsCancellationRequested)
        {
            frameTimer.Restart();
            await Task.Run(() => GenerateFrame(frameCounter));
            await Dispatcher.UIThread.InvokeAsync(() => current.CopyTo(output), DispatcherPriority.Render);
            current.CopyTo(target);
            if (frameTimer.Elapsed.TotalMilliseconds <= 1000f / frameRate)
            { await Task.Delay((int)(1000 / frameRate - frameTimer.Elapsed.TotalMilliseconds)); }
            //(current,  previous) = (previous, current);
            frameCounter++;
            Debug.WriteLine(frameCounter.ToString());
        }
    }

    private void GenerateFrame(uint frameCounter)
    {
        var fHeight = size.Height;
        Parallel.For( 0, size.Height, (int y) =>
        {
            byte r = (byte)(y * 255 / size.Height);
            for (int x = 0; x < size.Width; x++) { current[x, y] = new Bgra((byte)(frameCounter), (byte)y, r, 255); }
        });
    }
}