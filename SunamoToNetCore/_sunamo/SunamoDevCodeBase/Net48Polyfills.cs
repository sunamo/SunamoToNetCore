// Compatibility helpers so the project compiles for net48/netstandard2.0 in addition to net8+.
// FileAsync is active for all target frameworks (forwards to File.*Async on modern ones).
// The extension polyfills below only compile for net48/netstandard2.0.
// Type polyfills (Index/Range, init, required, nullable attributes, ...) come from PolySharp.

internal static class FileAsync
{
#if NET5_0_OR_GREATER
    internal static System.Threading.Tasks.Task<string> ReadAllTextAsync(string path, System.Threading.CancellationToken cancellationToken = default)
        => System.IO.File.ReadAllTextAsync(path, cancellationToken);

    internal static System.Threading.Tasks.Task WriteAllTextAsync(string path, string? contents, System.Threading.CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllTextAsync(path, contents, cancellationToken);

    internal static System.Threading.Tasks.Task<string[]> ReadAllLinesAsync(string path, System.Threading.CancellationToken cancellationToken = default)
        => System.IO.File.ReadAllLinesAsync(path, cancellationToken);

    internal static System.Threading.Tasks.Task WriteAllLinesAsync(string path, System.Collections.Generic.IEnumerable<string> lines, System.Threading.CancellationToken cancellationToken = default)
        => System.IO.File.WriteAllLinesAsync(path, lines, cancellationToken);
#else
    /// <summary>
    /// Read all text async.
    /// </summary>
    internal static async System.Threading.Tasks.Task<string> ReadAllTextAsync(string path, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var reader = new System.IO.StreamReader(path);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Read all text async.
    /// </summary>
    internal static async System.Threading.Tasks.Task<string> ReadAllTextAsync(string path, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var reader = new System.IO.StreamReader(path, encoding);
        return await reader.ReadToEndAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Write all text async.
    /// </summary>
    internal static async System.Threading.Tasks.Task WriteAllTextAsync(string path, string? contents, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var writer = new System.IO.StreamWriter(path, false);
        await writer.WriteAsync(contents ?? string.Empty).ConfigureAwait(false);
    }

    /// <summary>
    /// Write all text async.
    /// </summary>
    internal static async System.Threading.Tasks.Task WriteAllTextAsync(string path, string? contents, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var writer = new System.IO.StreamWriter(path, false, encoding);
        await writer.WriteAsync(contents ?? string.Empty).ConfigureAwait(false);
    }

    /// <summary>
    /// Read all lines async.
    /// </summary>
    internal static async System.Threading.Tasks.Task<string[]> ReadAllLinesAsync(string path, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var lines = new System.Collections.Generic.List<string>();
        using var reader = new System.IO.StreamReader(path);
        string? line;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            lines.Add(line);
        }
        return lines.ToArray();
    }

    /// <summary>
    /// Read all lines async.
    /// </summary>
    internal static async System.Threading.Tasks.Task<string[]> ReadAllLinesAsync(string path, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var lines = new System.Collections.Generic.List<string>();
        using var reader = new System.IO.StreamReader(path, encoding);
        string? line;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            lines.Add(line);
        }
        return lines.ToArray();
    }

    /// <summary>
    /// Write all lines async.
    /// </summary>
    internal static async System.Threading.Tasks.Task WriteAllLinesAsync(string path, System.Collections.Generic.IEnumerable<string> lines, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var writer = new System.IO.StreamWriter(path, false);
        foreach (var line in lines)
        {
            await writer.WriteLineAsync(line).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Write all lines async.
    /// </summary>
    internal static async System.Threading.Tasks.Task WriteAllLinesAsync(string path, System.Collections.Generic.IEnumerable<string> lines, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var writer = new System.IO.StreamWriter(path, false, encoding);
        foreach (var line in lines)
        {
            await writer.WriteLineAsync(line).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Read all bytes async.
    /// </summary>
    internal static async System.Threading.Tasks.Task<byte[]> ReadAllBytesAsync(string path, System.Threading.CancellationToken cancellationToken = default)
    {
        using var stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read, 4096, useAsync: true);
        var bytes = new byte[stream.Length];
        int totalRead = 0;
        while (totalRead < bytes.Length)
        {
            int bytesRead = await stream.ReadAsync(bytes, totalRead, bytes.Length - totalRead, cancellationToken).ConfigureAwait(false);
            if (bytesRead == 0)
            {
                throw new System.IO.EndOfStreamException();
            }
            totalRead += bytesRead;
        }
        return bytes;
    }

    /// <summary>
    /// Write all bytes async.
    /// </summary>
    internal static async System.Threading.Tasks.Task WriteAllBytesAsync(string path, byte[] bytes, System.Threading.CancellationToken cancellationToken = default)
    {
        using var stream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None, 4096, useAsync: true);
        await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Append all text async.
    /// </summary>
    internal static async System.Threading.Tasks.Task AppendAllTextAsync(string path, string? contents, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var writer = new System.IO.StreamWriter(path, true);
        await writer.WriteAsync(contents ?? string.Empty).ConfigureAwait(false);
    }

    /// <summary>
    /// Append all text async.
    /// </summary>
    internal static async System.Threading.Tasks.Task AppendAllTextAsync(string path, string? contents, System.Text.Encoding encoding, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var writer = new System.IO.StreamWriter(path, true, encoding);
        await writer.WriteAsync(contents ?? string.Empty).ConfigureAwait(false);
    }

    /// <summary>
    /// Append all lines async.
    /// </summary>
    internal static async System.Threading.Tasks.Task AppendAllLinesAsync(string path, System.Collections.Generic.IEnumerable<string> lines, System.Threading.CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        using var writer = new System.IO.StreamWriter(path, true);
        foreach (var line in lines)
        {
            await writer.WriteLineAsync(line).ConfigureAwait(false);
        }
    }
#endif
}

#if !NET5_0_OR_GREATER

internal static class Net48ProcessPolyfillExtensions
{
    /// <summary>
    /// Wait for exit async.
    /// </summary>
    internal static System.Threading.Tasks.Task WaitForExitAsync(this System.Diagnostics.Process process, System.Threading.CancellationToken cancellationToken = default)
    {
        var completionSource = new System.Threading.Tasks.TaskCompletionSource<object?>(System.Threading.Tasks.TaskCreationOptions.RunContinuationsAsynchronously);
        process.EnableRaisingEvents = true;
        process.Exited += (sender, eventArgs) => completionSource.TrySetResult(null);
        if (process.HasExited)
        {
            completionSource.TrySetResult(null);
        }
        if (cancellationToken.CanBeCanceled)
        {
            cancellationToken.Register(() => completionSource.TrySetCanceled(cancellationToken));
        }
        return completionSource.Task;
    }
}

internal static class Net48StreamPolyfillExtensions
{
    /// <summary>
    /// Read exactly.
    /// </summary>
    internal static void ReadExactly(this System.IO.Stream stream, byte[] buffer, int offset, int count)
    {
        int totalRead = 0;
        while (totalRead < count)
        {
            int bytesRead = stream.Read(buffer, offset + totalRead, count - totalRead);
            if (bytesRead == 0)
            {
                throw new System.IO.EndOfStreamException();
            }
            totalRead += bytesRead;
        }
    }

    /// <summary>
    /// Read exactly.
    /// </summary>
    internal static void ReadExactly(this System.IO.Stream stream, byte[] buffer)
    {
        stream.ReadExactly(buffer, 0, buffer.Length);
    }
}

internal static class Net48DbConnectionPolyfillExtensions
{
    /// <summary>
    /// Close async.
    /// </summary>
    internal static System.Threading.Tasks.Task CloseAsync(this System.Data.Common.DbConnection connection)
    {
        connection.Close();
        return System.Threading.Tasks.Task.CompletedTask;
    }
}

namespace System.Collections.Generic
{
    internal static class Net48CollectionPolyfillExtensions
    {
        /// <summary>
        /// Try add.
        /// </summary>
        internal static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }
            dictionary.Add(key, value);
            return true;
        }
    }
}

namespace System
{
    internal static class Net48StringPolyfillExtensions
    {
        /// <summary>
        /// Split.
        /// </summary>
        internal static string[] Split(this string text, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return text.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Split.
        /// </summary>
        internal static string[] Split(this string text, char separator, StringSplitOptions options)
        {
            return text.Split(new[] { separator }, options);
        }

        /// <summary>
        /// Replace.
        /// </summary>
        internal static string Replace(this string text, string oldValue, string? newValue, StringComparison comparisonType)
        {
            if (oldValue == null)
            {
                throw new ArgumentNullException(nameof(oldValue));
            }
            if (oldValue.Length == 0)
            {
                throw new ArgumentException("String cannot be of zero length.", nameof(oldValue));
            }
            newValue ??= string.Empty;

            var result = new Text.StringBuilder(text.Length);
            int previousIndex = 0;
            int foundIndex = text.IndexOf(oldValue, comparisonType);
            while (foundIndex >= 0)
            {
                result.Append(text, previousIndex, foundIndex - previousIndex);
                result.Append(newValue);
                previousIndex = foundIndex + oldValue.Length;
                foundIndex = text.IndexOf(oldValue, previousIndex, comparisonType);
            }
            result.Append(text, previousIndex, text.Length - previousIndex);
            return result.ToString();
        }

        /// <summary>
        /// Starts with.
        /// </summary>
        internal static bool StartsWith(this string text, char value)
        {
            return text.Length != 0 && text[0] == value;
        }

        /// <summary>
        /// Ends with.
        /// </summary>
        internal static bool EndsWith(this string text, char value)
        {
            return text.Length != 0 && text[text.Length - 1] == value;
        }

        /// <summary>
        /// Contains.
        /// </summary>
        internal static bool Contains(this string text, char value)
        {
            return text.IndexOf(value) >= 0;
        }

        /// <summary>
        /// Contains.
        /// </summary>
        internal static bool Contains(this string text, string value, StringComparison comparisonType)
        {
            return text.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// Replace line endings.
        /// </summary>
        internal static string ReplaceLineEndings(this string text)
        {
            return text.ReplaceLineEndings(Environment.NewLine);
        }

        /// <summary>
        /// Replace line endings.
        /// </summary>
        internal static string ReplaceLineEndings(this string text, string replacementText)
        {
            string normalized = text.Replace("\r\n", "\n").Replace("\r", "\n");
            return replacementText == "\n" ? normalized : normalized.Replace("\n", replacementText);
        }
    }
}

namespace System.Runtime.Versioning
{
    // net48/netstandard2.0 do not have these attributes; PolySharp does not generate them
    [AttributeUsage(
        AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor |
        AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field |
        AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Module |
        AttributeTargets.Property | AttributeTargets.Struct,
        AllowMultiple = true, Inherited = false)]
    internal sealed class SupportedOSPlatformAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of SupportedOSPlatformAttribute.
        /// </summary>
        public SupportedOSPlatformAttribute(string platformName) { PlatformName = platformName; }
        public string PlatformName { get; }
    }

    [AttributeUsage(
        AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor |
        AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field |
        AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Module |
        AttributeTargets.Property | AttributeTargets.Struct,
        AllowMultiple = true, Inherited = false)]
    internal sealed class UnsupportedOSPlatformAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of UnsupportedOSPlatformAttribute.
        /// </summary>
        public UnsupportedOSPlatformAttribute(string platformName) { PlatformName = platformName; }
        public string PlatformName { get; }
    }
}

#endif
