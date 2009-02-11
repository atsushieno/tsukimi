mkdir $1
cd $1
cp ../../../Processing.Core.dll* .
sed -e "s/REPLACE_HERE/FooBar/g" <../AppManifest_Template.xaml > AppManifest.xaml
mono --debug ../../../processingimporter.exe ../$2 > $1.cs
mxap
