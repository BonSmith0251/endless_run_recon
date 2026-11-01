param([string]$Method='ReconstructionTools.Smoke',[string]$Run='smoke-01',[switch]$NoGraphics)
$editorExe='C:/Program Files/Unity 2022.3.62f3/Editor/Unity.exe'
$projectDirectory='D:/games/Endless Rails 1-0/game/scene-reconstruction/EndlessRails-2022'
$reviewDirectory='D:/games/Endless Rails 1-0/game/scene-reconstruction/migration-2022/evidence'
$unityArguments=@('-batchmode','-projectPath',('"'+$projectDirectory+'"'),'-executeMethod',$Method,'--burst-disable-compilation','-disable-assembly-updater','-force-d3d11','-logFile',('"'+$reviewDirectory+'/'+$Run+'.log"'))
if($NoGraphics) { $unityArguments+='-nographics' }
$record=[ordered]@{startedUtc=[DateTime]::UtcNow.ToString('O');method=$Method;project=$projectDirectory;arguments=$unityArguments;pid=$null;finishedUtc=$null;exitCode=$null}
$childEditor=Start-Process -FilePath $editorExe -ArgumentList $unityArguments -PassThru -WindowStyle Hidden
$record.pid=$childEditor.Id
$record | ConvertTo-Json -Depth 5 | Set-Content ($reviewDirectory+'/'+$Run+'-process.json')
$childEditor.WaitForExit()
$record.finishedUtc=[DateTime]::UtcNow.ToString('O');$record.exitCode=$childEditor.ExitCode
$record | ConvertTo-Json -Depth 5 | Set-Content ($reviewDirectory+'/'+$Run+'-process.json')
Write-Output ($record | ConvertTo-Json -Depth 5 -Compress)
$scanScript='D:/games/Endless Rails 1-0/game/scene-reconstruction/coordinator/scene-review/check_completed_log.py'
& python $scanScript --log ($reviewDirectory+'/'+$Run+'.log') --process ($reviewDirectory+'/'+$Run+'-process.json') --output ($reviewDirectory+'/'+$Run+'-completed-log.json')
if($childEditor.ExitCode -ne 0) { exit $childEditor.ExitCode }
exit $LASTEXITCODE
