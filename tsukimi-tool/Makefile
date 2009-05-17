SUBDIRS = Processing.Core Processing.Importer tsukimi-tool

CONVERTER_EXE = ../build/tsukimi-tool.exe
CONVERTER_DLL = ../build/Processing.Importer.dll

CONVERTER_EXE_SOURCES = \
	driver.cs \

all: $(CONVERTER_EXE)

$(CONVERTER_EXE) : $(CONVERTER_EXE_SOURCES) $(CONVERTER_DLL)
	gmcs -debug -out:$(CONVERTER_EXE) $(CONVERTER_EXE_SOURCES) -r:$(CONVERTER_DLL)

BINFILES = $(CONVERTER_EXE) $(CONVERTER_EXE).mdb

clean:
	rm -f $(BINFILES)

