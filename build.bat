:: import VS2013 environment variables
call "%VS120COMNTOOLS%\vsvars32.bat"

:: build the Debug squishlib
devenv %~dp0\FreeImage.2013.sln /Build "Debug|Win32" /Project %~dp0\FreeImage.2013.vcxproj /ProjectConfig "Debug|Win32"

:: same for the Release squishlib
devenv %~dp0\FreeImage.2013.sln /Build "Release|Win32" /Project %~dp0\FreeImage.2013.vcxproj /ProjectConfig "Release|Win32"

