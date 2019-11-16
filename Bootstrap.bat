@echo off

rem CSBuildRun, Bootstrap build batch file
rem
rem File:	    Bootstrap.bat
rem Revision:	    1.0

setlocal enabledelayedexpansion
 
set N=0

IF EXIST C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\csc.exe (
  set dotnetname[!N!]=".Net2.0"
  set dotnetpath[!N!]=C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\
  set /a N=N+1
) 

IF EXIST C:\WINDOWS\Microsoft.NET\Framework64\v2.0.50727\csc.exe (
  set dotnetname[!N!]=."Net2.0(x64)"
  set dotnetpath[!N!]=C:\WINDOWS\Microsoft.NET\Framework64\v2.0.50727\
  set /a N=N+1
) 

IF EXIST C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\csc.exe (
  set dotnetname[!N!]=".Net4.0"
  set dotnetpath[!N!]=C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\
  set /a N=N+1
) 

IF EXIST C:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319\csc.exe (
  set dotnetname[!N!]=".Net4.0(x64)"
  set dotnetpath[!N!]=C:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319\
  set /a N=N+1
) 

IF not %N%==0 ECHO Please select target dot net version.

IF not "%dotnetpath[0]%"=="" ECHO 0-%dotnetname[0]% (%dotnetpath[0]%)
IF not "%dotnetpath[1]%"=="" ECHO 1-%dotnetname[1]% (%dotnetpath[1]%)
IF not "%dotnetpath[2]%"=="" ECHO 2-%dotnetname[2]% (%dotnetpath[2]%)
IF not "%dotnetpath[3]%"=="" ECHO 3-%dotnetname[3]% (%dotnetpath[3]%) 
 
set /p dotnettarget=""

set dotnetselected=FALSE
IF %dotnettarget%==0 (
  set dotnetselected=TRUE
  set dotnetver="Net20"
)
IF %dotnettarget%==1 (
  set dotnetselected=TRUE
  set dotnetver="Net20"
)
IF %dotnettarget%==2 (
  set dotnetselected=TRUE
  set dotnetver="Net40"
)
IF %dotnettarget%==3 (
  set dotnetselected=TRUE
  set dotnetver="Net40"
)

IF %dotnetselected%==FALSE (
  ECHO Invalid target number.
  GOTO ONEXIT
)

set /p buildtype="Please select build type (0:command line 1:command line(admin) 2:window 3:window(admin)."

set buildtypeselected=FALSE
set additionalparam=
IF %buildtype%==0 (
  set buildtypeselected=TRUE
  set additionalparam=/define:%dotnetver%
)
IF %buildtype%==1 (
  set buildtypeselected=TRUE
  set additionalparam=/win32manifest:App.manifest /define:%dotnetver%
)
IF %buildtype%==2 (
  set buildtypeselected=TRUE
  set additionalparam=/target:winexe /define:%dotnetver%;GUI
)
IF %buildtype%==3 (
  set buildtypeselected=TRUE
  set additionalparam=/target:winexe /win32manifest:App.manifest /define:%dotnetver%;GUI
)

IF %buildtypeselected%==FALSE (
  ECHO Invalid build type.
  GOTO ONEXIT
)

set CSC_PATH=!dotnetpath[%dotnettarget%]!
set PATH=%CSC_PATH%;%PATH%
csc.exe %additionalparam% /main:YellowStrelitzia.CSTools.Bootstrap /out:BuildAndRun.exe Bootstrap.cs



:ONEXIT

endlocal
