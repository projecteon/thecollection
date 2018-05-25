# https://weblog.west-wind.com/posts/2016/Jun/06/Publishing-and-Running-ASPNET-Core-Applications-with-IIS

& ".\build.ps1" win-x64

dotnet publish . --runtime win10-x64 --force -c Release