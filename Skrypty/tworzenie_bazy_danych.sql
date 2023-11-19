use master;

begin
	--https://stackoverflow.com/a/7469167
	declare @sql_drop_all_connections varchar(max);

	select @sql_drop_all_connections = coalesce(@sql_drop_all_connections,'') + 'kill ' + convert(varchar, spid) + ';'
	from master..sysprocesses
	where dbid in (db_id('money_loaner_data'), db_id('money_loaner_logic')) AND spid <> @@spid

	exec (@sql_drop_all_connections);

	declare @exec nvarchar(max);
	if db_id('money_loaner_data') is not null
	begin
		set @exec = concat('drop database ', 'money_loaner_data');
		exec (@exec);
	end

	if db_id('money_loaner_logic') is not null
	begin
		set @exec = concat('drop database ', 'money_loaner_logic');
		exec (@exec);
	end
end

create database money_loaner_data;
go

use money_loaner_data;
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

create table user_account (
	us_id int primary key identity(1, 1),
	us_email nvarchar(max) not null,
	us_haslo nvarchar(max) not null,
	us_data_dodania datetime not null,
	us_czy_aktywne bit not null,
	us_pk_id int not null foreign key references pozyczka_klient (pk_id),
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

create table pozyczka_dlug (
	pd_id int primary key identity(1, 1),
	pd_numer nvarchar(max) not null,
	pd_rb_id int not null foreign key references rachunek_bankowy (rb_id),
	pd_pk_id int not null foreign key references pozyczka_klient (pk_id),
	pd_data_dodania datetime not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

create table pozyczka_wniosek (
	pwn_id int primary key identity(1, 1),
	pwn_pd_id int not null foreign key references pozyczka_dlug (pd_id),
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
	pszo_okres_splaty decimal(18, 2) not null,
	pszo_kwota_wnioskowana decimal(18, 2) not null,
	pszo_prowizja decimal(18, 2) not null,
	pszo_odsetki decimal(18, 2) not null,
	pszo_calkowita_kwota_do_zaplaty decimal(18, 2) not null,
	pszo_raty_platne_do int not null,
	aud_data datetime default getdate(),
	aud_login nvarchar(max) default suser_name()
);
go

begin --tworzenie triggerów
	drop table if exists #triggery;
	select c.object_id, c.name column_name, t.name table_name
	into #triggery
	from sys.tables t
	join sys.columns c on t.object_id = c.object_id
	where c.is_identity = 1

	declare @c int = (select count(1) from #triggery);
	declare @i int = 0;

	while (@i < @c)
	begin
		declare @table nvarchar(max) = (select top 1 table_name from #triggery);
		declare @column nvarchar(max) = (select column_name from #triggery where table_name = @table);
		declare @trigger_skrypt nvarchar(max) = 
'create or alter trigger tr_' + @table + '_upd on ' + @table + ' for update
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
end'
		exec (@trigger_skrypt);
		delete #triggery where table_name = @table;
		set @i = @i + 1;
	end
end
go

create database money_loaner_logic;
go

use money_loaner_logic;

begin --tworzenie synonimów
	drop table if exists #synonimy;
	select t.name table_name
	into #synonimy
	from money_loaner_data. sys.tables t

	declare @c int = (select count(1) from #synonimy);
	declare @i int = 0;

	while (@i < @c)
	begin
		declare @table nvarchar(max) = (select top 1 table_name from #synonimy);
		declare @synonim_skrypt nvarchar(max) = 'create synonym ' + @table + ' for money_loaner_data.dbo.' + @table + ';';
	
		exec (@synonim_skrypt);
	
		delete #synonimy where table_name = @table;
		set @i = @i + 1;
	end
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

create or alter function dbo.f_pobierz_nowy_numer_klienta()
returns nvarchar(max)
as
begin
	declare @nowy_numer int;

	select top 1 @nowy_numer = pk_id
	from pozyczka_klient
	order by pk_data_dodania desc, pk_id desc

	set @nowy_numer = (isnull(@nowy_numer, 0) + 100000) + 1;

	return '00' + cast(@nowy_numer as nvarchar(max));
end
go

create or alter procedure p_klient_dodaj
	@imie nvarchar(max),
	@nazwisko nvarchar(max),
	@pesel varchar(11),
	@out_id int output
as
begin
	declare @nowy_numer nvarchar(max) = dbo.f_pobierz_nowy_numer_klienta();

	insert into pozyczka_klient (pk_numer, pk_imie, pk_nazwisko, pk_pesel, pk_data_dodania)
	values (@nowy_numer, @imie, @nazwisko, @pesel, getdate());

	set @out_id = scope_identity();
end;
go

create or alter procedure p_rachunek_bankowy_dodaj
	@numer varchar(26),
	@out_id int output
as
begin
	insert into rachunek_bankowy (rb_numer) values (@numer);

	set @out_id  = scope_identity();
end
go

create or alter function dbo.f_pobierz_nowy_numer_dlugu()
returns nvarchar(max)
as
begin
	declare @nowy_numer int;

	select top 1 @nowy_numer = pd_id
	from pozyczka_dlug
	order by pd_data_dodania desc, pd_id desc

	set @nowy_numer = (isnull(@nowy_numer, 0) + 100000) + 1;

	return '50' + cast(@nowy_numer as nvarchar(max));
end
go

create or alter procedure p_pozyczka_dlug_dodaj
	@rb_id int,
	@pk_id int,
	@out_id int output
as
begin
	declare @nowy_numer nvarchar(max) = dbo.f_pobierz_nowy_numer_dlugu();

	insert into pozyczka_dlug (pd_numer, pd_rb_id, pd_pk_id, pd_data_dodania)
	values (@nowy_numer, @rb_id, @pk_id, getdate());

	set @out_id  = scope_identity();
end
go

create or alter procedure p_pozyczka_wniosek_dodaj
	@pd_id int,
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

	insert into pozyczka_wniosek (pwn_pd_id, pwn_imie, pwn_nazwisko, pwn_numer_telefonu, pwn_pesel, pwn_email, pwn_miesieczny_dochod, pwn_miesieczne_wydatki, pwn_numer_konta, pwn_data_dodania)
	values (@pd_id, @imie, @nazwisko, @numer_telefonu, @pesel, @email, @miesieczny_dochod, @miesieczne_wydatki, @numer_konta, @now);

	set @out_id  = scope_identity();

	declare @pk_id int = (select pd_pk_id from pozyczka_dlug where pd_Id = @pd_id);

	insert into email (em_pk_id, em_nazwa, em_data_dodania, em_data_zakonczenia)
	values (@pk_id, @email, @now, null);

	insert into telefon (tn_pk_id, tn_nazwa, tn_data_dodania, tn_data_zakonczenia)
	values (@pk_id, @numer_telefonu, @now, null);

	declare @rb_id int;
	set @numer_konta = replace(@numer_konta, ' ', '');
	exec p_rachunek_bankowy_dodaj @numer_konta, @rb_id out;

	insert into klient_rachunek_bankowy (krb_rb_id, krb_pk_id, krb_data_dodania, krb_data_zakonczenia)
	values (@rb_id, @pk_id, @now, null);
end
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
	insert into pozyczka_szczegoly_oferty (pszo_pwn_id, pszo_rata_od, pszo_data_pierwszej_raty, pszo_rrso, pszo_okres_splaty, pszo_kwota_wnioskowana, pszo_prowizja, pszo_odsetki, pszo_calkowita_kwota_do_zaplaty, pszo_raty_platne_do)
	values (@pwn_id, @rata_od, @data_pierwszej_raty, @rrso, @okres_splaty, @kwota_wnioskowana, @prowizja, @odsetki, @calkowita_kwota_do_zaplaty, @raty_platne_do);
end
go