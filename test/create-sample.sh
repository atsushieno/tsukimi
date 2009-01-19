mkdir $1
cd $1
cp ../../Proce55ing.Core.dll* .
sed -e "s/REPLACE_HERE/FooBar/g" <../AppManifest_Template.xaml > AppManifest.xaml
mono --debug ../../proce55ing2cli.exe ../$2 > $1.cs
mxap
