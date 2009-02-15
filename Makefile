CONVERTER_EXE = processingimporter.exe
CORE_DLL = Processing.Core.dll

CONVERTER_SOURCES = \
	driver.cs \
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

$(CONVERTER_EXE) : $(CONVERTER_SOURCES) $(CORE_DLL)
	gmcs -debug -out:$(CONVERTER_EXE) $(CONVERTER_SOURCES) -r:$(CORE_DLL)

$(CORE_DLL) : $(CORE_DLL_SOURCES)
	smcs -debug -t:library -out:$(CORE_DLL) $(CORE_DLL_SOURCES)

ProcessingParser.cs : ProcessingParser.jay
	jay -tcv < skeleton.cs  ProcessingParser.jay > ProcessingParser.cs

EXTRA_DISTFILES = Makefile README processing_syntax.txt

DISTFILES = $(CONVERTER_SOURCES) $(CORE_DLL_SOURCES) $(EXTRA_DISTFILES)

clean:
	rm -f $(CORE_DLL) $(CORE_DLL).mdb $(CONVERTER_EXE) $(CONVERTER_EXE).mdb

dist:
	tar jcf tsukimi.tar.bz2 $(DISTFILES)
	tar jcf tsukimi-bin.tar.bz2 Processing.Core.dll Processing.Core.dll.mdb processingimporter.exe processingimporter.exe.mdb README processing_syntax.txt
