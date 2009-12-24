
EXTRA_DIST =  Makefile

# Warning: This is an automatically generated file, do not edit!

srcdir=.
top_srcdir=..

include $(top_srcdir)/config.make

ifeq ($(CONFIG),DEBUG)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = bin/Debug/tsukimi-tool.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../Processing.Importer/bin/Debug/Processing.Importer.dll
BUILD_DIR = bin/Debug

PROCESSING_IMPORTER_DLL_SOURCE=../Processing.Importer/bin/Debug/Processing.Importer.dll
PROCESSING_IMPORTER_DLL_MDB_SOURCE=../Processing.Importer/bin/Debug/Processing.Importer.dll.mdb
PROCESSING_IMPORTER_DLL_MDB=$(BUILD_DIR)/Processing.Importer.dll.mdb
TSUKIMI_TOOL_EXE_MDB_SOURCE=bin/Debug/tsukimi-tool.exe.mdb
TSUKIMI_TOOL_EXE_MDB=$(BUILD_DIR)/tsukimi-tool.exe.mdb

endif

ifeq ($(CONFIG),RELEASE)
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize-
ASSEMBLY = bin/Release/tsukimi-tool.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../Processing.Importer/bin/Release/Processing.Importer.dll
BUILD_DIR = bin/Release

PROCESSING_IMPORTER_DLL_SOURCE=../Processing.Importer/bin/Release/Processing.Importer.dll
PROCESSING_IMPORTER_DLL_MDB=
TSUKIMI_TOOL_EXE_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(PROCESSING_IMPORTER_DLL) \
	$(PROCESSING_IMPORTER_DLL_MDB) \
	$(TSUKIMI_TOOL_EXE_MDB)  

BINARIES = \
	$(TSUKIMI_TOOL)  


RESGEN=resgen2

PROCESSING_IMPORTER_DLL = $(BUILD_DIR)/Processing.Importer.dll
TSUKIMI_TOOL = $(BUILD_DIR)/tsukimi-tool

FILES = \
	driver.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	tsukimi-tool.in 

REFERENCES =  \
	System.Core

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

#Targets
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES)  $(top_srcdir)/config.make

include $(top_srcdir)/Makefile.include
#include $(srcdir)/custom-hooks.make



$(eval $(call emit-deploy-target,PROCESSING_IMPORTER_DLL))
$(eval $(call emit-deploy-target,PROCESSING_IMPORTER_DLL_MDB))
$(eval $(call emit-deploy-wrapper,TSUKIMI_TOOL,tsukimi-tool,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'


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
	$(call cp,$(PROCESSING_IMPORTER_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(PROCESSING_IMPORTER_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call cp,$(TSUKIMI_TOOL_EXE_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	mkdir -p '$(DESTDIR)$(bindir)'
	$(call cp,$(TSUKIMI_TOOL),$(DESTDIR)$(bindir))
	make post-install-local-hook prefix=$(prefix)

uninstall-local: $(ASSEMBLY) $(ASSEMBLY_MDB)
	make pre-uninstall-local-hook prefix=$(prefix)
	make uninstall-satellite-assemblies prefix=$(prefix)
	$(call rm,$(ASSEMBLY),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(ASSEMBLY_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(PROCESSING_IMPORTER_DLL),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(PROCESSING_IMPORTER_DLL_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(TSUKIMI_TOOL_EXE_MDB),$(DESTDIR)$(libdir)/$(PACKAGE))
	$(call rm,$(TSUKIMI_TOOL),$(DESTDIR)$(bindir))
	make post-uninstall-local-hook prefix=$(prefix)

install: install-local
uninstall: uninstall-local
clean: clean-local

include $(top_srcdir)/rules.make
