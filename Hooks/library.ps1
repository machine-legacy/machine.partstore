# -----------------------------------------------------------------------------
#
#
# -----------------------------------------------------------------------------

function Git
{
  param([string]$directory = $(throw "Directory is required."),
        [string]$command = $(throw "Command is required."))

  $startInfo = New-Object Diagnostics.ProcessStartInfo
  $startInfo.Filename = "C:\Program Files (x86)\Git\Cmd\git.cmd"
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

function DoWeHaveOriginRemote
{
  $remote = Git $global:repository "remote"
  return $remote.Output -match "^origin$"
}

if ($ENV:HOME -eq "")
{
  $ENV:HOME = $ENV:USERPROFILE
}

$global:repository = $repository

# EOF
