# NamedPipeRunAsClient

This repo reproduces a bug in .NET Core 2.0 NamedPipeServerStream.RunAsClient.

When RunAsClient executes, it causes all threads in the process to switch to the stream's client user.

To repro:

1. `$ dotnet publish` both projects to directory `~/RunAsClient`.
1. `$ sudo dotnet ~/RunAsClient/NamedPipeServerStreamBug.Server.dll`
1. In another shell:
1. `$ sudo chmod 777 /tmp/CoreFxPipe_BugTest` to allow anyone to write to the pipe
1. `$ dotnet ~/RunAsClient/NamedPipeServerStreamBug.dll`

## Expected results

```
BackgroundThread started. Running as: 'root'.
This should be run as the client. Running as 'eerhardt'.
BackgroundThread continued. Running as: 'root'.
```

## Actual results

```
BackgroundThread started. Running as: 'root'.
This should be run as the client. Running as 'eerhardt'.
BackgroundThread continued. Running as: 'eerhardt'.
```
