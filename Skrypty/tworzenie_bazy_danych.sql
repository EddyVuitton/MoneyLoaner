use master;
go

begin
	--https://stackoverflow.com/a/7469167
	declare @sql_drop_all_connections varchar(max);

	select @sql_drop_all_connections = coalesce(@sql_drop_all_connections,'') + 'kill ' + convert(varchar, spid) + ';'
	from master..sysprocesses
	where dbid in (
		db_id('money_loaner_data'),
		db_id('money_loaner_logic'),
		db_id('money_loaner_shdlog')
	) and spid <> @@spid

	exec (@sql_drop_all_connections);

	declare @exec nvarchar(max);
	if db_id('money_loaner_data') is not null
	begin
		set @exec = 'drop database money_loaner_data;';
		exec (@exec);
	end

	if db_id('money_loaner_logic') is not null
	begin
		set @exec = 'drop database money_loaner_logic;';
		exec (@exec);
	end

	if db_id('money_loaner_shdlog') is not null
	begin
		set @exec = 'drop database money_loaner_shdlog;';
		exec (@exec);
	end
end;
go

create database money_loaner_data;
go

use money_loaner_data;
go

create schema scoring;
go

create table rachunek_bankowy (
	rb_id int primary key identity(1, 1),
	rb_numer varchar(26) not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name(),
	check (len(rb_numer) = 26)
);
go

create table pozyczka_klient (
	pk_id int primary key identity(1, 1),
	pk_numer nvarchar(max) not null,
	pk_imie nvarchar(max) not null,
	pk_nazwisko nvarchar(max) not null,
	pk_pesel varchar(11) not null,
	pk_data_dodania datetime not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name(),
	check (len(pk_pesel) = 11)
);
go

