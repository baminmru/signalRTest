CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

drop table PerfDriverInfo;
drop table PerfData;
drop table AgentSettings;

create table PerfData(
    PerfDataId uuid not null primary key,
	Name text  not null,
	CPU  text null,
	RamTotal  text null,
	RamFree text null,
	IP text null,
	created timestamp without time zone NOT NULL default now()
);



create table PerfDriverInfo(
    PerfDriverInfoId uuid not null primary key,
	Name text  not null,
	TotalSize  text null,
	FreeSpace  text null,
	FreeUserSpace text null,
	PerfDataId uuid not null
);



ALTER TABLE public.perfdriverinfo
    ADD CONSTRAINT perfdriverinfo_fk FOREIGN KEY (perfdataid)
    REFERENCES public.perfdata (perfdataid) MATCH SIMPLE
    ON UPDATE RESTRICT
    ON DELETE RESTRICT
    NOT VALID;
	
CREATE TABLE public.AgentSettings
(
	AgentSettingsid uuid  not null primary key,
    ScanInterval integer
); 

insert into AgentSettings(AgentSettingsid, ScanInterval) values(gen_random_uuid (),10);


/*
delete from  PerfDriverInfo;
delete from  PerfData;
*/


