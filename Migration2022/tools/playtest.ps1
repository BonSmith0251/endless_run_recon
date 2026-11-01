param(
    [Parameter(Mandatory)][ValidateSet('capture','click','key','keys')][string]$Action,
    [Parameter(Mandatory)][int]$OwnedProcessId,
    [int]$X, [int]$Y,                 # click: client coordinates
    [string]$Key, [int]$HoldMs = 120,  # key: one key (W, A, S, D, SPACE, ESCAPE, RETURN)
    [string]$Sequence,                 # keys: "W:800,SPACE:100,D:600" (key:holdMs, run in order)
    [string]$Output                    # capture: png path
)
# Input and capture for the owned play-test player only. Input is sent only after verifying
# the owned game window is in the foreground; otherwise nothing is sent.
Add-Type -AssemblyName System.Drawing
Add-Type @'
using System;
using System.Runtime.InteropServices;
public static class PlayTest {
    [StructLayout(LayoutKind.Sequential)] public struct Rect { public int Left, Top, Right, Bottom; }
    [StructLayout(LayoutKind.Sequential)] public struct Point { public int X, Y; }
    [DllImport("user32.dll")] public static extern bool GetClientRect(IntPtr h, out Rect r);
    [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr h, out Rect r);
    [DllImport("user32.dll")] public static extern bool ClientToScreen(IntPtr h, ref Point p);
    [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
    [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] public static extern bool SetCursorPos(int x, int y);
    [DllImport("user32.dll")] public static extern void mouse_event(uint f, uint dx, uint dy, uint d, UIntPtr e);
    [DllImport("user32.dll")] public static extern void keybd_event(byte vk, byte scan, uint f, UIntPtr e);
    [DllImport("user32.dll")] public static extern uint MapVirtualKey(uint code, uint type);
    [DllImport("user32.dll")] public static extern bool PrintWindow(IntPtr h, IntPtr dc, uint f);
}
'@
$game = Get-Process -Id $OwnedProcessId -ErrorAction Stop
$window = $game.MainWindowHandle
if ($window -eq [IntPtr]::Zero) { throw 'Owned game window unavailable' }

function Focus {
    [void][PlayTest]::SetForegroundWindow($window)
    Start-Sleep -Milliseconds 150
    if ([PlayTest]::GetForegroundWindow() -ne $window) { throw 'Owned game is not foreground; no input sent' }
}
$vk = @{ W = 0x57; A = 0x41; S = 0x53; D = 0x44; SPACE = 0x20; ESCAPE = 0x1B; RETURN = 0x0D }
function Press([string]$name, [int]$ms) {
    $code = $vk[$name.ToUpper()]; if (-not $code) { throw "Unsupported key $name" }
    $scan = [byte][PlayTest]::MapVirtualKey($code, 0)
    [PlayTest]::keybd_event([byte]$code, $scan, 0, [UIntPtr]::Zero)
    Start-Sleep -Milliseconds $ms
    [PlayTest]::keybd_event([byte]$code, $scan, 2, [UIntPtr]::Zero)
}

switch ($Action) {
    'capture' {
        $r = New-Object PlayTest+Rect; [void][PlayTest]::GetWindowRect($window, [ref]$r)
        $bmp = New-Object Drawing.Bitmap (($r.Right - $r.Left), ($r.Bottom - $r.Top))
        $g = [Drawing.Graphics]::FromImage($bmp); $dc = $g.GetHdc()
        try { [void][PlayTest]::PrintWindow($window, $dc, 2) } finally { $g.ReleaseHdc($dc) }
        try { $bmp.Save($Output, [Drawing.Imaging.ImageFormat]::Png) } finally { $g.Dispose(); $bmp.Dispose() }
        "captured $Output"
    }
    'click' {
        $c = New-Object PlayTest+Rect; [void][PlayTest]::GetClientRect($window, [ref]$c)
        if ($X -lt 0 -or $Y -lt 0 -or $X -ge $c.Right -or $Y -ge $c.Bottom) { throw 'Point outside the owned game client' }
        Focus
        $p = New-Object PlayTest+Point; $p.X = $X; $p.Y = $Y
        [void][PlayTest]::ClientToScreen($window, [ref]$p)
        [void][PlayTest]::SetCursorPos($p.X, $p.Y); Start-Sleep -Milliseconds 300  # let UI register hover first
        [PlayTest]::mouse_event(2, 0, 0, 0, [UIntPtr]::Zero); Start-Sleep -Milliseconds 60
        [PlayTest]::mouse_event(4, 0, 0, 0, [UIntPtr]::Zero)
        "clicked $X,$Y"
    }
    'key' { Focus; Press $Key $HoldMs; "pressed $Key" }
    'keys' {
        Focus
        foreach ($step in $Sequence.Split(',')) { $k, $ms = $step.Split(':'); Press $k ([int]$ms) }
        "played $Sequence"
    }
}
