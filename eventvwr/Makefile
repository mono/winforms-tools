RESX_RESOURCES = \
	EventEntryProperties.resources	\
	EventViewer.resources

MONO_EVENTLOG_TYPE=local:/tmp/mono/eventlog
export MONO_EVENTLOG_TYPE

ifeq (net_2_0, $(PROFILE))
MCS = gmcs
RESGEN = resgen2
DEFINE = NET_1_1,NET_2_0,ONLY_2_0
else
MCS = mcs
RESGEN = resgen
DEFINE = NET_1_1,ONLY_1_1
endif

IMAGES_RESOURCES = \
	Resources/error.ico		\
	Resources/info.ico		\
	Resources/warning.ico		\
	Resources/eventlog.ico		\
	Resources/computer.ico		\
	Resources/successaudit.ico	\
	Resources/failureaudit.ico	\
	Resources/properties.png	\
	Resources/refresh.png

all: clean eventvwr.exe

eventvwr.exe : $(RESX_RESOURCES) $(IMAGE_RESOURCES)
	$(MCS) -target:winexe -out:eventvwr.exe		\
		-define:$(DEFINE)			\
		-r:System.Drawing.dll			\
		-r:System.Windows.Forms.dll		\
		-win32icon:Resources/eventlog.ico	\
		-warnaserror				\
		@eventvwr.exe.sources			\
		@eventvwr.exe.resources

clean:
	rm -fR eventvwr.exe
	rm -fR EventViewer.csproj.user
	rm -fR EventViewer.suo
	rm -fR test.exe
	rm -fR bin
	rm -fR obj
	for p in $(RESX_RESOURCES) ; do \
		rm -fR $$p ; \
	done

test: eventvwr.exe test.exe
	mono test.exe
	mono eventvwr.exe

test.exe: test.cs
	$(MCS) -target:exe -out:test.exe test.cs

$(RESX_RESOURCES): %.resources: %.resx
	$(RESGEN) $<
