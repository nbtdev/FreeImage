# freeimage

As Roland of Gilead understood, the world moved on. 

In this case, the world has moved on from CVS. 

As I would like to use FreeImage as a git submodule in other projects (and not perform an exercise in obvious futility by trying to get the FreeImage authors to move to git), rather than distributing source (or worse, maintaining binary builds), I took a fork of the FreeImage source from the Windows zipfile, version 3.17, and from that created this repo.

I had started using imazen's git FreeImage repo here on Github, but that one does not include the library dependencies, and the Windows build comes with VS solution/project files, saving the need to generate them (which also eliminates the need to have CMake as a pre-req for my projects). 

I do not intend for this to be a mirror of the FreeImage CVS repository at SourceForge, and if I update this repo here, it will be infrequently (FreeImage is a mature library as it is, so changes to the core source are infrequent as well). 
