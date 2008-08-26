
SOURCES = \
	driver.cs \
	ProcessingParser.cs \
	ProcessingAst.cs \
	ProcessingAstExpression.cs \
	ProcessingTokenizer.cs

all: driver.exe

driver.exe : $(SOURCES)
	gmcs -debug -out:driver.exe $(SOURCES) 

ProcessingParser.cs : ProcessingParser.jay
	jay -tcv < skeleton.cs  ProcessingParser.jay > ProcessingParser.cs

clean:
	rm -f driver.exe driver.exe.mdb

