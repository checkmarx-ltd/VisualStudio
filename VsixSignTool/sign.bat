
".\vsixsigntool.exe" sign /f ".\mymcre.pfx" /p neige /fd sha1 /sha1 "d1 97 08 c9 fc 45 86 43 51 65 76 f4 43 52 20 1f 89 a0 f3 40" "..\CxViewerVSIX\bin\Release\CxViewerVSIX.vsix"
".\vsixsigntool.exe" verify "..\CxViewerVSIX\bin\Release\CxViewerVSIX.vsix"