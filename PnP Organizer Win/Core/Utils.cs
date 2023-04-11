using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Octokit;
using PnPOrganizer.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI;

namespace PnPOrganizer.Core
{
    public static partial class Utils
    {
        public const long GitHubRepoID = 563509297;

        public static int GetAttributeBonus(int attributeValue) => (int)(Math.Floor(attributeValue * 0.5) - 5);

        #region Image
        /// <summary>
        /// Converts a <paramref name="bitmapImage"/> into a byte Array
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <returns></returns>
        public static async Task<IAsyncOperationWithProgress<IBuffer, uint>> BitmapToBytesAsync(SoftwareBitmap? bitmap, string fileExtension = "")
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));
            if (string.IsNullOrWhiteSpace(fileExtension))
                throw new ArgumentNullException(nameof(fileExtension));

            using var stream = new InMemoryRandomAccessStream();
            var encoderId = fileExtension switch
            {
                "png" => BitmapEncoder.PngEncoderId,
                "bmp" => BitmapEncoder.BmpEncoderId,
                "jpeg" or "jpg" => BitmapEncoder.JpegEncoderId,
                _ => BitmapEncoder.GifEncoderId
            };
            var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetSoftwareBitmap(bitmap);
            await encoder.FlushAsync();

            var bytes = new byte[stream.Size];
            return stream.ReadAsync(bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
        }

        /// <summary>
        /// Converts a byte Array into a BitmapImage
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static async Task<SoftwareBitmap> BitmapImageFromBytesAsync(IBuffer buffer)
        {
            using var stream = buffer.AsStream().AsRandomAccessStream();
            var decoder = await BitmapDecoder.CreateAsync(stream);
            return decoder.GetSoftwareBitmapAsync().GetResults();
        }
        #endregion Image

        #region XML
        private static readonly XmlWriterSettings _xmlWriterSettings = new()
        {
            Indent = true
        };

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="stream"></param>
        public static void SerializeAndWriteToXml<T>(T obj, Stream stream)
        {
            XmlSerializer serializer = new(typeof(T));
            using var writer = XmlWriter.Create(stream, _xmlWriterSettings);
            serializer.Serialize(writer, obj);
            writer.Close();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T ReadAndDeserializeFromXml<T>(Stream stream)
        {
            XmlSerializer serializer = new(typeof(T));
            using var reader = XmlReader.Create(stream);
            var obj = (T)serializer.Deserialize(reader)!;
            return obj;
        }
        #endregion XML

        #region Color
        /// <summary>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int GetArgbColorValue(Color color)
        {
            var argb = BitConverter.ToInt32(new byte[] { color.A, color.B, color.G, color.R}, 0);
            return argb;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color ColorFromArgbValue(int value)
        {
            var a = (byte)(value >> 24 & 0xFF);
            var r = (byte)(value >> 16 & 0xFF);
            var g = (byte)(value >> 8 & 0xFF);
            var b = (byte)(value & 0xFF);
            var color = Color.FromArgb(a, r, g, b);
            return color;
        }
        #endregion Color

        #region Controls
        public static T? FindVisualParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            if (child is T t)
                return t;
            var parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;
            if (parentObject is T parent)
                return parent;
            return FindVisualParent<T>(parentObject);
        }

        #endregion Controls

        #region Updates and Version Check
        public static async Task<bool> CheckVersionAsync()
        {
            var github = new GitHubClient(new ProductHeaderValue("PnP-Organizer"));
            var latestRelease = (await github.Repository.Release.GetAll(GitHubRepoID))[0];
            var tagName = latestRelease.TagName;

            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var productVersion = fvi.ProductVersion;

            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

            bool updateAvailable;
            if (tagName.Contains("rc") && !string.IsNullOrWhiteSpace(productVersion))
            {
                var rcTagExclRegex = RcTagExcludeRegex();
                var productBaseVersion = new Version(rcTagExclRegex.Replace(productVersion, string.Empty));
                var tagBaseVersion = new Version(rcTagExclRegex.Replace(tagName, string.Empty));

                if (tagBaseVersion == productBaseVersion && productVersion.Contains("rc"))
                {
                    var versionExclRegex = VersionExcludeRegex();
                    var tagRCVersion = int.Parse(versionExclRegex.Replace(tagName, string.Empty));
                    var productRCVersion = int.Parse(versionExclRegex.Replace(productVersion, string.Empty));
                    updateAvailable = tagRCVersion > productRCVersion;
                }
                else
                    updateAvailable = tagBaseVersion > currentVersion;

                if (updateAvailable)
                    Log.Information("{tagVersion} > {productVersion}", tagName, productVersion);
                else
                    Log.Information("{tagVersion} <= {productVersion}", tagName, productVersion);
            }
            else
            {
                var latestVersion = new Version(tagName);
                updateAvailable = latestVersion > currentVersion;
                Log.Information("Checking Version: {currentVersion} (Current) || {latestVersion} (Latest)", latestVersion, currentVersion);
                if (updateAvailable)
                    Log.Information("{latestVersion} > {currentVersion}", latestVersion, currentVersion);
                else
                    Log.Information("{latestVersion} <= {currentVersion}", latestVersion, currentVersion);
            }

            return updateAvailable;
        }

        public static async Task<Release> GetLatestRelease()
        {
            var github = new GitHubClient(new ProductHeaderValue("PnP-Organizer"));
            var latestRelease = (await github.Repository.Release.GetAll(GitHubRepoID))[0];
            return latestRelease;
        }
        #endregion

        #region Save/Load
        public static async Task<string> ExportDocument(RichEditBox rtBox)
        {
            FileSavePicker savePicker = new();

            var hWnd = WindowHelper.GetCurrentProcMainWindowHandle();
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            var file = await savePicker.PickSaveFileAsync();
            if(file != null)
            {
                CachedFileManager.DeferUpdates(file);

                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    rtBox.Document.SaveToStream(Microsoft.UI.Text.TextGetOptions.FormatRtf, stream);
                }
                var status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != FileUpdateStatus.Complete)
                {
                    var errorBox =
                        new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
            }
            return file?.Name ?? string.Empty;
        }

        [GeneratedRegex("-rc.*")]
        private static partial Regex RcTagExcludeRegex();
        [GeneratedRegex(".*-rc")]
        private static partial Regex VersionExcludeRegex();
        #endregion
    }
}
