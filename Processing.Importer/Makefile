
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/Processing.Importer.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../processing-api-generator/bin/Debug/processing-api-generator.exe
BUILD_DIR = bin/Debug

PROCESSING_API_GENERATOR_EXE_SOURCE=../processing-api-generator/bin/Debug/processing-api-generator.exe
PROCESSING_IMPORTER_DLL_MDB_SOURCE=bin/Debug/Processing.Importer.dll.mdb
PROCESSING_IMPORTER_DLL_MDB=$(BUILD_DIR)/Processing.Importer.dll.mdb

endif

ifeq ($(CONFIG),RELEASE)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/Processing.Importer.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../processing-api-generator/bin/Release/processing-api-generator.exe
BUILD_DIR = bin/Release

PROCESSING_API_GENERATOR_EXE_SOURCE=../processing-api-generator/bin/Release/processing-api-generator.exe
PROCESSING_IMPORTER_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(PROCESSING_API_GENERATOR_EXE) \
	$(PROCESSING_IMPORTER_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(PROCESSING_IMPORTER_PC)  


RESGEN=resgen2

PROCESSING_API_GENERATOR_EXE = $(BUILD_DIR)/processing-api-generator.exe
PROCESSING_IMPORTER_PC = $(BUILD_DIR)/processing.importer.pc

FILES = \
	CodeGenerator.cs \
	ProcessingAst.cs \
	ProcessingAstExpression.cs \
	ProcessingParser.cs \
	ProcessingProjectSource.cs \
	ProcessingSourceImporter.cs \
	ProcessingTokenizer.cs \
	ProcessingXapImporter.cs 

DATA_FILES = 

RESOURCES = \
	apilist.txt,Processing.Importer.apilist.txt 

EXTRAS = \
	ProcessingParser.jay \
	processing.importer.pc.in 

REFERENCES =  \
	System \
	System.Core

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,PROCESSING_API_GENERATOR_EXE))
$(eval $(call emit-deploy-wrapper,PROCESSING_IMPORTER_PC,processing.importer.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

# Targets for Custom commands
DEBUG_BeforeBuild:
	(cd /Users/atsushieno/svn/tsukimi && sh apigen.sh)


$(ASSEMBLY_MDB): $(ASSEMBLY)
$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	make pre-all-local-hook prefix=$(prefix)
	mkdir -p $(shell dirname $(ASSEMBLY))
	make $(CONFIG)_BeforeBuild
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
	make $(CONFIG)_AfterBuild
	make post-all-local-hook prefix=$(prefix)

install-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-install-local-hook prefix=$(prefix)
	make install-satellite-assemblies prefix=$(prefix)
	mkdir -p '$(DESTDIR)$(libdir)/$(PACKAGE)'
	$(call cp,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(PROCESSING_API_GENERATOR_EXE),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(PROCESSING_IMPORTER_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(libdir)/pkgconfig'
	$(call cp,$(PROCESSING_IMPORTER_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(PROCESSING_API_GENERATOR_EXE),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(PROCESSING_IMPORTER_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(PROCESSING_IMPORTER_PC),$(DESTDIR)$(libdir)/pkgconfig)
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
