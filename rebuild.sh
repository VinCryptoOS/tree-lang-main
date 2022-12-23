rootDir=`pwd`

rm -rf ./build/
mkdir -p ./build/builder

bash $rootDir/build.sh
