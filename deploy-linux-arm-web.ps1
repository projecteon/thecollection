param ([string]$ip, [string]$destination, [string]$username)

& ".\publish-linux-arm.ps1"

& pscp.exe -r .\X.MeetingRoom.Presentation.Web\bin\Debug\netcoreapp2.0\linux-arm\publish\* ${username}@${ip}:${destination}

& plink.exe -v -ssh ${username}@${ip} chmod u+x,o+x ${destination}