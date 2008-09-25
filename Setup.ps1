#
# Setup for Dev....
#

$cwd = Get-Location -PSProvider FileSystem
$ENV:PATH += (";" + $cwd + "\Build\Debug")

# EOF