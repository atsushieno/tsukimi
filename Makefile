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
	StandardLibrary.Conversion.cs \
	StandardLibrary.Files.cs \
	StandardLibrary.HiddenAPI.cs \
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

clean:
	rm -f $(CORE_DLL) $(CONVERTER_EXE)

