using System;

namespace BskMcLink;

public enum DirectLinkLogLevel
{
    Info,
    Warn
}

/// <summary>
/// 轻量日志回调入口：默认不绑定任何外部日志框架，调用方按需订阅 <see cref="OnLog"/>。
/// </summary>
public static class DirectLinkLog
{
    public static event Action<DirectLinkLogLevel, string>? OnLog;

    internal static void Info(string message) => OnLog?.Invoke(DirectLinkLogLevel.Info, message);

    internal static void Warn(string message) => OnLog?.Invoke(DirectLinkLogLevel.Warn, message);
}
