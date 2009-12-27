MONO_PATH=build mono processing-api-generator/bin/Debug/processing-api-generator.exe Processing.Core/bin/Debug/Processing.Core.dll | sort | uniq > Processing.Importer/apilist.txt
