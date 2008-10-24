# -----------------------------------------------------------------------------
#
#
# -----------------------------------------------------------------------------
param([string]$repository = $(throw "Repository is required."))

. "$repository\Hooks\library.ps1"

Write-Host "Refreshing..."

if (DoWeHaveOriginRemote)
{
  Git $repository "pull" | OutputOnly
}
else
{
  "Git repository has no origin remote."
}

# EOF
