csc /d:DEBUG /debug DbgViewDemo.cs
csc /t:library FuslogvwDemoLibrary.cs
csc /t:winexe /r:./FuslogvwDemoLibrary.dll FuslogvwDemo.cs