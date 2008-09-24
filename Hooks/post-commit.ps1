# -----------------------------------------------------------------------------
#
#
# -----------------------------------------------------------------------------
param([string]$repository = $(throw "Repository is required."),
      [string]$project = $(throw "Project is required."),
      [string]$version = $(throw "Version is required."),
      [string]$alias = $(throw "Alias is required."))

function Git
{
  param([string]$directory = $(throw "Directory is required."),
        [string]$command = $(throw "Command is required."))

  $startInfo = New-Object Diagnostics.ProcessStartInfo
  $startInfo.Filename = "C:\Program Files (x86)\Git\Bin\git.exe"
  $startInfo.Arguments = $command
  $startInfo.WorkingDirectory = $directory
  $startInfo.UseShellExecute = $false
  $startInfo.CreateNoWindow = $true 
  $startInfo.WindowStyle = [System.Diagnostics.ProcessWindowStyle]::Hidden
  $startInfo.RedirectStandardOutput = $true 
  $startInfo.RedirectStandardError = $true 
  $process = [Diagnostics.Process]::Start($startInfo)
  $standardOutput = $process.StandardOutput.ReadToEnd()
  $standardError = $process.StandardError.ReadToEnd()
  $process.WaitForExit()
  $res = @{ Output = $standardOutput; Error = $standardError; ExitCode = $process.ExitCode }
  return $res
}

function OutputOnly
{
  PROCESS {
    if ($_.Output.Trim() -eq "")
    {
      $_.Error.Trim()
    }
    elseif ($_.Error.Trim() -eq "")
    {
      $_.Output.Trim()
    }
    else
    {
      $_.Output + $_.Error.Trim()
    }
  }
}

function PushIfWeHaveRemote
{
  $remote = Git $global:repository "remote"
  if ($remote.Output -match "^origin$")
  {
    $res = Git $global:repository "push" | OutputOnly | Out-Host
  }
}

if ($ENV:HOME -eq "")
{
  $ENV:HOME = $ENV:USERPROFILE
}

$global:repository = $repository

Git $repository "add ." | OutputOnly
Git $repository "commit -v -m ""Adding new version of $project ($version) as $alias.""" | OutputOnly
PushIfWeHaveRemote

# EOF
