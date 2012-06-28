Name "Example1"
OutFile "SharpBooks-Install.exe"
SetCompressor lzma
InstallDir $PROGRAMFILES\SharpBooks
InstallDirRegKey HKLM "Software\SharpBooks" "InstallDir"
RequestExecutionLevel admin

!include "MUI2.nsh"

!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

Section "SharpBooks"
    SetOutPath $INSTDIR
    File SharpBooks.exe
    File *.dll
    WriteUninstaller "$INSTDIR\Uninstall.exe"
    SetShellVarContext All
    CreateDirectory $SMPROGRAMS\SharpBooks
    CreateShortCut "$SMPROGRAMS\SharpBooks\SharpBooks.lnk" $INSTDIR\SharpBooks.exe
    CreateShortCut "$SMPROGRAMS\SharpBooks\Uninstall SharpBooks.lnk" $INSTDIR\Uninstall.exe
SectionEnd

Section "Uninstall"
    Delete "$INSTDIR\Uninstall.exe"
    RMDir "$INSTDIR"
    DeleteRegKey HKLM "Software\SharpBooks"
    SetShellVarContext All
    RMDir $SMPROGRAMS\SharpBooks
SectionEnd
