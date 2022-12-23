rootDir=`pwd`

rm -rf ./build/
mkdir -p ./build/builder

dotnet clean

bash $rootDir/build.sh
