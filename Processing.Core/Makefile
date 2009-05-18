CORE_DLL = ../build/Processing.Core.dll

CORE_DLL_SOURCES = \
	StandardLibrary.cs \
	StandardLibrary.ArrayFunctions.cs \
	StandardLibrary.Color.cs \
	StandardLibrary.Conversion.cs \
	StandardLibrary.Files.cs \
	StandardLibrary.HiddenAPI.cs \
	StandardLibrary.Miscellaneous.cs \
	StandardLibrary.PImage.cs \
	StandardLibrary.StringFunctions.cs \
	StandardLibrary.Math.cs \
	StandardLibrary.Shapes.cs \
	StandardLibrary.Transform.cs \
	StandardLibrary.Typography.cs

all: $(CORE_DLL)

$(CORE_DLL) : $(CORE_DLL_SOURCES)
	smcs -debug -t:library -out:$(CORE_DLL) $(CORE_DLL_SOURCES)

BINFILES = $(CORE_DLL) $(CORE_DLL).mdb

clean:
	rm -f $(BINFILES)

