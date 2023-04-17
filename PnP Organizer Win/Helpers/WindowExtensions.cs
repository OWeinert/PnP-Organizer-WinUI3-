using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.Graphics;
using WinRT.Interop;

namespace PnPOrganizer.Helpers
{
    public static class WindowExtensions
    {
        public static AppWindow GetAppWindow(this Window window)
        {
            var windowHandle = WindowNative.GetWindowHandle(window);
            var windowId = Win32Interop.GetWindowIdFromWindow(windowHandle);
            return AppWindow.GetFromWindowId(windowId);
        }

        public static (int Width, int Height) GetCurrentWindowSize(this Window window)
        {
            var size = window.GetAppWindow().Size;
            return (size.Width, size.Height);
        }

        public static void SetWindowSize(this Window window, int width, int height)
        {
            var appWindow = window.GetAppWindow();
            SizeInt32 size = new(width, height);
            appWindow.Resize(size);
        }

        public static void SetIsAlwaysOnTop(this Window window, bool value)
        {
            var appWindow = window.GetAppWindow();

            if (appWindow.Presenter is OverlappedPresenter overlappedPresenter)
            {
                overlappedPresenter.IsAlwaysOnTop = value;
                return;
            }

            throw new NotSupportedException($"Always on top is not supported with {appWindow.Presenter.Kind}.");
        }

        public static bool GetIsAlwaysOnTop(this Window window)
        {
            var appWindow = window.GetAppWindow();

            if (appWindow.Presenter is OverlappedPresenter overlappedPresenter)
            {
                return overlappedPresenter.IsAlwaysOnTop;
            }

            throw new NotSupportedException($"Always on top is not supported with {appWindow.Presenter.Kind}.");
        }
    }
}