using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnPOrganizer.Core
{
    using Microsoft.UI;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml;
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using WinRT;
    using WinRT.Interop;

    public static class WindowHelper
    {
        public static IntPtr GetCurrentProcMainWindowHandle()
        {
            using var process = Process.GetCurrentProcess();
            return process.MainWindowHandle;
        }

        public static IntPtr GetHandle(this Window window)
        {
            WindowWrapper? windowWrapper = WindowWrapper.FromAbi(window.GetHandle());
            if(windowWrapper != null)
                return windowWrapper.WindowHandle;
            return IntPtr.Zero;
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        private interface IWindowNative
        {
            IntPtr WindowHandle { get; }
        }

        [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
        private class WindowWrapper : IWindowNative
        {
            [Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
            public struct Vftbl
            {
                public delegate int _get_WindowHandle_0(IntPtr thisPtr, out IntPtr windowHandle);
                internal global::WinRT.Interop.IUnknownVftbl IUnknownVftbl;
                public _get_WindowHandle_0 get_WindowHandle_0;
                public static readonly Vftbl AbiToProjectionVftable;
                public static readonly IntPtr AbiToProjectionVftablePtr;

                static Vftbl()
                {
                    AbiToProjectionVftable = new Vftbl
                    {
                        IUnknownVftbl = global::WinRT.Interop.IUnknownVftbl.AbiToProjectionVftbl,
                        get_WindowHandle_0 = Do_Abi_get_WindowHandle_0
                    };
                    AbiToProjectionVftablePtr = Marshal.AllocHGlobal(Marshal.SizeOf<Vftbl>());
                    Marshal.StructureToPtr(AbiToProjectionVftable, AbiToProjectionVftablePtr, false);
                }

                private static int Do_Abi_get_WindowHandle_0(IntPtr thisPtr, out IntPtr windowHandle)
                {
                    windowHandle = default;
                    try
                    {
                        windowHandle = ComWrappersSupport.FindObject<IWindowNative>(thisPtr).WindowHandle;
                    }
                    catch (Exception ex)
                    {
                        return Marshal.GetHRForException(ex);
                    }
                    return 0;
                }
            }
            internal static ObjectReference<Vftbl> FromAbi(IntPtr thisPtr)
            {
                return ObjectReference<Vftbl>.FromAbi(thisPtr);
            }

            public static implicit operator WindowWrapper?(IObjectReference obj)
            {
                return (obj != null) ? new WindowWrapper(obj) : null;
            }

            protected readonly ObjectReference<Vftbl> _obj;
            public IObjectReference ObjRef => _obj;
            public IntPtr ThisPtr => _obj.ThisPtr;
            public ObjectReference<I> AsInterface<I>()
            {
                return _obj.As<I>();
            }

            public A As<A>()
            {
                return _obj.AsType<A>();
            }

            public WindowWrapper(IObjectReference obj) : this(obj.As<Vftbl>()) { }
            internal WindowWrapper(ObjectReference<Vftbl> obj)
            {
                _obj = obj;
            }

            public IntPtr WindowHandle
            {
                get
                {
                    Marshal.ThrowExceptionForHR(_obj.Vftbl.get_WindowHandle_0(ThisPtr, out var windowHandle));

                    return windowHandle;
                }
            }
        }

        static public Window CreateWindow()
        {
            var newWindow = new Window();
            TrackWindow(newWindow);
            return newWindow;
        }

        static public void TrackWindow(Window window)
        {
            window.Closed += (sender, args) => {
                _activeWindows.Remove(window);
            };
            _activeWindows.Add(window);
        }

        static public AppWindow GetAppWindow(Window window)
        {
            var hWnd = WindowNative.GetWindowHandle(window);
            var wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        static public Window? GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot != null)
            {
                foreach (Window window in _activeWindows)
                {
                    if (element.XamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }

        static public List<Window> ActiveWindows { get { return _activeWindows; } }

        static private List<Window> _activeWindows = new List<Window>();
    }
}
