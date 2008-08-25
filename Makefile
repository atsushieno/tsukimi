
SOURCES = \
	driver.cs \
	ProcessingParser.cs \
	ProcessingAst.cs \
	ProcessingAstExpression.cs \
	ProcessingTokenizer.cs

all: driver.exe

driver.exe : $(SOURCES)
	gmcs -out:driver.exe $(SOURCES) 

ProcessingParser.cs : ProcessingParser.jay
	jay -tc < skeleton.cs  ProcessingParser.jay > ProcessingParser.cs

clean:
	rm -f driver.exe driver.exe.mdb

