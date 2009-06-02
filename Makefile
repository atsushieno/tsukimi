SUBDIRS = Processing.Core Processing.Importer tsukimi-tool

all:
	cd Processing.Core && make || exit -1;
	cd Processing.Importer && make || exit -1;
	cd tsukimi-tool && make || exit -1;

clean:
	cd Processing.Core && make clean || exit -1;
	cd Processing.Importer && make clean || exit -1;
	cd tsukimi-tool && make clean || exit -1;
