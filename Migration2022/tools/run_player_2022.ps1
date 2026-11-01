param([string]$Run='player-run-01',[int]$Seconds=45)
# Launch the owned 2022 player windowed, capture its own window, close it through the normal
# window-close path and scan the completed log. No Steam licensing behavior is bypassed.
$evidence='D:/games/Endless Rails 1-0/game/scene-reconstruction/migration-2022/evidence'
$player='D:/games/Endless Rails 1-0/game/scene-reconstruction/migration-2022/player/EndlessRails-2022.exe'
$capture='D:/games/Endless Rails 1-0/game/scene-reconstruction/coordinator/scene-review/capture_owned_player.ps1'
$arguments=@('-force-d3d11','-screen-fullscreen','0','-screen-width','1280','-screen-height','720','-logFile',('"'+$evidence+'/'+$Run+'.log"'))
$record=[ordered]@{startedUtc=[DateTime]::UtcNow.ToString('O');executable=$player;arguments=$arguments;pid=$null;capture=$null;closeRequested=$null;finishedUtc=$null;exitCode=$null;networkSamples=0;tcp=@();udp=@()}
$process=Start-Process -FilePath $player -WorkingDirectory (Split-Path $player -Parent) -ArgumentList $arguments -PassThru -WindowStyle Normal
$record.pid=$process.Id
# Sample the player's own sockets once per second for the whole run.
$tcp=@{}; $udp=@{}
for($second=0; $second -lt $Seconds -and -not $process.HasExited; $second++) {
    Get-NetTCPConnection -OwningProcess $process.Id -ErrorAction SilentlyContinue | ForEach-Object { $tcp[('{0}:{1} -> {2}:{3} {4}' -f $_.LocalAddress,$_.LocalPort,$_.RemoteAddress,$_.RemotePort,$_.State)]=1 }
    Get-NetUDPEndpoint -OwningProcess $process.Id -ErrorAction SilentlyContinue | ForEach-Object { $udp[('{0}:{1}' -f $_.LocalAddress,$_.LocalPort)]=1 }
    $record.networkSamples++
    Start-Sleep -Seconds 1
    $process.Refresh()
}
$record.tcp=@($tcp.Keys); $record.udp=@($udp.Keys)
if(-not $process.HasExited) {
    $record.capture=(& powershell -NoProfile -ExecutionPolicy Bypass -File $capture -OwnedProcessId $process.Id -Output ($evidence+'/'+$Run+'.png')) -join ''
    $record.closeRequested=$process.CloseMainWindow()
    if(-not $process.WaitForExit(30000)) { $process.Kill(); $record.closeRequested='killed after 30 s' }
}
$process.WaitForExit()
$record.finishedUtc=[DateTime]::UtcNow.ToString('O'); $record.exitCode=$process.ExitCode
$record | ConvertTo-Json -Depth 5 | Set-Content ($evidence+'/'+$Run+'-process.json')
Write-Output ($record | ConvertTo-Json -Depth 5 -Compress)
& python 'D:/games/Endless Rails 1-0/game/scene-reconstruction/coordinator/scene-review/check_completed_log.py' --log ($evidence+'/'+$Run+'.log') --process ($evidence+'/'+$Run+'-process.json') --output ($evidence+'/'+$Run+'-completed-log.json')
