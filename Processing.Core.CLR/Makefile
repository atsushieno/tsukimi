CORE_DLL = ../build/Processing.Core.CLR.dll

X = ../Processing.Core

CORE_DLL_SOURCES = \
	$(X)/StandardLibrary.cs \
	$(X)/StandardLibrary.ArrayFunctions.cs \
	$(X)/StandardLibrary.Color.cs \
	$(X)/StandardLibrary.Conversion.cs \
	$(X)/StandardLibrary.Files.cs \
	$(X)/StandardLibrary.HiddenAPI.cs \
	$(X)/StandardLibrary.Miscellaneous.cs \
	$(X)/StandardLibrary.PImage.cs \
	$(X)/StandardLibrary.StringFunctions.cs \
	$(X)/StandardLibrary.Math.cs \
	$(X)/StandardLibrary.Shapes.cs \
	$(X)/StandardLibrary.Transform.cs \
	$(X)/StandardLibrary.Typography.cs \
	AssemblyInfo.cs

all: $(CORE_DLL)

$(CORE_DLL) : $(CORE_DLL_SOURCES)
	gmcs -debug -t:library -d:DESKTOP -pkg:moonlight-gtk -out:$(CORE_DLL) $(CORE_DLL_SOURCES)

BINFILES = $(CORE_DLL) $(CORE_DLL).mdb

clean:
	rm -f $(BINFILES)