create table uzytkownik_konto (
	uk_id int primary key identity(1, 1),
	uk_email nvarchar(max) not null,
	uk_haslo nvarchar(max) not null,
	uk_data_dodania datetime not null,
	uk_czy_aktywne bit not null,
	uk_pk_id int not null foreign key references pozyczka_klient (pk_id),
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table klient_rachunek_bankowy (
	krb_id int primary key identity(1, 1),
	krb_rb_id int not null foreign key references rachunek_bankowy (rb_id),
	krb_pk_id int not null foreign key references pozyczka_klient (pk_id),
	krb_data_dodania datetime not null,
	krb_data_zakonczenia datetime null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table email (
	em_id int primary key identity(1, 1),
	em_pk_id int not null foreign key references pozyczka_klient (pk_id),
	em_nazwa nvarchar(max) not null,
	em_data_dodania datetime not null,
	em_data_zakonczenia datetime null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table telefon (
	tn_id int primary key identity(1, 1),
	tn_pk_id int not null foreign key references pozyczka_klient (pk_id),
	tn_nazwa nvarchar(max) not null,
	tn_data_dodania datetime not null,
	tn_data_zakonczenia datetime null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table pozyczka (
	po_id int primary key identity(1, 1),
	po_numer nvarchar(max) not null,
	po_rb_id int not null foreign key references rachunek_bankowy (rb_id),
	po_pk_id int not null foreign key references pozyczka_klient (pk_id),
	po_data_dodania datetime not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table pozyczka_wniosek (
	pwn_id int primary key identity(1000001, 1),
	pwn_po_id int not null foreign key references pozyczka (po_id),
	pwn_imie nvarchar(max) not null,
	pwn_nazwisko nvarchar(max) not null,
	pwn_numer_telefonu nvarchar(max) not null,
	pwn_pesel varchar(11) not null,
	pwn_email nvarchar(max) not null,
	pwn_miesieczny_dochod int not null,
	pwn_miesieczne_wydatki int not null,
	pwn_numer_konta varchar(26) not null,
	pwn_data_dodania datetime not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name(),
	check (len(pwn_pesel) = 11),
	check (len(pwn_numer_konta) = 26)
);
go

create table pozyczka_szczegoly_oferty (
	pszo_id int primary key identity(1, 1),
	pszo_pwn_id int not null foreign key references pozyczka_wniosek (pwn_id),
	pszo_rata_od decimal(18, 2) not null,
	pszo_data_pierwszej_raty date not null,
	pszo_rrso decimal(18, 2) not null,
	pszo_okres_splaty int not null,
	pszo_kwota_wnioskowana decimal(18, 2) not null,
	pszo_prowizja decimal(18, 2) not null,
	pszo_odsetki decimal(18, 2) not null,
	pszo_calkowita_kwota_do_zaplaty decimal(18, 2) not null,
	pszo_raty_platne_do int not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table pozyczka_harmonogram (
	ph_id int primary key identity(1, 1),
	ph_po_id int not null foreign key references pozyczka (po_id),
	ph_nazwa nvarchar(max) not null,
	ph_data_dodania datetime not null,
	ph_data_rozpoczecia date not null,
	ph_data_zakonczenia date not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table pozyczka_rata (
	por_id int primary key identity(1, 1),
	por_ph_id int not null foreign key references pozyczka_harmonogram (ph_id),
	por_numer int not null,
	por_data_wymagalnosci datetime not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table ksiegowanie_typ (
	kst_id int primary key identity(1, 1),
	kst_nazwa nvarchar(max) not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table ksiegowanie (
	ks_id int primary key identity(1, 1),
	ks_por_id int null foreign key references pozyczka_rata (por_id),
	ks_data_dodania datetime not null,
	ks_data_operacji datetime not null,
	ks_kst_id int not null foreign key references ksiegowanie_typ (kst_id),
	ks_uwagi nvarchar(max) null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table ksiegowanie_konto (
	ksk_id int primary key identity(1, 1),
	ksk_nazwa nvarchar(max) not null,
	ksk_czy_techniczne bit not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table ksiegowanie_konto_subkonto (
	ksksub_id int primary key identity(1, 1),
	ksksub_ksk_id int not null foreign key references ksiegowanie_konto (ksk_id),
	ksksub_nazwa nvarchar(max) not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table ksiegowanie_dekret (
	ksd_id int primary key identity(1, 1),
	ksd_ks_id int not null foreign key references ksiegowanie (ks_id),
	ksd_por_id int null foreign key references pozyczka_rata (por_id),
	ksd_ksk_id int not null foreign key references ksiegowanie_konto (ksk_id),
	ksd_ksksub_id int null foreign key references ksiegowanie_konto_subkonto (ksksub_id),
	ksd_kwota_wn decimal(18, 2) not null,
	ksd_kwota_ma decimal(18, 2) not null,
	ksd_rb_id int null foreign key references rachunek_bankowy (rb_id),
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name(),
	check ((ksd_kwota_wn = 0 and ksd_kwota_ma != 0) or (ksd_kwota_wn != 0 and ksd_kwota_ma = 0))
);
go

create table scoring.model (
	scrm_id int primary key identity(1, 1),
	scrm_nazwa nvarchar(max) not null,
	scrm_data_dodania datetime not null,
	scrm_data_zakonczenia datetime null,
	scrm_czy_aktywne as iif(scrm_data_zakonczenia is null, 1, 0),
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table scoring.pole (
	scrp_id int primary key identity(1, 1),
	scrp_scrm_id int not null foreign key references scoring.model (scrm_id),
	scrp_nazwa nvarchar(max) not null,
	scrp_skrot nvarchar(max) not null,
	scrp_zapytanie nvarchar(max) not null,
	scrp_data_dodania datetime not null,
	scrp_data_zakonczenia datetime null,
	scrp_czy_aktywne as iif(scrp_data_zakonczenia is null, 1, 0),
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table scoring.przeliczenie (
	scrprz_id int primary key identity(1, 1),
	scrprz_data_dodania datetime not null,
	scrprz_data_zakonczenia datetime null,
	scrprz_po_id int not null foreign key references pozyczka (po_id),
	scrprz_scrm_id int not null foreign key references scoring.model (scrm_id),
	scrprz_czy_dyskwalifikacja bit null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table scoring.wartosc (
	scrw_id int primary key identity(1, 1),
	scrw_scrp_id int not null foreign key references scoring.pole (scrp_id),
	scrw_scrprz_id int not null foreign key references scoring.przeliczenie (scrprz_id),
	scrw_wartosc bit not null,
	scrw_start datetime not null,
	scrw_koniec datetime not null,
	scrw_blad nvarchar(max) null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create database money_loaner_shdlog;
go

--begin --tworzenie shadow loga
--	drop table if exists #tabele;
--	select  t.name table_name
--	into #tabele
--	from sys.tables t

--	declare @c int = (select count(1) from #tabele);
--	declare @i int = 0;

--	while (@i < @c)
--	begin
--		declare @table nvarchar(max) = (select top 1 table_name from #tabele);
--		declare @create_table_skrypt nvarchar(max) = 'select top 0 * into money_loaner_shdlog..' + @table + ' from ' + @table + '; alter table money_loaner_shdlog..' + @table + ' add aud_oper char;';

--		exec (@create_table_skrypt);
		
--		delete #tabele where table_name = @table;
--		set @i = @i + 1;
--	end

--	-- ... to do ...
--end;
--go

begin --tworzenie triggerów
	drop table if exists #triggery;
	select c.object_id, c.name column_name, t.name table_name, iif(s.name = 'dbo', null, s.name) schema_name
	into #triggery
	from sys.tables t
	join sys.columns c on t.object_id = c.object_id
	join sys.schemas s on s.schema_id = t.schema_id
	where c.is_identity = 1

	declare @c int = (select count(1) from #triggery);
	declare @i int = 0;

	while (@i < @c)
	begin
		declare @table nvarchar(max) = (select top 1 table_name from #triggery);
		declare @schema nvarchar(max) = (select schema_name from #triggery where table_name = @table);
		declare @trigger_name nvarchar(max) = 'tr_' + isnull(@schema + '_', '') + @table + '_upd';
		declare @column nvarchar(max) = (select column_name from #triggery where table_name = @table);

		--select @table '@table', @schema '@schema', @trigger_name '@trigger_name', isnull(@schema + '.', '')

		declare @trigger_skrypt nvarchar(max) = 
'create or alter trigger ' + @trigger_name + ' on ' + + isnull(@schema + '.', '') + @table + ' for update
as
begin
	if (trigger_nestlevel() < 2)
	begin
		update ' + @table + ' set
			aud_data = getdate(),
			aud_login = suser_name()
		from ' + @table + '
		join inserted on inserted.' + @column + ' = ' + @table + '.' + @column + '
	end
end';		

		exec (@trigger_skrypt);
		delete #triggery where table_name = @table;
		set @i = @i + 1;
	end
end;
go

begin --uzupe³nienie s³owników
	insert into ksiegowanie_typ (kst_nazwa) values
	('obci¹¿enie'), ('sp³ata');
		
	insert into ksiegowanie_konto (ksk_nazwa, ksk_czy_techniczne) values
	('techniczne', 1),
	('wp³ata', 1),
	('kapita³', 0),
	('prowizja', 0),
	('odsetki', 0),
	('umorzenie', 1);
	
	insert into ksiegowanie_konto_subkonto (ksksub_ksk_id, ksksub_nazwa) values
	(2, 'Przelew'),
	(4, 'Prowizja administracyjna'),
	(5, 'Odsetki umowne'),
	(6, 'Zni¿ka/Rabat');
	
	insert into scoring.model (scrm_nazwa, scrm_data_dodania) values
	('Pierwsze sprawdzenie klienta', getdate())

	insert into scoring.pole (scrp_scrm_id, scrp_nazwa, scrp_skrot, scrp_zapytanie, scrp_data_dodania) values
	(1, 'Klient jest zbyt m³ody', 'KL_WIEK', 'select scoring.wiek_klienta(@pozyczka_id);', getdate()),
	(1, 'Niewystarczaj¹cy dochód klienta', 'KL_DOCH', 'select scoring.dochod_klienta(@pozyczka_id);', getdate()),
	(1, 'Klient ma ju¿ otwart¹ po¿yczkê', 'KL_POZ', 'select scoring.otwarte_pozyczki_klienta(@pozyczka_id);', getdate())
end;
go

create database money_loaner_logic;
go

use money_loaner_logic;
go

create schema scoring;
go

begin --tworzenie synonimów
	drop table if exists #synonimy;
	select t.name table_name, iif(s.name = 'dbo', null, s.name) schema_name
	into #synonimy
	from money_loaner_data.sys.tables t
	join money_loaner_data.sys.schemas s on s.schema_id = t.schema_id

	declare @c int = (select count(1) from #synonimy);
	declare @i int = 0;

	while (@i < @c)
	begin
		declare @table nvarchar(max), @schema nvarchar(max);
		select top 1 @table = table_name, @schema = schema_name
		from #synonimy

		declare @synonim_skrypt nvarchar(max) = 'create synonym ' + isnull(@schema + '.', '') + @table + ' for money_loaner_data.' + isnull(@schema, 'dbo') + '.' + @table + ';';
	
		exec (@synonim_skrypt);
	
		delete #synonimy where table_name = @table;
		set @i = @i + 1;
	end
end;
go

create or alter function scoring.wiek_klienta(@pozyczka_id int)
returns bit
as
begin
	declare @result int = 0;

	return @result;
end;
go

create or alter function scoring.dochod_klienta(@pozyczka_id int)
returns bit
as
begin
	declare @result int = 0;

	select @result = iif(pwn_miesieczny_dochod - pwn_miesieczne_wydatki < pszo_rata_od, 1, 0)
	from pozyczka_wniosek
	join pozyczka_szczegoly_oferty on pwn_id = pszo_pwn_id
	where pwn_po_id = @pozyczka_id

	return @result;
end;
go

create or alter function scoring.otwarte_pozyczki_klienta(@pozyczka_id int)
returns bit
as
begin
	declare @result int;
	declare @pk_id int = (select po_pk_id from pozyczka where po_id = @pozyczka_id);

	select top 1 @result = 1
	from pozyczka
	join pozyczka_harmonogram on ph_id = dbo.f_aktualny_harmonogram(po_id)
	join pozyczka_rata on ph_id = por_ph_id
	join ksiegowanie_dekret on por_id = ksd_por_id
	join ksiegowanie on ks_id = ksd_ks_id
	where
		po_pk_id = @pk_id
	group by por_ph_id
	having
		sum(iif(ks_kst_id = 1, ksd_kwota_wn, 0)) -
		sum(iif(ks_kst_id = 2, ksd_kwota_ma, 0)) > 0

	return isnull(@result, 0);
end;
go

create or alter function scoring.f_scoring_wynik (@po_id int)
returns bit
begin
	declare @wynik int;

	select top 1 @wynik = scrprz_czy_dyskwalifikacja
	from scoring.przeliczenie
	where scrprz_po_id = 1
	order by scrprz_czy_dyskwalifikacja desc

	return @wynik;
end
go

create or alter function dbo.f_pobierz_nowy_numer_klienta()
returns nvarchar(max)
as
begin
	declare @nowy_numer int;

	select top 1 @nowy_numer = pk_id
	from pozyczka_klient
	order by pk_data_dodania desc, pk_id desc

	set @nowy_numer = (isnull(@nowy_numer, 0) + 1000) + 1;

	return '200' + cast(@nowy_numer as nvarchar(max));
end;
go

create or alter function dbo.f_pobierz_nowy_numer_pozyczki()
returns nvarchar(max)
as
begin
	declare @nowy_numer int;

	select top 1 @nowy_numer = po_id
	from pozyczka
	order by po_data_dodania desc, po_id desc

	set @nowy_numer = (isnull(@nowy_numer, 0) + 100000) + 1;

	return '30' + cast(@nowy_numer as nvarchar(max));
end;
go

create or alter function f_przerob_na_dekrety (	
	@xml xml
)
returns table
as
return (
	select rata, konto_nazwa, kwota, data_wymagalnosci, konto
	from (
		select
			rata,
			konto_nazwa,
			kwota,
			[data_wymagalnosci],
			case konto_nazwa
				when 'kapital' then 3
				when 'odsetki' then 5
				when 'prowizja' then 4
			end konto
		from (
			select
				y.value('(Number)[1]', 'int') [rata],
				y.value('(Principal)[1]', 'decimal(18, 2)') [kapital],
				y.value('(Interest)[1]', 'decimal(18, 2)') [odsetki],
				y.value('(Fee)[1]', 'decimal(18, 2)') [prowizja],
				y.value('(PaymentDate)[1]', 'date') [data_wymagalnosci]
			from @xml.nodes('//raty/InstallmentDtoList/InstallmentDto') as x(y)
		) x
		unpivot (
			kwota for konto_nazwa in ([kapital], [odsetki], [prowizja])
		) unpvt
		union all
		select y.value('(Number)[1]', 'int'), 'techniczne', y.value('(Total)[1]', 'decimal(18, 2)'), y.value('(PaymentDate)[1]', 'date'), 1
		from @xml.nodes('//raty/InstallmentDtoList/InstallmentDto') as x(y)
	) y
);
go

create or alter function f_aktualna_pozyczka (@pk_id int)
returns int
begin
	declare @aktualna_pozyczka int;

	select top 1 @aktualna_pozyczka = po_id
	from pozyczka
	join pozyczka_harmonogram on ph_id = dbo.f_aktualny_harmonogram(po_id)
	join pozyczka_rata on ph_id = por_ph_id
	join ksiegowanie_dekret on por_id = ksd_por_id
	join ksiegowanie on ks_id = ksd_ks_id
	where
		po_pk_id = @pk_id
	group by po_id, po_data_dodania
	having
		sum(iif(ks_kst_id = 1, ksd_kwota_wn, 0)) -
		sum(iif(ks_kst_id = 2, ksd_kwota_ma, 0)) > 0
	order by po_data_dodania desc, po_id desc

	return @aktualna_pozyczka;
end
go

create or alter function f_aktualny_harmonogram (@po_id int)
returns int
begin
	declare @aktualny_harmonogram int;

	select top 1 @aktualny_harmonogram = ph_id
	from pozyczka_harmonogram
	where ph_po_id = @po_id
	order by cast(ph_data_dodania as date) desc, ph_id desc

	return @aktualny_harmonogram;
end
go

create or alter procedure p_klient_pobierz @pesel varchar(11)
as
begin
	select
		pk_id,
		pk_numer,
		pk_imie,
		pk_nazwisko,
		pk_pesel
	from pozyczka_klient
	where pk_pesel = @pesel
end;
go

create or alter procedure p_klient_email_aktualizuj
	@pk_id int,
	@email nvarchar(max)
as
begin
	declare @aktualny_email nvarchar(max) = (select em_nazwa from email where em_pk_id = @pk_id and em_data_zakonczenia is null);
	declare @now datetime = getdate();

	if (@aktualny_email is null)
	begin
		insert into email (em_pk_id, em_nazwa, em_data_dodania, em_data_zakonczenia)
		values (@pk_id, @email, @now, null);
	end

	if (@email != @aktualny_email)
	begin
		update email
		set em_data_zakonczenia = @now
		where em_pk_id = @pk_id and em_data_zakonczenia is null;

		insert into email (em_pk_id, em_nazwa, em_data_dodania, em_data_zakonczenia)
		values (@pk_id, @email, @now, null);

		update uzytkownik_konto
		set uk_email = @email
		where uk_pk_id = @pk_id;
	end
end;
go

create or alter procedure p_klient_telefon_aktualizuj
	@pk_id int,
	@numer_telefonu varchar(max)
as
begin
	declare @aktualny_numer_telefonu varchar(max) = (select tn_nazwa from telefon where tn_pk_id = @pk_id and tn_data_zakonczenia is null);
	declare @now datetime = getdate();
	set @numer_telefonu = replace(@numer_telefonu, ' ', '');

	if (@aktualny_numer_telefonu is null)
	begin
		insert into telefon (tn_pk_id, tn_nazwa, tn_data_dodania, tn_data_zakonczenia)
		values (@pk_id, @numer_telefonu, @now, null);

		return;
	end

	if (@numer_telefonu != @aktualny_numer_telefonu)
	begin
		update telefon
		set tn_data_zakonczenia = @now
		where tn_pk_id = @pk_id and tn_data_zakonczenia is null;
		
		insert into telefon (tn_pk_id, tn_nazwa, tn_data_dodania, tn_data_zakonczenia)
		values (@pk_id, @numer_telefonu, @now, null);
	end
end;
go

create or alter procedure p_pozyczka_klient_aktualizuj
	@imie nvarchar(max),
	@nazwisko nvarchar(max),
	@pesel varchar(11),
	@email nvarchar(max),
	@numer_telefonu varchar(max),
	@out_id int output
as
begin
	declare @nowy_numer nvarchar(max) = dbo.f_pobierz_nowy_numer_klienta();
	declare @now datetime = getdate();
	declare @pk_id int = (select top 1 pk_id from pozyczka_klient where pk_pesel = @pesel);

	if (isnull(@pk_id, 0) = 0)
	begin
		insert into pozyczka_klient (pk_numer, pk_imie, pk_nazwisko, pk_pesel, pk_data_dodania)
		values (@nowy_numer, @imie, @nazwisko, @pesel, @now);

		set @out_id = scope_identity();

		if (@email is not null)
		begin
			insert into email (em_pk_id, em_nazwa, em_data_dodania, em_data_zakonczenia)
			values (@out_id, @email, @now, null);
		end

		if (@numer_telefonu is not null)
		begin
			insert into telefon (tn_pk_id, tn_nazwa, tn_data_dodania, tn_data_zakonczenia)
			values (@out_id, @numer_telefonu, @now, null);
		end
	end
	else
	begin
		exec p_klient_email_aktualizuj @pk_id, @email;
		exec p_klient_telefon_aktualizuj @pk_id, @numer_telefonu;

		set @out_id = @pk_id;
	end
end;
go

create or alter procedure p_rachunek_bankowy_dodaj
	@numer varchar(26),
	@out_id int output
as
begin
	insert into rachunek_bankowy (rb_numer) values (@numer);

	set @out_id  = scope_identity();
end;
go

create or alter procedure p_klient_rachunek_bankowy_aktualizuj
	@pk_id int,
	@numer_konta varchar(26)
as
begin
	declare @now datetime = getdate();
	declare @aktualny_numer_konta_id int;
	declare @aktualny_numer_konta varchar(26);
	declare @nowy_rachunek_id int;
	
	set @aktualny_numer_konta = replace(@aktualny_numer_konta, ' ', '');
	set @numer_konta = replace(@numer_konta, ' ', '');

	select @aktualny_numer_konta_id = rb_id, @aktualny_numer_konta = rb_numer
	from klient_rachunek_bankowy
	join rachunek_bankowy on rb_id = krb_rb_id
	where
		krb_pk_id = @pk_id and
		krb_data_zakonczenia is null

	if (@aktualny_numer_konta_id is null)
	begin
		exec p_rachunek_bankowy_dodaj @numer_konta, @nowy_rachunek_id out;

		insert into klient_rachunek_bankowy(krb_rb_id, krb_pk_id, krb_data_dodania)
		values (@nowy_rachunek_id, @pk_id, @now);

		return;
	end

	if (@aktualny_numer_konta != @numer_konta)
	begin
		update klient_rachunek_bankowy
		set krb_data_zakonczenia = @now
		where krb_pk_id = @pk_id and krb_data_zakonczenia is null

		exec p_rachunek_bankowy_dodaj @numer_konta, @nowy_rachunek_id out;
		
		insert into klient_rachunek_bankowy(krb_rb_id, krb_pk_id, krb_data_dodania)
		values (@nowy_rachunek_id, @pk_id, @now);
	end
end;
go

create or alter procedure p_pozyczka_dodaj
	@rb_id int,
	@pk_id int,
	@out_id int output
as
begin
	declare @nowy_numer nvarchar(max) = dbo.f_pobierz_nowy_numer_pozyczki();

	insert into pozyczka (po_numer, po_rb_id, po_pk_id, po_data_dodania)
	values (@nowy_numer, @rb_id, @pk_id, getdate());

	set @out_id  = scope_identity();
end;
go

create or alter procedure p_pozyczka_wniosek_dodaj
	@po_id int,
	@imie nvarchar(max),
	@nazwisko nvarchar(max),
	@numer_telefonu nvarchar(max),
	@pesel varchar(11),
	@email nvarchar(max),
	@miesieczny_dochod int,
	@miesieczne_wydatki int,
	@numer_konta varchar(26),
	@out_id int output
as
begin
	declare @now datetime = getdate();

	insert into pozyczka_wniosek (pwn_po_id, pwn_imie, pwn_nazwisko, pwn_numer_telefonu, pwn_pesel, pwn_email, pwn_miesieczny_dochod, pwn_miesieczne_wydatki, pwn_numer_konta, pwn_data_dodania)
	values (@po_id, @imie, @nazwisko, @numer_telefonu, @pesel, @email, @miesieczny_dochod, @miesieczne_wydatki, @numer_konta, @now);

	set @out_id = scope_identity();

	declare @pk_id int = (select po_pk_id from pozyczka where po_id = @po_id);

	exec p_klient_email_aktualizuj @pk_id, @email;
	exec p_klient_telefon_aktualizuj @pk_id, @numer_telefonu;
	exec p_klient_rachunek_bankowy_aktualizuj @pk_id, @numer_konta;
end;
go

create or alter procedure p_pozyczka_szczegoly_oferty_dodaj
	@pwn_id int,
	@rata_od decimal(18, 2),
	@data_pierwszej_raty date,
	@rrso decimal(18, 2),
	@okres_splaty decimal(18, 2),
	@kwota_wnioskowana decimal(18, 2),
	@prowizja decimal(18, 2),
	@odsetki decimal(18, 2),
	@calkowita_kwota_do_zaplaty decimal(18, 2),
	@raty_platne_do int
as
begin
	insert into pozyczka_szczegoly_oferty (
		pszo_pwn_id,
		pszo_rata_od,
		pszo_data_pierwszej_raty,
		pszo_rrso,
		pszo_okres_splaty,
		pszo_kwota_wnioskowana,
		pszo_prowizja,
		pszo_odsetki,
		pszo_calkowita_kwota_do_zaplaty,
		pszo_raty_platne_do
	)
	values (
		@pwn_id,
		@rata_od,
		@data_pierwszej_raty,
		@rrso,
		@okres_splaty,
		@kwota_wnioskowana,
		@prowizja,
		@odsetki,
		@calkowita_kwota_do_zaplaty,
		@raty_platne_do
	);
end;
go

create or alter procedure p_uzytkownik_konto_dodaj
	@imie nvarchar(max),
	@nazwisko nvarchar(max),
	@pesel varchar(11),
	@email nvarchar(max),
	@haslo nvarchar(max)
as
begin
	declare @pk_id int, @uk_id int, @em_id int, @em_pk_id int;
	
	select top 1 @pk_id = pk_id, @uk_id = @uk_id
	from pozyczka_klient
	left join uzytkownik_konto on pk_id = uk_pk_id
	where pk_pesel = @pesel
	
	if (@pk_id is not null and @uk_id is not null)
	begin
		raiserror('Masz ju¿ za³o¿one swoje konto', 16, 1);
		return;
	end

	select top 1 @em_id = em_id, @em_pk_id = em_pk_id
	from email
	where em_nazwa = @email and em_data_zakonczenia is null

	if (@em_id is not null and @em_pk_id != @pk_id)
	begin
		raiserror('Podany adres email jest zajêty', 16, 1);
		return;
	end

	declare @pk_id_out int;
	exec p_pozyczka_klient_aktualizuj @imie, @nazwisko, @pesel, @email, null, @pk_id_out out;

	insert into uzytkownik_konto (uk_email, uk_haslo, uk_data_dodania, uk_czy_aktywne, uk_pk_id)
	values (@email, @haslo, getdate(), 1, @pk_id_out);
end;
go

create or alter procedure p_uzytkownik_konto_zmien_haslo
	@pk_id int,
	@haslo nvarchar(max)
as
begin
	update uzytkownik_konto
	set uk_haslo = @haslo
	where uk_pk_id = @pk_id;
end;
go

create or alter procedure p_uzytkownik_konto_pobierz
	@email nvarchar(max),
	@pk_id int,
	@pesel varchar(11)
as
begin
	select top 1 uk_id, uk_email, uk_haslo, uk_data_dodania, uk_czy_aktywne, uk_pk_id
	from uzytkownik_konto
	join pozyczka_klient on pk_id = uk_pk_id
	where uk_email = @email or uk_pk_id = @pk_id or pk_pesel = @pesel;
end;
go

create or alter procedure p_uzytkownik_konto_zmien_haslo
	@pk_id int,
	@haslo nvarchar(max)
as
begin
	update uzytkownik_konto
	set uk_haslo = @haslo
	where uk_pk_id = @pk_id;
end;
go

create or alter procedure p_dodaj_harmonogram @po_id int, @xml xml
as
begin
	declare @now datetime = getdate();
	declare @ph_id int;
	declare @raty table (por_id int, rata int);
	declare @ksiegowania table (ks_id int, por_id int);
	declare @StartDate date;
	declare @LastInstallmentDate date;

	select
		@StartDate = y.value('(StartDate)[1]', 'date'),
		@LastInstallmentDate = y.value('(LastInstallmentDate)[1]', 'date')
	from @xml.nodes('//raty') as x(y)

	drop table if exists #harm;
	select rata, konto_nazwa, kwota, data_wymagalnosci, konto
	into #harm
	from dbo.f_przerob_na_dekrety(cast(@xml as xml))
	order by rata, konto

	insert into pozyczka_harmonogram (ph_po_id, ph_nazwa, ph_data_dodania, ph_data_rozpoczecia, ph_data_zakonczenia)
	select @po_id, 'Harmonogram pocz¹tkowy', @now, @StartDate, @LastInstallmentDate;

	set @ph_id = scope_identity();

	merge pozyczka_rata as t
	using (
		select rata, data_wymagalnosci
		from #harm
		where konto = 1
	) as s
	on 1 = 0
	when not matched then
		insert (por_ph_id, por_numer, por_data_wymagalnosci)
		values (@ph_id, rata, data_wymagalnosci)
		output inserted.por_id, s.rata
		into @raty (por_id, rata)
	;

	merge ksiegowanie as t
	using (
		select por_id
		from @raty
	) as s
	on 1 = 0
	when not matched then
		insert (ks_por_id, ks_data_dodania, ks_data_operacji, ks_kst_id)
		values (por_id, @now, @now, 1)
		output inserted.ks_id, s.por_id
		into @ksiegowania (ks_id, por_id)
	;

	insert into ksiegowanie_dekret (ksd_ks_id, ksd_por_id, ksd_ksk_id, ksd_ksksub_id, ksd_kwota_wn, ksd_kwota_ma)
	select ks_id, raty.por_id, konto, ksksub_id, iif(konto != 1, kwota, 0), iif(konto = 1, kwota, 0)
	from #harm harm
	join @raty raty on harm.rata = raty.rata
	join @ksiegowania ks on ks.por_id = raty.por_id
	left join ksiegowanie_konto_subkonto on ksksub_ksk_id = harm.konto
	order by harm.rata, konto
end;
go

create or alter procedure p_pobierz_harmonogram @ph_id int
as
begin
	select
		por_id															PorId,
		por_numer														Number,
		cast(por_data_wymagalnosci as date)								PaymentDate,
		sum(iif(ks_kst_id = 1, ksd_kwota_wn, 0))						Debt,
		sum(iif(ks_kst_id = 2, ksd_kwota_ma, 0))						Repayment,
		sum(iif(ks_kst_id = 1, ksd_kwota_wn, 0)) -
		sum(iif(ks_kst_id = 2, ksd_kwota_ma, 0))						Balance,
		sum(iif(ks_kst_id = 1 and ksd_ksk_id = 3, ksd_kwota_wn, 0)) -
		sum(iif(ks_kst_id = 2 and ksd_ksk_id = 3, ksd_kwota_ma, 0))		Principal,
		sum(iif(ks_kst_id = 1 and ksd_ksk_id = 4, ksd_kwota_wn, 0)) -
		sum(iif(ks_kst_id = 2 and ksd_ksk_id = 4, ksd_kwota_ma, 0))		AdmissionFee,
		sum(iif(ks_kst_id = 1 and ksd_ksk_id = 5, ksd_kwota_wn, 0)) -
		sum(iif(ks_kst_id = 2 and ksd_ksk_id = 5, ksd_kwota_ma, 0))		ContractualInterest
	from pozyczka_rata
	join ksiegowanie_dekret on por_id = ksd_por_id
	join ksiegowanie on ks_id = ksd_ks_id
	where por_ph_id = @ph_id
	group by por_id, por_numer, por_data_wymagalnosci
	order by PaymentDate
end;
go

create or alter procedure p_konto_informacje_pobierz @pk_id int
as
begin
	declare @aktualna_pozyczka int = dbo.f_aktualna_pozyczka(@pk_id);
	declare @czy_zdyskwalifikowana_pozyczka bit = (select scoring.f_scoring_wynik(1));

	select
		pk_numer [ClientNumber],
		pk_imie [Name],
		pk_nazwisko [Surname],
		pk_pesel [PersonalNumber],
		pk_data_dodania [AccountCreateDate],
		isnull(po_id, -1) [LoanId],
		isnull(po_numer, '-') [LoanNumber],
		isnull(rb_numer, '-') [CCNumberToRepayment],
		em_nazwa [Email],
		tn_nazwa [Phone]
	from pozyczka_klient
	left join email on pk_id = em_pk_id and em_data_zakonczenia is null
	left join telefon on pk_id = tn_pk_id and tn_data_zakonczenia is null
	left join (
		select po_pk_id, po_id, po_numer, rb_numer
		from pozyczka
		join rachunek_bankowy on rb_id = po_rb_id
		where po_id = @aktualna_pozyczka and @czy_zdyskwalifikowana_pozyczka = 0
	) x on pk_id = po_pk_id
	where
		pk_id = @pk_id
end;
go

create or alter procedure p_scoring_wylicz
	@po_id int,
	@out_is_disqualification int output
as
begin
	declare @now datetime = getdate();

	if not exists (
		select top 1 1
		from pozyczka
		where po_id = @po_id
	)
	begin
		declare @error nvarchar(max) = concat('Brak po¿yczki z id [', cast(@po_id as varchar(max)), '] w systemie');
		raiserror(@error, 16, 1);
		return;
	end

	drop table if exists #modele;
	select scrm_id as t_scrm_id
	into #modele
	from scoring.model
	where scrm_czy_aktywne = 1

	drop table if exists #pola;
	select scrp_id, t_scrm_id, scrp_zapytanie, row_number() over (order by (select 0)) LP
	into #pola
	from #modele
	join scoring.pole on t_scrm_id = scrp_scrm_id
	where scrp_czy_aktywne = 1

	declare @przeliczenia table (lp int, scrp_id int, scrm_id int, start datetime, koniec datetime, wynik bit, blad nvarchar(max));
	declare @wynik table (wynik int);

	declare @c int = (select count(1) from #pola);
	declare @i int = 0;

	while (@i < @c)
	begin
		declare @start datetime = getdate();
		declare @aktualne_lp int, @scrm_id int, @scrprz_id int, @scrp_id int, @zapytanie nvarchar(max);

		select top 1 @aktualne_lp = lp, @scrm_id = t_scrm_id, @scrp_id = scrp_id, @zapytanie = replace(scrp_zapytanie, '@pozyczka_id', cast(@po_id as varchar(max)))
		from #pola
	
		begin try
			insert into @wynik
			exec (@zapytanie);

			insert into @przeliczenia (lp, scrp_id, scrm_id, start, koniec, wynik)
			select @aktualne_lp, @scrp_id, @scrm_id, @start, getdate(), wynik
			from @wynik
		end try
		begin catch
			insert into @przeliczenia (lp, scrp_id, scrm_id, start, koniec, wynik, blad)
			values (@aktualne_lp, @scrp_id, @scrm_id, @start, getdate(), 0, error_message())
		end catch

		delete #pola where lp = @aktualne_lp;
		delete @wynik;
		set @i = @i + 1;
	end

	declare @dodane_przeliczenia table (t_scrprz_id int, t_scrm_id int);

	merge scoring.przeliczenie as t
	using (
		select scrm_id, min(start) start, max(koniec) koniec, max(0 + wynik) wynik
		from @przeliczenia
		group by scrm_id
	) as s
	on 1 = 0
	when not matched then
		insert (scrprz_data_dodania, scrprz_data_zakonczenia, scrprz_po_id, scrprz_scrm_id, scrprz_czy_dyskwalifikacja)
		values (s.start, s.koniec, @po_id, s.scrm_id, s.wynik)
		output inserted.scrprz_id, s.scrm_id
		into @dodane_przeliczenia (t_scrprz_id, t_scrm_id)
	;

	insert into scoring.wartosc (scrw_scrp_id, scrw_scrprz_id, scrw_wartosc, scrw_start, scrw_koniec, scrw_blad)
	select t2.scrp_id, t1.t_scrprz_id, t2.wynik, t2.start, t2.koniec, t2.blad
	from @dodane_przeliczenia t1
	join @przeliczenia t2 on t1.t_scrm_id = t2.scrm_id

	select @out_is_disqualification = max(0 + wynik)
	from @przeliczenia
end;
go

create or alter procedure p_konto_historia_pozyczek @pk_id int
as
begin
	select
		pwn_id ProposalId
		,pwn_data_dodania DateOfProposal
		,iif(scrprz_czy_dyskwalifikacja = 1, 'Wniosek odrzucony', iif(saldo_aktualne > 0, 'Po¿yczka wyp³acona', 'Po¿yczka sp³acona')) ProposalStatus
		,isnull(x.po_numer, '') LoanNumber
		,isnull(saldo_aktualne, 0) CurrentBalance
	from pozyczka_wniosek
	join pozyczka on po_id = pwn_po_id
	join scoring.przeliczenie on po_id = scrprz_po_id
	left join (
		select po_id, po_numer, sum(iif(ks_kst_id = 1, ksd_kwota_wn, 0)) - sum(iif(ks_kst_id = 2, ksd_kwota_ma, 0)) saldo_aktualne
		from pozyczka
		join pozyczka_harmonogram on po_id = ph_po_id
		join pozyczka_rata on ph_id = por_ph_id
		join ksiegowanie_dekret on por_id = ksd_por_id
		join ksiegowanie on ks_id = ksd_ks_id
		where po_pk_id = @pk_id and ph_id = dbo.f_aktualny_harmonogram(po_id)
		group by po_id, po_numer
	) x on x.po_id = pwn_po_id
	where
		po_pk_id = @pk_id
end;
go