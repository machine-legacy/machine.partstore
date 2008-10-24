# -----------------------------------------------------------------------------
#
#
# -----------------------------------------------------------------------------
param([string]$repository = $(throw "Repository is required."),
      [string]$project = $(throw "Project is required."),
      [string]$version = $(throw "Version is required."),
      [string]$alias = $(throw "Alias is required."))

. "$repository\Hooks\library.ps1"

Git $repository "add ." | OutputOnly
Git $repository "commit -v -m ""Adding new version of $project ($version) as $alias.""" | OutputOnly

# EOF
