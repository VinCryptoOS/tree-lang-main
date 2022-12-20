rootDir=`pwd`

rm -rf ./build/
mkdir build

cd builder

dotnet publish -c Release

# cp -r ../trusts build

# dotnet publish ./tests/exe/ --configuration Release

# 7z a -y -t7z -stl -m0=lzma -mx=9 -ms=on -bb0 -bd -ssc -ssw ../build/vinny-socks5-proxy-net70.7z ./build/arc/vinny-socks5-proxy &&

