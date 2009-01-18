CONVERTER_EXE = proce55ing2cli.exe
CORE_DLL = Proce55ing.Core.dll

CONVERTER_SOURCES = \
	driver.cs \
	ProcessingParser.cs \
	ProcessingAst.cs \
	ProcessingAstExpression.cs \
	ProcessingTokenizer.cs \
	CodeGenerator.cs

CORE_DLL_SOURCES = \
	StandardLibrary.cs \
	StandardLibrary.Conversion.cs \
	StandardLibrary.StringFunctions.cs \
	StandardLibrary.Math.cs

all: $(CORE_DLL) $(CONVERTER_EXE)

$(CONVERTER_EXE) : $(CONVERTER_SOURCES) $(CORE_DLL)
	smcs -debug -out:$(CONVERTER_EXE) $(CONVERTER_SOURCES) -r:$(CORE_DLL)

$(CORE_DLL) : $(CORE_DLL_SOURCES)
	smcs -debug -t:library -out:$(CORE_DLL) $(CORE_DLL_SOURCES)

ProcessingParser.cs : ProcessingParser.jay
	jay -tcv < skeleton.cs  ProcessingParser.jay > ProcessingParser.cs

clean:
	rm -f $(CORE_DLL) $(CONVERTER_EXE)

