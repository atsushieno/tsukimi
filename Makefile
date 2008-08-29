CONVERTER_EXE = proce55ing2xaml.exe
CORE_DLL = Proce55ing.Core.dll

CONVERTER_SOURCES = \
	driver.cs \
	ProcessingParser.cs \
	ProcessingAst.cs \
	ProcessingAstExpression.cs \
	ProcessingTokenizer.cs \

CORE_DLL_SOURCES = \
	StandardLibrary.cs \
	StandardLibrary.Conversion.cs \
	StandardLibrary.StringFunctions.cs

all: $(CORE_DLL) $(CONVERTER_EXE)

$(CONVERTER_EXE) : $(CONVERTER_SOURCES) $(CORE_DLL)
	gmcs -debug -out:$(CONVERTER_EXE) $(CONVERTER_SOURCES) -r:System.Windows.dll -r:$(CORE_DLL)

$(CORE_DLL) : $(CORE_DLL_SOURCES)
	gmcs -debug -t:library -out:$(CORE_DLL) $(CORE_DLL_SOURCES) -r:System.Windows.dll

ProcessingParser.cs : ProcessingParser.jay
	jay -tcv < skeleton.cs  ProcessingParser.jay > ProcessingParser.cs

clean:
	rm -f driver.exe driver.exe.mdb

