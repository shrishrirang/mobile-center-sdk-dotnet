#!/bin/bash
set -e

# Replace version in all the assemblies
for file in `find Apps -name AssemblyInfo.cs | grep Demo`
do
    sed -E -i '' "s/(AssemblyInformationalVersion\(\")(.*)\"\)/\1$1\")/g" $file
    sed -E -i '' "s/(AssemblyFileVersion\(\")(.*)\"\)/\1$1.0\")/g" $file
done

# Replace android versions
for file in `find Apps -name AndroidManifest.xml | grep Properties | grep Demo`
do
    sed -E -i '' "s/(android:versionName=\")([^\"]+)/\1$1/g" $file
    versionCode=$((`grep versionCode $file | sed -E "s/^.*versionCode=\"([^\"]*)\".*$/\1/"`+1))
    sed -E -i '' "s/(android:versionCode=\")([^\"]+)/\1$versionCode/g" $file
done

# Replace ios versions
for file in `find Apps -name Info.plist | grep Demo | egrep -v "/(obj|bin)/"`
do
    versionName=$1 perl -pi -e 'undef $/; s/(CFBundleVersion<\/key>\s*<string>)([^<]*)/${1}$ENV{versionName}/' $file
    versionName=$1 perl -pi -e 'undef $/; s/(CFBundleShortVersionString<\/key>\s*<string>)([^<]*)/${1}$ENV{versionName}/' $file
done

# Update nuget package dependencies for Mobile Center
./build.sh -t UpdateDemoDependencies