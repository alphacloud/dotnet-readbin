Contributing
============

## Pull requests

- Add XML documentation to all public members.
- Add unit-tests covering functionality implemented (unless this is trivial fix or logic was not changed).
- Make sure all tests are passing.
- Squash commits. Ideally it should be 1 commit per feature implemented or bug fixed). If there are several features/bugs 
addressed by pull request, consider creating several PRs.
- Create pull request against **develop** branch.


## Release process

* Release notes are generated based on closed issues assigned to milestone `version`

- Create `releases/version` branch of `develop`
- Prepare for release -> (produces beta- versions)
- Merge `releases/version` to `master`
- Merge `releases/version` to develop
- Remove releases/
- Make sure all issues included in release are closed and labelled correctly
- Tag master with [release/]version -> produces release version
