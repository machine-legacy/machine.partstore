Might be useful to allow the user to move to the previous version of a project.
To help them find something that works better?

Need to make Repository directories machine agnostic.

In show, display stars or something for references that can be updated to the
newer version of the project. Later this can also be changed to show missing
or dangling project references that aren't in our repository.

Mutators:
 -Add Dependency Reference
 -Change Dependency Reference
 -Remove Dependency Reference
 -Unpackage Dependency Version
 -Delete Unpackaged Versions (reference remains?)
 -Publish/Archive New Version
 
It would be nice if a repository could have its own set of hooks, we could then
easily manage a repository inside of a git repository for example.

Add ReferenceStatus to store the condition of a ProjectReference? These can be
printed into the UI.

Repository.Save:
  Commit versions that have been added.
  Save Manifest.xml
  Run Hooks
  