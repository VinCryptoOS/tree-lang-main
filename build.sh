rootDir=`pwd`

mkdir -p ./build/builder

dotnet publish $rootDir/builder --use-current-runtime true --self-contained false -c Release --output $rootDir/build/builder

exitCode=$?
SUCCESS=0

cd $rootDir
if [ "$exitCode" -ne $SUCCESS ]
then
    echo "\nERROR at build 'builder'\n"
    exit;
fi

bash $rootDir/fbuild.sh


# cp -r ../trusts build

# dotnet publish ./tests/exe/ --configuration Release

# 7z a -y -t7z -stl -m0=lzma -mx=9 -ms=on -bb0 -bd -ssc -ssw ../build/vinny-socks5-proxy-net70.7z ./build/arc/vinny-socks5-proxy &&

