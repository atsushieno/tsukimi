CONVERTER_DLL = ../build/Processing.Importer.dll

CONVERTER_DLL_SOURCES = \
	ProcessingProjectSource.cs \
	ProcessingSourceImporter.cs \
	ProcessingXapImporter.cs \
	ProcessingParser.cs \
	ProcessingAst.cs \
	ProcessingAstExpression.cs \
	ProcessingTokenizer.cs \
	CodeGenerator.cs

all: $(CONVERTER_DLL)

$(CONVERTER_DLL) : $(CONVERTER_DLL_SOURCES) apilist.txt
	gmcs -debug -t:library -out:$(CONVERTER_DLL) $(CONVERTER_DLL_SOURCES) -resource:apilist.txt

ProcessingParser.cs : ProcessingParser.jay
	jay -tcv < skeleton.cs  ProcessingParser.jay > ProcessingParser.cs

BINFILES = $(CONVERTER_DLL) $(CONVERTER_DLL).mdb

apigen:
	mv ../libs/Mono.Cecil.dll .
	gmcs processing-apilist-generator.cs -r:Mono.Cecil.dll
	mono processing-apilist-generator.exe | sort | uniq > apilist.txt
	mv Mono.Cecil.dll ../libs

clean:
	rm -f $(BINFILES)

