# CSBuildRun
Simple C# on demand executor project

If you prefer to use C# insted of Powershell, this tool might help you.

Bootstrap.bat try to check your window's dotnet runtime , and it produce
Executable , that will load C# source code at runtime, compile it , and
call that C# code.

If you launch Bootstrap.bat, it ask you which version of .net framework want
to use, 

>Please select target dot net version.
>0-".Net4.0" (C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\)
>1-".Net4.0(x64)" (C:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319\)

after select framwork, then it also ask , which type of executable shoud be built.

>Please select build type (0:command line 1:command line(admin) 2:window 3:window(admin).

Then Bootstrap.bat will generate Executable. (BuildAndRun.exe)
BuildAndRun.exe will load configuration from BuildAndRun.exe.config and then 
load, compile and execute Main.cs file, on demand.

`<Japanese>`
Powershell は覚えていないが、バッチファイルやWSHを利用では役不足の場合、このツールでC#の
コードをシェル的に利用できるかもしれません。

Bootstrap.batを起動し、入力に応答して、BuildAndRun.exeを生成してください。

生成したのち、BuildAndRun.exe.configとMain.csを編集して、 BuildAndRun.exeを
実行すると、起動ごとにMain.csをコンパイルし、実行します。


`</Japanese>`
