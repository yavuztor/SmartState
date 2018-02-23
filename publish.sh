#! /bin/bash
PKGNAME=`dotnet pack | tail -1 | egrep -o "[^\/]+.nupkg"`
echo PKGNAME is $PKGNAME
pushd SmartState/bin/Debug
dotnet nuget push $PKGNAME -k "$NUGET_KEY" -s "https://api.nuget.org/v3/index.json"
popd