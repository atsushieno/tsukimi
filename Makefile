CONVERTER_EXE = tsukimi-tool.exe
CONVERTER_DLL = Processing.Importer.dll
CORE_DLL = Processing.Core.dll

CONVERTER_EXE_SOURCES = \
	driver.cs \

CONVERTER_DLL_SOURCES = \
	ProcessingProjectSource.cs \
	ProcessingSourceImporter.cs \
	ProcessingXapImporter.cs \
	ProcessingParser.cs \
	ProcessingAst.cs \
	ProcessingAstExpression.cs \
	ProcessingTokenizer.cs \
	CodeGenerator.cs

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
	StandardLibrary.Typography.cs

all: $(CORE_DLL) $(CONVERTER_EXE)

$(CONVERTER_EXE) : $(CONVERTER_EXE_SOURCES) $(CONVERTER_DLL)
	gmcs -debug -out:$(CONVERTER_EXE) $(CONVERTER_EXE_SOURCES) -r:$(CONVERTER_DLL)

$(CONVERTER_DLL) : $(CORE_DLL) $(CONVERTER_DLL_SOURCES)
	gmcs -debug -t:library -out:$(CONVERTER_DLL) $(CONVERTER_DLL_SOURCES)

$(CORE_DLL) : $(CORE_DLL_SOURCES)
	smcs -debug -t:library -out:$(CORE_DLL) $(CORE_DLL_SOURCES)

ProcessingParser.cs : ProcessingParser.jay
	jay -tcv < skeleton.cs  ProcessingParser.jay > ProcessingParser.cs

EXTRA_DISTFILES = Makefile README processing_syntax.txt

DISTFILES = $(CONVERTER_EXE_SOURCES) $(CONVERTER_DLL_SOURCES) $(CORE_DLL_SOURCES) $(EXTRA_DISTFILES)
BINFILES = $(CORE_DLL) $(CORE_DLL).mdb $(CONVERTER_DLL) $(CONVERTER_DLL).mdb $(CONVERTER_EXE) $(CONVERTER_EXE).mdb

clean:
	rm -f $(BINFILES)

dist:
	tar jcf tsukimi.tar.bz2 $(DISTFILES)
	tar jcf tsukimi-bin.tar.bz2 $(BINFILES) README processing_syntax.txt

