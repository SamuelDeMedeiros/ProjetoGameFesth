-- drop database bdGameFesth;
/*
CREATE USER 'luis'@'localhost' IDENTIFIED BY '12345678';
grant all privileges on bdgamefesth.* To 'luis'@'localhost';
flush privileges;
*/
CREATE DATABASE bdGameFesth;
USE bdGameFesth;

CREATE TABLE tb_produto( 
    CodBarras bigint not null primary key, 
    Nome varchar(100) not null,
    Marca varchar(100) not null,
    Descricao varchar(400) not null,
    ImagemProduto varchar(255) not null,
    Valor decimal (8,2) check (Valor > 0) not null, 
    QtdEst smallint check (QtdEst >= 0) not null
);
/*
CREATE TABLE tb_produtoFav(
	CodBarrasFav bigint not null,
	Id_Cli int not null, 
    foreign key (Id_Cli) references Tb_Cliente(Id_cliente), 
    foreign key (CodBarrasFav) references tb_produto(CodBarras)
);
*/
CREATE TABLE tb_login(
	id_Login int primary key auto_increment,
    email varchar(200) not null unique,
    senha  varchar(200) not null,
    tipo_login char(1) not null
);

CREATE TABLE Tb_Cliente( 
    Id_cliente int primary key auto_increment, 
    Nome varchar(200) not null, 
    Nascimento date not null, 
    Sexo char(1) not null, 
    Telefone varchar(14) not null,
    Situacao char(1) not null,
    id_log int not null,
    foreign key (id_log) references tb_login(id_Login) 
);

CREATE TABLE Tb_Vendas( 
    Nf int not null primary key, 
    ValorTotal decimal (8,2) not null, 
    Data_ DateTime not null, 
    Id_cli int not null, 
    foreign key (Id_cli) references Tb_Cliente(Id_cliente) 
);

CREATE TABLE Tb_ItemVenda( 
    Nf int not null, 
    CodBarras bigint not null, 
    Valor Decimal (8,2) check (Valor > 0) not null, 
    Qtd smallint check (Qtd > 0) not null, 
    foreign key (Nf) references Tb_Vendas(Nf), 
    foreign key (CodBarras) references Tb_Produto(CodBarras), 
    primary key(Nf, CodBarras) 
);

CREATE TABLE Tb_PF( 
    RG int not null, 
    Cpf bigint not null primary key unique, 
    Id_Cli int not null, 
    foreign key (Id_Cli) references tb_cliente(Id_cliente) 
);

CREATE TABLE Tb_PJ( 
    Cnpj bigint primary key not null, 
    IE varchar(200) not null, 
    NomeFantasia varchar (200) not null, 
    RazaoSocial varchar (200) not null, 
    Id_Cli int not null, 
    foreign key (Id_Cli) references Tb_Cliente(Id_cliente) 
);

CREATE TABLE Tb_Estado( 
    IdUf int primary key auto_increment, 
    NomeUf char(2) unique not null 
);

INSERT INTO Tb_Estado (NomeUf) VALUES ('AC');
INSERT INTO Tb_Estado (NomeUf) VALUES ('AL');
INSERT INTO Tb_Estado (NomeUf) VALUES ('AP');
INSERT INTO Tb_Estado (NomeUf) VALUES ('AM');
INSERT INTO Tb_Estado (NomeUf) VALUES ('BA');
INSERT INTO Tb_Estado (NomeUf) VALUES ('CE');
INSERT INTO Tb_Estado (NomeUf) VALUES ('DF');
INSERT INTO Tb_Estado (NomeUf) VALUES ('ES');
INSERT INTO Tb_Estado (NomeUf) VALUES ('GO');
INSERT INTO Tb_Estado (NomeUf) VALUES ('MA');
INSERT INTO Tb_Estado (NomeUf) VALUES ('MT');
INSERT INTO Tb_Estado (NomeUf) VALUES ('MS');
INSERT INTO Tb_Estado (NomeUf) VALUES ('MG');
INSERT INTO Tb_Estado (NomeUf) VALUES ('PA');
INSERT INTO Tb_Estado (NomeUf) VALUES ('PB');
INSERT INTO Tb_Estado (NomeUf) VALUES ('PR');
INSERT INTO Tb_Estado (NomeUf) VALUES ('PE');
INSERT INTO Tb_Estado (NomeUf) VALUES ('PI');
INSERT INTO Tb_Estado (NomeUf) VALUES ('RJ');
INSERT INTO Tb_Estado (NomeUf) VALUES ('RN');
INSERT INTO Tb_Estado (NomeUf) VALUES ('RS');
INSERT INTO Tb_Estado (NomeUf) VALUES ('RO');
INSERT INTO Tb_Estado (NomeUf) VALUES ('RR');
INSERT INTO Tb_Estado (NomeUf) VALUES ('SC');
INSERT INTO Tb_Estado (NomeUf) VALUES ('SP');
INSERT INTO Tb_Estado (NomeUf) VALUES ('SE');
INSERT INTO Tb_Estado (NomeUf) VALUES ('TO');

CREATE TABLE Tb_Cidade( 
    IdCid int primary key auto_increment, 
    NomeCid varchar(200) unique not null 
);

CREATE TABLE Tb_Endereco( 
	CEP numeric(8) primary key not null, 
    Logradouro varchar(50) not null, 
    IdUf int, 
    IdCid int, 
    foreign key (IdUf) references Tb_Estado(IdUf), 
    foreign key (IdCid) references Tb_Cidade(IdCid) 
);

CREATE TABLE Tb_EndCliente( 
    CEP numeric(8) not null, 
    IdCli int not null,
    num int not null,
    foreign key (Cep) references Tb_Endereco(Cep), 
    foreign key (IdCli) references Tb_Cliente(Id_cliente), 
    primary key(Cep, IdCli) 
);

CREATE TABLE Tb_Funcionario( 
    Id_func int primary key auto_increment, 
    Nome_Func varchar(200) not null, 
    DataAdmissao Date not null, 
    Tel bigint not null, 
    Tipo char(1) not null, 
    num int not null,
    Cep_Func numeric(8) not null,
    Situacao char(1) not null,
	id_log int not null,
    foreign key (id_log) references tb_login(id_Login),
    foreign key (Cep_Func) references Tb_Endereco(CEP) 
);
-- ALTER TABLE Tb_Funcionario ADD num int;
-- ALTER TABLE tb_endcliente DROP FOREIGN KEY tb_endcliente_ibfk_1;
-- ALTER TABLE tb_endcliente ADD FOREIGN KEY (cep) references tb_Endereco(CEP);
-- DROP procedure spInsereProduto;
-- Views
-- view funcionario
create view vw_FuncEnd as
select Id_func, id_log, Nome_Func, DataAdmissao, Tel, Tipo,Situacao, Num, TF.Cep_Func, Logradouro, NomeUf, NomeCid, email, senha
from TB_FUNCIONARIO TF
join Tb_Endereco TE on TF.Cep_Func = TE.CEP
join Tb_Estado TES on TE.idUf = TES.idUF
join Tb_cidade TC on TE.idCid = TC.IdCid
join tb_login TL on TF.id_log = TL.id_login;

-- view cliente
create view vw_ClienteEnd as
select Id_cliente, id_log, Nome, Nascimento, Sexo,Situacao, Telefone, Num, TEC.CEP, Logradouro, NomeUf, NomeCid, email, senha
from tb_cliente TCL
join TB_endCLiente TEC on TCL.id_cliente = TEC.idcli
join Tb_Endereco TE on TEC.Cep = TE.CEP
join Tb_Estado TES on TE.idUf = TES.idUF
join Tb_cidade TC on TE.idCid = TC.IdCid
join tb_login TL on TCL.id_log = TL.id_login;

-- view pj e  pf
CREATE OR REPLACE VIEW vw_cliCad AS
SELECT 
    tc.Id_cliente AS Codigo,
    tc.Nome AS Nome,
    tec.num AS numCli,
    te.CEP AS Cep,
    te.Logradouro AS Logradouro,
    tcid.NomeCid AS Cidade,
    tes.NomeUf AS UF,
    CONCAT_WS(' / ', COALESCE(tpf.CPF, tpj.CNPJ)) AS CPF_CNPJ,
    CONCAT_WS(' / ', COALESCE(tpf.RG, ''), COALESCE(tpj.IE, '')) AS RG_IE
FROM 
    Tb_Cliente tc
JOIN 
    Tb_EndCliente tec ON tc.Id_cliente = tec.IdCli
JOIN 
    Tb_Endereco te ON tec.CEP = te.CEP
JOIN 
    Tb_Cidade tcid ON te.IdCid = tcid.IdCid
JOIN 
    Tb_Estado tes ON te.IdUf = tes.IdUf
LEFT JOIN 
    Tb_PF tpf ON tc.Id_cliente = tpf.Id_Cli
LEFT JOIN 
    Tb_PJ tpj ON tc.Id_cliente = tpj.Id_Cli;
  
  -- detalhes da venda
    CREATE OR REPLACE VIEW vw_detaVenda AS
SELECT 
    tv.NF AS NotaFiscal,
    tv.ValorTotal AS Total,
    tv.Data_ AS datacao,
    tv.Id_cli AS ClienteiD,
    tp.Nome AS Produto,
    tp.Marca AS Marca,
    tp.Descricao AS Descricao,
    tp.ImagemProduto AS ImagemProduto,
    tp.Valor AS Valor,
    tp.QtdEst AS QtdEst
		FROM 
    tb_vendas tv
    JOIN 
    tb_itemvenda tiv ON tiv.NF = tv.NF
	JOIN 
    tb_produto tp ON tiv.CodBarras = tp.CodBarras;
  
  
-- Insere produto
DELIMITER $$
CREATE PROCEDURE spInsereProduto (
 IN vCodBarras BIGINT,
 IN  vNome VARCHAR(100),
 IN  vMarca VARCHAR(100),
 IN vImagemProduto VARCHAR(255), 
 IN  vDescricao varchar(400),
 IN vValor DECIMAL(8,2), 
 IN vQtdEst smallint)
BEGIN
	INSERT INTO Tb_Produto (CodBarras, Nome, Marca, ImagemProduto, Descricao, Valor, QtdEst) VALUES (vCodBarras, vNome, vMarca, vImagemProduto, vDescricao, vValor, vQtdEst);
END $$
DELIMITER ;

-- Teclado
CALL spInsereProduto(111232333446,'teclado logitech g-pro X', 'logitech', '/Imagens/logitechgpro.jpeg','Teclado Mecânico Gamer sem fio USB Logitech G PRO X TKL LIGHTSPEED Preto com Layout Americano.', 1199.90, 50);
CALL spInsereProduto(111232333447,'teclado logitech g915', 'logitech', '/Imagens/logitechg915.jpg','Um avanço em design e engenharia. LIGHTSPEED sem fio de nível profissional, LIGHTSYNC RGB avançado e interruptores mecânicos de baixo perfil e alto desempenho a sua escolha. Totalmente equipado com teclas G programáveis.', 943.50, 35);
CALL spInsereProduto(111232333448,'teclado huntsman 60%', 'Razer', '/Imagens/razerhuntsman.jpg','Domine em uma escala diferente com o Razer Huntsman Mini — um teclado de 60% para jogos com interruptores ópticos Razer de ponta. Altamente portátil e ideal para configurações simplificadas, é hora de experimentar uma atuação extremamente rápida em nosso fator de forma mais compacto até agora.', 700, 70);
CALL spInsereProduto(111232333449,'teclado corsair k-100', 'Corsair', '/Imagens/corsair-k100.jpeg','O incomparável teclado mecânico gamer CORSAIR K100 RGB combina um lindo design em alumínio, iluminação RGB por tecla, a poderosa tecnologia de hiperprocessamento CORSAIR AXON e os mecanismos de tecla CHERRY MX SPEED.', 1700, 15);
CALL spInsereProduto(111232333462, 'Teclado HyperX Alloy FPS Pro', 'HyperX', '/Imagens/alloyfpspro.jpg', 'Teclado mecânico gamer HyperX Alloy FPS Pro com switches Cherry MX, layout compacto TKL, iluminação vermelha e estrutura em aço.', 449.90, 30);
CALL spInsereProduto(111232333463, 'Teclado Ducky One 2 Mini', 'Ducky', '/Imagens/duckyone2mini.jpeg', 'Teclado mecânico gamer Ducky One 2 Mini com switches Cherry MX, layout compacto 60%, teclas PBT Double-Shot e iluminação RGB configurável.', 799.90, 20);
CALL spInsereProduto(111232333464, 'Teclado Corsair K70 RGB MK.2', 'Corsair', '/Imagens/k70rgbmk2.jpg', 'Teclado mecânico gamer Corsair K70 RGB MK.2 com switches Cherry MX, teclas programáveis, iluminação RGB por tecla e estrutura em alumínio.', 999.90, 15);
CALL spInsereProduto(111232333465, 'Teclado Razer BlackWidow Elite', 'Razer', '/Imagens/blackwidowelite.jpg', 'Teclado mecânico gamer Razer BlackWidow Elite com switches Razer Green, teclas macro programáveis, apoio de pulso magnético e iluminação RGB.', 799.90, 25);
CALL spInsereProduto(111232333468, 'Teclado Apple Magic Keyboard', 'Apple', '/Imagens/magickeyboard.jpeg', 'Teclado sem fio Apple Magic Keyboard com design minimalista, teclas de perfil baixo, bateria recarregável e conexão Bluetooth.', 999.90, 13);
-- Mouse
CALL spInsereProduto(111232333472, 'Mouse Logitech MX Master 3', 'Logitech', '/Imagens/mxmaster3.jpg', 'Mouse sem fio Logitech MX Master 3 com sensor Darkfield, roda de rolagem MagSpeed, conectividade Bluetooth e recarregamento USB-C.', 699.90, 20);
CALL spInsereProduto(111232333473, 'Mouse Razer Basilisk Ultimate', 'Razer', '/Imagens/basiliskultimate.jpg', 'Mouse gamer sem fio Razer Basilisk Ultimate com sensor óptico Focus+ 20K, switches ópticos Razer, roda de rolagem ajustável e iluminação RGB Chroma.', 999.90, 15);
CALL spInsereProduto(111232333474, 'Mouse Corsair Ironclaw RGB Wireless', 'Corsair', '/Imagens/ironclaw.jpg', 'Mouse gamer sem fio Corsair Ironclaw RGB Wireless com sensor óptico Pixart PMW3391, 10 botões programáveis, pegada palm grip e iluminação RGB.', 599.90, 25);
CALL spInsereProduto(111232333475, 'Mouse SteelSeries Rival 650 Wireless', 'SteelSeries', '/Imagens/rival650.jpeg', 'Mouse gamer sem fio SteelSeries Rival 650 Wireless com sensor TrueMove3+ 12.000 CPI, 7 botões programáveis, peso ajustável e tecnologia de carregamento rápido.', 799.90, 20);
CALL spInsereProduto(111232333478, 'Mouse Microsoft Pro IntelliMouse', 'Microsoft', '/Imagens/prointelli.jpeg', 'Mouse com fio Microsoft Pro IntelliMouse com sensor PixArt PMW3389, 5 botões, roda de rolagem metálica e design clássico.', 299.90, 25);
CALL spInsereProduto(111232333479, 'Mouse Asus ROG Gladius II', 'Asus', '/Imagens/asusroggladiusii.jpg', 'Mouse gamer com fio Asus ROG Gladius II com sensor óptico 12.000 DPI, switches Omron, 6 botões programáveis e iluminação Aura Sync.', 399.90, 20);
CALL spInsereProduto(111232333480, 'Mouse Gigabyte AORUS M5', 'Gigabyte', '/Imagens/aorusm5.jpeg', 'Mouse gamer com fio Gigabyte AORUS M5 com sensor óptico Pixart 3389, switches Omron, 16.000 DPI e iluminação RGB Fusion.', 349.90, 20);
CALL spInsereProduto(111232333481, 'Mouse Lenovo Legion M500 RGB', 'Lenovo', '/Imagens/legionm500.jpeg', 'Mouse gamer com fio Lenovo Legion M500 RGB com sensor óptico 16.000 DPI, switches Omron, 7 botões programáveis e iluminação RGB personalizável.', 249.90, 30);
-- Headset
CALL spInsereProduto(111232333482, 'Headset Logitech G Pro X Wireless', 'Logitech', '/Imagens/gproxwireless.jpeg', 'Headset gamer sem fio Logitech G Pro X Wireless com drivers Pro-G de 50 mm, microfone removível com Blue VO!CE e tecnologia Lightspeed.', 1299.90, 15);
CALL spInsereProduto(111232333483, 'Headset Razer BlackShark V2 Pro', 'Razer', '/Imagens/blacksharkv2pro.jpeg', 'Headset gamer sem fio Razer BlackShark V2 Pro com drivers Razer Triforce Titanium de 50 mm, microfone removível com cancelamento de ruído e tecnologia HyperSpeed.', 1199.90, 20);
CALL spInsereProduto(111232333484, 'Headset HyperX Cloud Flight S', 'HyperX', '/Imagens/cloudflights.jpeg', 'Headset gamer sem fio HyperX Cloud Flight S com drivers de 50 mm, microfone removível com cancelamento de ruído, controles de áudio no próprio headset e bateria com até 30 horas de duração.', 899.90, 25);
CALL spInsereProduto(111232333485, 'Headset Corsair Virtuoso RGB Wireless', 'Corsair', '/Imagens/virtuosorgbwireless.jpeg', 'Headset gamer sem fio Corsair Virtuoso RGB Wireless com drivers de neodímio de 50 mm, microfone omnidirecional removível e áudio de alta fidelidade.', 999.90, 20);
CALL spInsereProduto(111232333486, 'Headset SteelSeries Arctis Pro Wireless', 'SteelSeries', '/Imagens/arctispro.jpeg', 'Headset gamer sem fio SteelSeries Arctis Pro Wireless com áudio Hi-Res, transmissão dupla sem fio, microfone ClearCast e bateria com até 20 horas de duração.', 1499.90, 10);
CALL spInsereProduto(111232333487, 'Headset Astro A50 Wireless Gen 4', 'Astro', '/Imagens/astroa50gen4.jpeg', 'Headset gamer sem fio Astro A50 Gen 4 com Dolby Audio, base de carregamento magnético, microfone flip-up e personalização de áudio via software.', 1799.90, 8);
CALL spInsereProduto(111232333488, 'Headset Logitech G432', 'Logitech', '/Imagens/logitechg432.jpeg', 'Headset gamer com fio Logitech G432 com drivers de 50 mm, som surround DTS Headphone:X 2.0, microfone 6 mm flip-to-mute e controles de áudio no próprio headset.', 299.90, 30);
CALL spInsereProduto(111232333489, 'Headset Razer Kraken X', 'Razer', '/Imagens/razerkrakenx.jpeg', 'Headset gamer com fio Razer Kraken X com drivers de 40 mm, microfone cardioide dobrável e design ultraleve.', 349.90, 25);
CALL spInsereProduto(111232333490, 'Headset HyperX Cloud Stinger', 'HyperX', '/Imagens/cloudstinger.jpeg', 'Headset gamer com fio HyperX Cloud Stinger com drivers de 50 mm, controle de volume no próprio headset, microfone com cancelamento de ruído e design leve.', 249.90, 35);
CALL spInsereProduto(111232333491, 'Headset Corsair HS70 Pro Wireless', 'Corsair', '/Imagens/hs70prowireless.jpeg', 'Headset gamer sem fio Corsair HS70 Pro Wireless com drivers de neodímio de 50 mm, microfone unidirecional removível e conforto duradouro.', 499.90, 20);
-- Placa-mãe
CALL spInsereProduto(111232333492, 'Placa-mãe Gigabyte B550 AORUS Elite', 'Gigabyte', '/Imagens/b550aoruselite.jpeg', 'Placa-mãe Gigabyte B550 AORUS Elite ATX com suporte para processadores AMD Ryzen de 3ª geração, PCIe 4.0, dissipador de calor VRM e RGB Fusion.', 999.90, 10);
CALL spInsereProduto(111232333493, 'Placa-mãe Asus TUF Gaming B550-Plus', 'Asus', '/Imagens/tufgamingb550plus.jpeg', 'Placa-mãe Asus TUF Gaming B550-Plus ATX com suporte para processadores AMD Ryzen de 3ª geração, PCIe 4.0, componentes TUF de grau militar e Aura Sync.', 799.90, 15);
CALL spInsereProduto(111232333494, 'Placa-mãe MSI MAG B550 Tomahawk', 'MSI', '/Imagens/magb550tomahawk.jpeg', 'Placa-mãe MSI MAG B550 Tomahawk ATX com suporte para processadores AMD Ryzen de 3ª geração, PCIe 4.0, dissipador de calor M.2 e Mystic Light.', 1199.90, 8);
CALL spInsereProduto(111232333495, 'Placa-mãe ASRock B550M Steel Legend', 'ASRock', '/Imagens/b550msteellegend.jpeg', 'Placa-mãe ASRock B550M Steel Legend Micro ATX com suporte para processadores AMD Ryzen de 3ª geração, PCIe 4.0, dissipador de calor XXL e Polychrome RGB.', 699.90, 12);
CALL spInsereProduto(111232333496, 'Placa-mãe Biostar Racing B550GTQ', 'Biostar', '/Imagens/racingb550gtq.jpeg', 'Placa-mãe Biostar Racing B550GTQ ATX com suporte para processadores AMD Ryzen de 3ª geração, PCIe 4.0, áudio Hi-Fi e iluminação LED.', 599.90, 20);
CALL spInsereProduto(111232333497, 'Placa-mãe Colorful CVN X570 Gaming Pro V14', 'Colorful', '/Imagens/cvnx570gamingprov14.jpeg', 'Placa-mãe Colorful CVN X570 Gaming Pro V14 ATX com suporte para processadores AMD Ryzen de 3ª geração, PCIe 4.0, design militar e iluminação RGB.', 899.90, 10);
CALL spInsereProduto(111232333498, 'Placa-mãe EVGA X570 Dark', 'EVGA', '/Imagens/x570dark.jpeg', 'Placa-mãe EVGA X570 Dark E-ATX com suporte para processadores AMD Ryzen de 3ª geração, PCIe 4.0, VRM de 17 fases e conectividade WiFi 6.', 1999.90, 5);
CALL spInsereProduto(111232333500, 'Placa-mãe Gigabyte Z490 AORUS XTREME WATERFORCE', 'Gigabyte', '/Imagens/z490aorusxtremewaterforce.jpeg', 'Placa-mãe Gigabyte Z490 AORUS XTREME WATERFORCE E-ATX com suporte para processadores Intel Core de 10ª e 11ª geração, PCIe 4.0, dissipador de calor Monoblock e RGB Fusion.', 3999.90, 3);
CALL spInsereProduto(111232333501, 'Placa-mãe Asus ROG Zenith II Extreme Alpha', 'Asus', '/Imagens/rogzenithiiextremealpha.jpeg', 'Placa-mãe Asus ROG Zenith II Extreme Alpha E-ATX com suporte para processadores AMD Ryzen Threadripper de 3ª geração, PCIe 4.0, VRM de 16 fases e iluminação Aura Sync.', 5999.90, 2);
-- Processador
CALL spInsereProduto(111232333502, 'Processador Intel Core i7-11700K', 'Intel', '/Imagens/intelcorei7-11700k.jpeg', 'Processador Intel Core i7-11700K de 11ª geração com 8 núcleos, 16 threads, frequência de até 5.0 GHz e tecnologia de overclocking desbloqueada.', 2399.90, 8);
CALL spInsereProduto(111232333503, 'Processador AMD Ryzen 7 5800X', 'AMD', '/Imagens/amdryzen75800x.jpeg', 'Processador AMD Ryzen 7 5800X com 8 núcleos, 16 threads, frequência de até 4.7 GHz e 36 MB de cache.', 2299.90, 10);
CALL spInsereProduto(111232333504, 'Processador Intel Core i5-11600K', 'Intel', '/Imagens/intelcorei5-11600k.jpeg', 'Processador Intel Core i5-11600K de 11ª geração com 6 núcleos, 12 threads, frequência de até 4.9 GHz e tecnologia de overclocking desbloqueada.', 1599.90, 12);
CALL spInsereProduto(111232333505, 'Processador AMD Ryzen 5 5600X', 'AMD', '/Imagens/amdryzen55600x.jpeg', 'Processador AMD Ryzen 5 5600X com 6 núcleos, 12 threads, frequência de até 4.6 GHz e 35 MB de cache.', 1499.90, 15);
CALL spInsereProduto(111232333506, 'Processador Intel Core i9-10900K', 'Intel', '/Imagens/intelcorei9-10900k.jpeg', 'Processador Intel Core i9-10900K de 10ª geração com 10 núcleos, 20 threads, frequência de até 5.3 GHz e tecnologia de overclocking desbloqueada.', 2799.90, 6);
CALL spInsereProduto(111232333507, 'Processador AMD Ryzen 9 5950X', 'AMD', '/Imagens/amdryzen95950x.jpeg', 'Processador AMD Ryzen 9 5950X com 16 núcleos, 32 threads, frequência de até 4.9 GHz e 72 MB de cache.', 4499.90, 4);
CALL spInsereProduto(111232333508, 'Processador Intel Core i3-10100', 'Intel', '/Imagens/intelcorei3-10100.jpeg', 'Processador Intel Core i3-10100 de 10ª geração com 4 núcleos, 8 threads, frequência de até 4.3 GHz e gráficos Intel UHD integrados.', 799.90, 20);
CALL spInsereProduto(111232333509, 'Processador AMD Ryzen 3 3300X', 'AMD', '/Imagens/amdryzen33300x.jpeg', 'Processador AMD Ryzen 3 3300X com 4 núcleos, 8 threads, frequência de até 4.3 GHz e 18 MB de cache.', 699.90, 25);
CALL spInsereProduto(111232333510, 'Processador Intel Pentium Gold G6400', 'Intel', '/Imagens/intelpentiumgoldg6400.jpeg', 'Processador Intel Pentium Gold G6400 de 10ª geração com 2 núcleos, 4 threads, frequência de até 4.0 GHz e gráficos Intel UHD integrados.', 299.90, 30);
CALL spInsereProduto(111232333511, 'Processador AMD Athlon 3000G', 'AMD', '/Imagens/amdathlon3000g.jpeg', 'Processador AMD Athlon 3000G com 2 núcleos, 4 threads, frequência de até 3.5 GHz e gráficos Radeon Vega 3 integrados.', 199.90, 40);
-- Memória
CALL spInsereProduto(111232333512, 'Memória G.Skill Trident Z RGB 32GB (2x16GB)', 'G.Skill', '/Imagens/gskilltridentzrgb.jpeg', 'Kit de memória G.Skill Trident Z RGB 32GB (2x16GB) DDR4 3600MHz CL16 com iluminação RGB configurável e suporte para perfis XMP 2.0.', 1299.90, 10);
CALL spInsereProduto(111232333513, 'Memória Crucial Ballistix RGB 16GB (2x8GB)', 'Crucial', '/Imagens/crucialballistixrgb.jpeg', 'Kit de memória Crucial Ballistix RGB 16GB (2x8GB) DDR4 3200MHz CL16 com iluminação RGB personalizável e latências baixas.', 699.90, 15);
CALL spInsereProduto(111232333514, 'Memória Corsair Vengeance LPX 32GB (2x16GB)', 'Corsair', '/Imagens/corsairvengeancelpx.jpeg', 'Kit de memória Corsair Vengeance LPX 32GB (2x16GB) DDR4 3200MHz CL16 com dissipador de calor em alumínio e alta compatibilidade.', 899.90, 8);
CALL spInsereProduto(111232333515, 'Memória Kingston HyperX Fury RGB 64GB (2x32GB)', 'Kingston', '/Imagens/kingstonhyperxfuryrgb.jpeg', 'Kit de memória Kingston HyperX Fury RGB 64GB (2x32GB) DDR4 3200MHz CL16 com iluminação RGB personalizável e perfil baixo.', 1899.90, 5);
CALL spInsereProduto(111232333516, 'Memória TeamGroup T-Force Delta RGB 16GB (2x8GB)', 'TeamGroup', '/Imagens/teamgrouptforcedeltargb.jpeg', 'Kit de memória TeamGroup T-Force Delta RGB 16GB (2x8GB) DDR4 3200MHz CL16 com iluminação RGB de fluxo e design elegante.', 599.90, 20);
CALL spInsereProduto(111232333517, 'Memória Patriot Viper Steel Series 32GB (2x16GB)', 'Patriot', '/Imagens/patriotvipersteel.jpeg', 'Kit de memória Patriot Viper Steel Series 32GB (2x16GB) DDR4 3600MHz CL18 com dissipador de calor em aço escovado e performance confiável.', 999.90, 12);
CALL spInsereProduto(111232333518, 'Memória Adata XPG Spectrix D60G 64GB (2x32GB)', 'Adata', '/Imagens/adataxpgspectrixd60g.jpeg', 'Kit de memória Adata XPG Spectrix D60G 64GB (2x32GB) DDR4 3200MHz CL16 com iluminação RGB intensa e dissipador de calor com desempenho térmico.', 1699.90, 8);
CALL spInsereProduto(111232333519, 'Memória Crucial Ballistix Max RGB 32GB (2x16GB)', 'Crucial', '/Imagens/crucialballistixmaxrgb.jpeg', 'Kit de memória Crucial Ballistix Max RGB 32GB (2x16GB) DDR4 4000MHz CL18 com iluminação RGB de alta performance e overclocking avançado.', 1399.90, 10);
CALL spInsereProduto(111232333520, 'Memória Corsair Dominator Platinum RGB 64GB (2x32GB)', 'Corsair', '/Imagens/corsairdominatorplatinumrgb.jpeg', 'Kit de memória Corsair Dominator Platinum RGB 64GB (2x32GB) DDR4 3600MHz CL16 com iluminação RGB personalizável e design premium.', 2199.90, 6);
CALL spInsereProduto(111232333521, 'Memória G.Skill Ripjaws V 16GB (2x8GB)', 'G.Skill', '/Imagens/gskillripjawsv.jpeg', 'Kit de memória G.Skill Ripjaws V 16GB (2x8GB) DDR4 3200MHz CL16 com dissipador de calor estilizado e performance comprovada.', 499.90, 25);
-- SSD/HD
CALL spInsereProduto(111232333522, 'SSD Samsung 980 PRO 1TB NVMe', 'Samsung', '/Imagens/samsung980pro.jpeg', 'SSD Samsung 980 PRO NVMe PCIe M.2 de 1TB com velocidades de leitura/gravação de até 7000/5000 MB/s e tecnologia V-NAND 3-bit MLC.', 1499.90, 15);
CALL spInsereProduto(111232333523, 'SSD Crucial P5 Plus 2TB NVMe', 'Crucial', '/Imagens/crucialp5plus.jpeg', 'SSD Crucial P5 Plus NVMe PCIe M.2 de 2TB com velocidades de leitura/gravação de até 6600/5000 MB/s e tecnologia Micron 3D NAND.', 1999.90, 10);
CALL spInsereProduto(111232333524, 'SSD WD Black SN850 500GB NVMe', 'WD', '/Imagens/wdblacksn850.jpeg', 'SSD WD Black SN850 NVMe PCIe M.2 de 500GB com velocidades de leitura/gravação de até 7000/5300 MB/s e tecnologia PCIe Gen4.', 799.90, 20);
CALL spInsereProduto(111232333525, 'SSD Adata XPG GAMMIX S70 2TB NVMe', 'Adata', '/Imagens/adataxpggammixs70.jpeg', 'SSD Adata XPG GAMMIX S70 NVMe PCIe M.2 de 2TB com velocidades de leitura/gravação de até 7400/6400 MB/s e tecnologia 3D NAND Flash.', 2499.90, 8);
CALL spInsereProduto(111232333526, 'SSD Corsair MP600 PRO 1TB NVMe', 'Corsair', '/Imagens/corsairmp600pro.jpeg', 'SSD Corsair MP600 PRO NVMe PCIe M.2 de 1TB com velocidades de leitura/gravação de até 7000/6600 MB/s e tecnologia 3D TLC NAND.', 1699.90, 12);
CALL spInsereProduto(111232333527, 'SSD Gigabyte AORUS Gen4 2TB NVMe', 'Gigabyte', '/Imagens/gigabyteaorusgen4.jpeg', 'SSD Gigabyte AORUS Gen4 NVMe PCIe M.2 de 2TB com velocidades de leitura/gravação de até 7000/6850 MB/s e dissipador de calor de cobre.', 2999.90, 6);
CALL spInsereProduto(111232333528, 'SSD Sabrent Rocket 4 Plus 500GB NVMe', 'Sabrent', '/Imagens/sabrentrocket4plus.jpeg', 'SSD Sabrent Rocket 4 Plus NVMe PCIe M.2 de 500GB com velocidades de leitura/gravação de até 7400/5200 MB/s e dissipador de calor de cobre.', 699.90, 25);
CALL spInsereProduto(111232333529, 'HD Seagate Barracuda 4TB', 'Seagate', '/Imagens/seagatebarracuda.jpeg', 'HD Seagate Barracuda de 4TB com interface SATA III, velocidade de rotação de 5400 RPM, cache de 256 MB e confiabilidade comprovada.', 599.90, 20);
CALL spInsereProduto(111232333530, 'HD Western Digital Blue 1TB', 'WD', '/Imagens/wdblue.jpeg', 'HD Western Digital Blue de 1TB com interface SATA III, velocidade de rotação de 7200 RPM, cache de 64 MB e confiabilidade para uso diário.', 299.90, 35);
CALL spInsereProduto(111232333531, 'HD Toshiba P300 2TB', 'Toshiba', '/Imagens/toshibap300.jpeg', 'HD Toshiba P300 de 2TB com interface SATA III, velocidade de rotação de 7200 RPM, cache de 64 MB e tecnologia de controle de vibração.', 399.90, 30);
-- Placa de vídeo
CALL spInsereProduto(111232333532, 'Placa de vídeo Nvidia GeForce RTX 3090', 'Nvidia', '/Imagens/nvidiageforcertx3090.jpeg', 'Placa de vídeo Nvidia GeForce RTX 3090 com 24 GB de memória GDDR6X, interface de 384 bits, Ray Tracing em tempo real e DLSS AI.', 9999.90, 4);
CALL spInsereProduto(111232333533, 'Placa de vídeo AMD Radeon RX 6900 XT', 'AMD', '/Imagens/amdradeonrx6900xt.jpeg', 'Placa de vídeo AMD Radeon RX 6900 XT com 16 GB de memória GDDR6, interface de 256 bits, Ray Tracing e tecnologia FidelityFX Super Resolution.', 8999.90, 5);
CALL spInsereProduto(111232333534, 'Placa de vídeo Nvidia GeForce RTX 3080', 'Nvidia', '/Imagens/nvidiageforcertx3080.jpeg', 'Placa de vídeo Nvidia GeForce RTX 3080 com 10 GB de memória GDDR6X, interface de 320 bits, Ray Tracing em tempo real e DLSS AI.', 5999.90, 8);
CALL spInsereProduto(111232333535, 'Placa de vídeo AMD Radeon RX 6800 XT', 'AMD', '/Imagens/amdradeonrx6800xt.jpeg', 'Placa de vídeo AMD Radeon RX 6800 XT com 16 GB de memória GDDR6, interface de 256 bits, Ray Tracing e tecnologia FidelityFX Super Resolution.', 4999.90, 10);
CALL spInsereProduto(111232333536, 'Placa de vídeo Nvidia GeForce RTX 3070', 'Nvidia', '/Imagens/nvidiageforcertx3070.jpeg', 'Placa de vídeo Nvidia GeForce RTX 3070 com 8 GB de memória GDDR6, interface de 256 bits, Ray Tracing em tempo real e DLSS AI.', 3999.90, 15);
CALL spInsereProduto(111232333537, 'Placa de vídeo AMD Radeon RX 6700 XT', 'AMD', '/Imagens/amdradeonrx6700xt.jpeg', 'Placa de vídeo AMD Radeon RX 6700 XT com 12 GB de memória GDDR6, interface de 192 bits, Ray Tracing e tecnologia FidelityFX Super Resolution.', 2999.90, 20);
CALL spInsereProduto(111232333538, 'Placa de vídeo Nvidia GeForce RTX 3060 Ti', 'Nvidia', '/Imagens/nvidiageforcertx3060ti.jpeg', 'Placa de vídeo Nvidia GeForce RTX 3060 Ti com 8 GB de memória GDDR6, interface de 256 bits, Ray Tracing em tempo real e DLSS AI.', 2999.90, 20);
CALL spInsereProduto(111232333539, 'Placa de vídeo AMD Radeon RX 6600 XT', 'AMD', '/Imagens/amdradeonrx6600xt.jpeg', 'Placa de vídeo AMD Radeon RX 6600 XT com 8 GB de memória GDDR6, interface de 128 bits, Ray Tracing e tecnologia FidelityFX Super Resolution.', 2499.90, 25);
CALL spInsereProduto(111232333540, 'Placa de vídeo Nvidia GeForce RTX 3060', 'Nvidia', '/Imagens/nvidiageforcertx3060.jpeg', 'Placa de vídeo Nvidia GeForce RTX 3060 com 12 GB de memória GDDR6, interface de 192 bits, Ray Tracing em tempo real e DLSS AI.', 2499.90, 25);
CALL spInsereProduto(111232333541, 'Placa de vídeo AMD Radeon RX 6500 XT', 'AMD', '/Imagens/amdradeonrx6500xt.jpeg', 'Placa de vídeo AMD Radeon RX 6500 XT com 4 GB de memória GDDR6, interface de 64 bits, Ray Tracing e tecnologia FidelityFX Super Resolution.', 1999.90, 30);
-- Monitor
CALL spInsereProduto(111232333542, 'Monitor LG 34GN850-B', 'LG', '/Imagens/lg34gn850b.jpeg', 'Monitor LG 34GN850-B de 34 polegadas com resolução ultrawide QHD (3440x1440), taxa de atualização de 160Hz, tecnologia Nano IPS e suporte para NVIDIA G-SYNC.', 3499.90, 8);
CALL spInsereProduto(111232333543, 'Monitor Asus ROG Swift PG279Q', 'Asus', '/Imagens/asusrogswiftpg279q.jpeg', 'Monitor Asus ROG Swift PG279Q de 27 polegadas com resolução WQHD (2560x1440), taxa de atualização de 165Hz, tecnologia IPS e suporte para NVIDIA G-SYNC.', 2999.90, 10);
CALL spInsereProduto(111232333544, 'Monitor Acer Predator X34', 'Acer', '/Imagens/acerpredatorx34.jpeg', 'Monitor Acer Predator X34 de 34 polegadas com resolução ultrawide QHD (3440x1440), taxa de atualização de 120Hz, tecnologia IPS e suporte para NVIDIA G-SYNC.', 2499.90, 12);
CALL spInsereProduto(111232333545, 'Monitor Dell S2721DGF', 'Dell', '/Imagens/dells2721dgf.jpeg', 'Monitor Dell S2721DGF de 27 polegadas com resolução QHD (2560x1440), taxa de atualização de 165Hz, tecnologia IPS e suporte para AMD FreeSync e NVIDIA G-SYNC.', 1999.90, 15);
CALL spInsereProduto(111232333546, 'Monitor Samsung Odyssey G7', 'Samsung', '/Imagens/samsungodysseyg7.jpeg', 'Monitor Samsung Odyssey G7 de 32 polegadas com resolução WQHD (2560x1440), taxa de atualização de 240Hz, tecnologia QLED e curvatura de 1000R.', 2999.90, 8);
CALL spInsereProduto(111232333547, 'Monitor BenQ EX3501R', 'BenQ', '/Imagens/benqex3501r.jpeg', 'Monitor BenQ EX3501R de 35 polegadas com resolução ultrawide QHD (3440x1440), taxa de atualização de 100Hz, tecnologia HDR e curvatura de 1800R.', 2199.90, 12);
CALL spInsereProduto(111232333548, 'Monitor AOC C24G1', 'AOC', '/Imagens/aocc24g1.jpeg', 'Monitor AOC C24G1 de 24 polegadas com resolução Full HD (1920x1080), taxa de atualização de 144Hz, tecnologia VA e suporte para AMD FreeSync.', 1299.90, 20);
CALL spInsereProduto(111232333549, 'Monitor LG 27GN950-B', 'LG', '/Imagens/lg27gn950b.jpeg', 'Monitor LG 27GN950-B de 27 polegadas com resolução 4K UHD (3840x2160), taxa de atualização de 144Hz, tecnologia Nano IPS e suporte para NVIDIA G-SYNC.', 3999.90, 6);
CALL spInsereProduto(111232333550, 'Monitor ViewSonic Elite XG270QG', 'ViewSonic', '/Imagens/viewsonicelitexg270qg.jpeg', 'Monitor ViewSonic Elite XG270QG de 27 polegadas com resolução QHD (2560x1440), taxa de atualização de 165Hz, tecnologia IPS e suporte para NVIDIA G-SYNC.', 2799.90, 10);
CALL spInsereProduto(111232333551, 'Monitor MSI Optix MAG274QRF-QD', 'MSI', '/Imagens/msioptixmag274qrfqd.jpeg', 'Monitor MSI Optix MAG274QRF-QD de 27 polegadas com resolução QHD (2560x1440), taxa de atualização de 165Hz, tecnologia IPS e suporte para AMD FreeSync.', 2299.90, 12);

-- quase pronto
DELIMITER //
CREATE PROCEDURE InserirFuncionarios(
 IN vNome VARCHAR(200),
 IN vTel BIGINT,
 IN vTipo char(1),
 IN vCep char(8),
 IN vLogradouro VARCHAR(50), 
 IN vNum INT,
 IN vNomeCid VARCHAR(200),
 IN vNomeUf CHAR(2),
 IN vEmail varchar(200),
 IN vSenha varchar(200))
BEGIN
    DECLARE vEstadoId, vCidadeId, vId_log INT;
    DECLARE vTIPO_LOGIN,vSituacao CHAR(1);
    
    SET vEstadoId = (SELECT IdUf from tb_Estado where NomeUf = vNomeUf);
    SET vCidadeId = (SELECT idCid from tb_Cidade where NomeCid = vNomeCid);
    SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    SET vTIPO_LOGIN = 'F';
    SET vSituacao = 'A';

	IF NOT EXISTS(SELECT id_login from tb_login where Email = vEmail) THEN
		INSERT INTO tb_login (email, senha, tipo_login) VALUES (vEmail, vSenha, vTipo_login);
         SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    END IF;
	IF NOT EXISTS (SELECT Id_func from Tb_Funcionario where id_log = vId_log) THEN 
    -- verificaçao de endereco
		IF NOT EXISTS (SELECT Cep from tb_Endereco where Cep = vCep) THEN
			IF NOT EXISTS (SELECT IdUf from tb_Estado where NomeUf = vNomeUf) THEN
				insert into tb_Estado(NomeUf) value(vNomeUf);
                SET vEstadoId = (SELECT IdUf from tb_Estado where nomeUF = vNomeUf);
			END IF;
            IF NOT EXISTS (SELECT idCid from tb_Cidade where NomeCid = vNomeCid) THEN
				insert into tb_Cidade(NomeCid) value(vNomeCid);
                SET vCidadeId = (SELECT idCid from tb_Cidade where NomeCid = vNomeCid);
			END IF;
		insert into tb_Endereco(Cep, logradouro, IdUf, idCid)
						values(vCep, vLogradouro, vEstadoId, vCidadeId);
		END IF;
	INSERT INTO Tb_Funcionario (Nome_Func, DataAdmissao, Tel, Tipo, Situacao, num, id_log, Cep_Func) 
		values(vNome, CURRENT_Date(), vTel, vTipo,vSituacao, vNum, vId_log, vCep);
	else 
		SELECT "Cadastro já existe!!";
    END IF;
END//
DELIMITER ;

CALL InserirFuncionarios('Lulu', 119796012340, 'G', '06029902', 'Núcleo Cidade de Deus', 18, 'Osasco', 'SP', 'luisidebel@gmail.com', 'qwe123');

DELIMITER $$

CREATE PROCEDURE InserirPF(
 IN vNome VARCHAR(200),
 IN vNascimento date,
 IN vSexo char(1),
 IN vTelefone varchar(14),

 IN vRg int,
 In vCpf bigint,
 IN vCep char(8),
 IN vLogradouro VARCHAR(50), 
 IN vNum INT,
 IN vNomeCid VARCHAR(200),
 IN vNomeUf CHAR(2),
 IN vEmail varchar(200),
 IN vSenha varchar(200))
BEGIN
    DECLARE vId_estado, vId_Cidade, vId_log, vId_Cliente INT;
     DECLARE vTIPO_LOGIN, vSituacao CHAR(1);
     
     
    

    -- Recupera Id_estado e Id_Cidade com base nos nomes de estado e cidade fornecidos
    SET vId_estado = (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF);
    SET vId_Cidade = (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid);
	SET vId_Cliente = (SELECT Id_cliente from tb_cliente where id_log = vId_log);
	SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    SET vTIPO_LOGIN = 'C';
    SET vSituacao = 'A';

	
    -- Verifica se o cliente e o endereço já existem
    IF EXISTS (SELECT Id_Cliente FROM Tb_Cliente WHERE Id_Cliente = vId_Cliente) AND EXISTS (SELECT Cep FROM Tb_Endereco WHERE Cep = vCep) THEN
        SELECT 'Cadastro já existe!!';
	 ELSE
		IF NOT EXISTS(SELECT id_login from tb_login where Email = vEmail) THEN
			INSERT INTO tb_login (email, senha, tipo_login) VALUES (vEmail, vSenha, vTipo_login);
			 SET vId_log = (SELECT id_login from tb_login where email = vEmail);
		else 
			SELECT "email já existe!!";
		END IF;
			-- Insere um novo cliente se ele não existir
		IF NOT EXISTS (SELECT Id_Cliente FROM Tb_Cliente WHERE id_log = vId_log) THEN
			INSERT INTO tb_cliente(Nome, Nascimento, Sexo, Telefone, Situacao, id_log) 
			values(vNome, vNascimento, vSexo, vTelefone,vSituacao, vId_log); 
				SET vId_Cliente = LAST_INSERT_ID();
				-- Insere na Tb_PF se o CPF não existir
				IF NOT EXISTS (SELECT Cpf FROM Tb_PF WHERE Cpf = vCPF) THEN
					INSERT INTO Tb_PF(Rg, Cpf, Id_Cli)
					VALUES (vRg, vCpf, vId_Cliente);
				END IF;
				SELECT 'Cliente cadastrado!!';
		END IF;
			-- Insere um novo endereço se ele não existir
		IF NOT EXISTS (SELECT Cep FROM Tb_Endereco WHERE Cep = vCep) THEN
				IF NOT EXISTS (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF) THEN
					INSERT INTO Tb_Estado(NomeUf)
					VALUES (vNomeUF);
					SET vId_estado = (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF);
				 END IF;
					IF NOT EXISTS (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid) THEN
							INSERT INTO Tb_Cidade(NomeCid)
							VALUES (vNomeCid);
							SET vId_Cidade = (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid);
				END IF;
				INSERT INTO Tb_Endereco(Cep, Logradouro, IdUf, IdCid)
				VALUES (vCep, vLogradouro, vId_estado, vId_Cidade);
				SELECT 'Endereço cadastrado!!';
                IF NOT EXISTS (SELECT cep FROM Tb_EndCliente WHERE Cep = vCep And IdCli = vId_Cliente) THEN
			INSERT INTO Tb_EndCliente(Cep, IdCli, num)
			VALUES (vCep, vId_Cliente, vNum);
			SELECT 'Endereço cadastrado na Tb_EndCliente!!';
			END IF;
			END IF;
			-- Vincula o cliente ao endereço
    END IF;
END $$

DELIMITER ;

CALL InserirPF('sasa', '1993-04-02', 'M', '1234543899', 123359, 12345678909, '12344319', 'Rua ABC', 128, 'Cidade do rk', 'RJ', 's11a@example.com', 'sen2ha');
CALL InserirPF('susu', '1992-01-03', 'F', '1234543898', 123358, 12345678908,'12344318', 'Rua bcd', 1322, 'Ouro gtoso', 'MG', 's411a@example.com', 'sen2ha');
CALL InserirPF('Roberto', '1992-01-03', 'M', '1234543898', 123358, 12345678908,'12344318', 'Rua bcd', 1322, 'Ouro gtoso', 'MG', 'Rbpeixotojr@gmail.com', 'senha123');
-- drop procedure InserirPJ;
/*
DELIMITER $$

CREATE PROCEDURE InserirPJ(
 IN vNome VARCHAR(200),
 IN vNascimento date,
 IN vSexo char(1),
 IN vTelefone varchar(14),
 IN vCnpj bigint,
 IN vIE varchar (200),
 IN vNomeFantasia varchar (200),
 IN vRazaoSocial varchar (200),
 IN vCep char(8),
 IN vLogradouro VARCHAR(50), 
 IN vNum INT,
 IN vNomeCid VARCHAR(200),
 IN vNomeUf CHAR(2),
 IN vEmail varchar(200),
 IN vSenha varchar(200))
BEGIN
    DECLARE vId_estado, vId_Cidade, vId_log, vId_Cliente INT;
	DECLARE vTIPO_LOGIN,vSituacao CHAR(1);

    -- Recupera Id_estado e Id_Cidade com base nos nomes de estado e cidade fornecidos
    SET vId_estado = (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF);
    SET vId_Cidade = (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid);
	SET vId_Cliente = (SELECT Id_cliente from tb_cliente where id_log = vId_log);
	SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    SET vTIPO_LOGIN = 'C';
    SET vSituacao = 'A';

	
    -- Verifica se o cliente e o endereço já existem
    IF EXISTS (SELECT Id_Cliente FROM Tb_Cliente WHERE Id_Cliente = vId_Cliente) AND EXISTS (SELECT Cep FROM Tb_Endereco WHERE Cep = vCep) THEN
		SELECT 'Cadastro já existe!!';
	IF NOT EXISTS (SELECT Cnpj FROM Tb_PJ WHERE Cnpj = vCnpj) THEN
		INSERT INTO Tb_PJ(Cnpj, IE, NomeFantasia, RazaoSocial, Id_Cli)
		VALUES (vCnpj, vIE, vNomeFantasia, vRazaoSocial, vId_Cliente);
	END IF;
    IF NOT EXISTS(SELECT id_login FROM tb_login WHERE Email = vEmail) THEN
        INSERT INTO tb_login (email, senha, tipo_login) VALUES (vEmail, vSenha, vTipo_login);
        SET vId_log = (SELECT id_login FROM tb_login WHERE email = vEmail);
    ELSE 
        SELECT 'email já existe!!';
    END IF;
    
    -- Insere um novo cliente se ele não existir
    IF NOT EXISTS (SELECT Id_Cliente FROM Tb_Cliente WHERE id_log = vId_log) THEN
        INSERT INTO tb_cliente(Nome, Situacao, Nascimento, Sexo, Telefone, id_log) 
        VALUES (vNome, vSituacao, vNascimento, vSexo, vTelefone, vId_log);
        
        SET vId_Cliente = LAST_INSERT_ID();
        
        -- Insere na Tb_PJ se o CNPJ não existir

        
        SELECT 'Cliente cadastrado!!';
    END IF;
    
    -- Insere um novo endereço se ele não existir
    IF NOT EXISTS (SELECT Cep FROM Tb_Endereco WHERE Cep = vCep) THEN
        IF NOT EXISTS (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF) THEN
            INSERT INTO Tb_Estado(NomeUf)
            VALUES (vNomeUF);
            
            SET vId_estado = (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF);
        END IF;
        
        IF NOT EXISTS (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid) THEN
            INSERT INTO Tb_Cidade(NomeCid)
            VALUES (vNomeCid);
            
            SET vId_Cidade = (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid);
        END IF;
        
        INSERT INTO Tb_Endereco(Cep, Logradouro, IdUf, IdCid)
        VALUES (vCep, vLogradouro, vId_estado, vId_Cidade);
        
        SELECT 'Endereço cadastrado!!';
    END IF;
    
    -- Vincula o cliente ao endereço
    IF NOT EXISTS (SELECT cep FROM Tb_EndCliente WHERE Cep = vCep AND IdCli = vId_Cliente) THEN
        INSERT INTO Tb_EndCliente(Cep, IdCli, num)
        VALUES (vCep, vId_Cliente, vNum);
        
        SELECT 'Endereço cadastrado na Tb_EndCliente!!';
    END IF;
END IF;

a
DELIMITER ;

CALL InserirPJ('Nome', '1990-01-01', 'M', '1234567890', 12345678901234, '123456', 'Nome Fantasia', 'Razao Social', '12345678', 'Rua Exemplo', 123, 'Cidade Exemplo', 'UF', 'exemplo@email.com', 'senha');
CALL InserirPJ('FIDO', '1998-11-21', 'F', '119123112345', 12345678901237, '123456', 'Nome Fantasia', 'Razao Social', '12345679', 'Rua dtc', 123, 'Cidade tcc', 'SP', 'i@jmail.com', 'senha');
*/ww
select * from tb_login;

select * from tb_cliente;
select * from tb_pj;
-- procedimentos vendas e trigger


DELIMITER //

CREATE TRIGGER DiminuirQtdProduto 
AFTER INSERT ON Tb_ItemVenda 
FOR EACH ROW 
BEGIN 
    DECLARE nova_qtd DECIMAL(8,2); 
    DECLARE produto_existente DECIMAL(8,2); 
    
    SELECT QtdEst INTO produto_existente FROM tb_produto WHERE CodBarras = NEW.CodBarras; 
    
    IF produto_existente IS NOT NULL THEN 
        SET nova_qtd = produto_existente - NEW.Qtd; 
        UPDATE tb_produto SET QtdEst = nova_qtd WHERE CodBarras = NEW.CodBarras; 
    END IF; 
END;//

DELIMITER ;
-- falta verificar quantidade
DELIMITER //

CREATE PROCEDURE RegistrarVenda(
    vNf INT,
	vEmail varchar(200),
    vQtd SMALLINT,
    vCodBarras BIGINT
)
BEGIN
    DECLARE Id_Cli, QtdEstoque INT;
    DECLARE VALOR_PRODUTO, SOMA DECIMAL(8,2);
    
    SET VALOR_PRODUTO = (SELECT Valor FROM tb_produto WHERE CodBarras = vCodBarras);
    SET Id_Cli = (SELECT Id_cliente FROM vw_ClienteEnd WHERE email = vEmail);
    SET SOMA = (SELECT ValorTotal FROM Tb_Vendas WHERE Nf = vNf);
    
    IF SOMA IS NULL THEN
        SET SOMA = 0;
    END IF;
    
    IF NOT EXISTS(SELECT Id_cliente FROM vw_ClienteEnd WHERE email = vEmail) THEN 
        SELECT "Cliente não cadastrado!!";
    ELSE
        IF NOT EXISTS(SELECT Nf FROM Tb_Vendas WHERE Nf = vNf) THEN
            IF NOT EXISTS(SELECT CodBarras FROM tb_produto WHERE CodBarras = vCodBarras) THEN
                SELECT "Produto não cadastrado!!";
            ELSE
                IF (0 >= (SELECT QtdEst - vQtd FROM tb_produto WHERE CodBarras = vCodBarras)) THEN
                    SET QtdEstoque = (SELECT QtdEst FROM tb_produto WHERE CodBarras = vCodBarras);
                    SELECT CONCAT("Estoque insuficiente! QTD em estoque: ", QtdEstoque); 
                ELSE
                    INSERT INTO Tb_Vendas (Nf, ValorTotal, Data_, Id_cli)
                    VALUES (vNf, SOMA, NOW(), Id_Cli);
                    
                    INSERT INTO Tb_ItemVenda (Nf, CodBarras, Valor, Qtd)
                    VALUES (vNf, vCodBarras, VALOR_PRODUTO, vQtd);
                    
                    UPDATE Tb_Vendas
                    SET ValorTotal = SOMA + VALOR_PRODUTO * vQtd,
                    Data_ = NOW()
                    WHERE Nf = vNf;
                    
                    SELECT ("Venda Realizada"); 
                END IF;
            END IF;
        ELSE 
            IF EXISTS (SELECT Nf FROM Tb_ItemVenda WHERE Nf = vNf AND CodBarras = vCodBarras) THEN
                SELECT "Duplicidade, Item já registrado!";
            ELSEIF NOT EXISTS(SELECT CodBarras FROM tb_produto WHERE CodBarras = vCodBarras) THEN
                SELECT "Produto não cadastrado!!";
            ELSE
                IF (0 >= (SELECT QtdEst - vQtd FROM tb_produto WHERE CodBarras = vCodBarras)) THEN
                    SET QtdEstoque = (SELECT QtdEst FROM tb_produto WHERE CodBarras = vCodBarras);
                    SELECT CONCAT("Estoque insuficiente! QTD em estoque: ", QtdEstoque); 
                ELSE
					SET vNF = (SELECT Nf from Tb_Vendas ORDER BY Nf DESC LIMIT 1);
                    INSERT INTO Tb_ItemVenda (Nf, CodBarras, Valor, Qtd)
                    VALUES (vNf, vCodBarras, VALOR_PRODUTO, vQtd);
                    
                    UPDATE Tb_Vendas
                    SET ValorTotal = SOMA + VALOR_PRODUTO * vQtd,
                    Data_ = NOW()
                    WHERE Nf = vNf;
                END IF;
            END IF;
        END IF;
    END IF;
END //
    
DELIMITER ;
-- drop procedure RegistrarVenda;
select * from tb_produto;
call RegistrarVenda (10011,'s411a@example.com', 1, 111232333448);
call RegistrarVenda (10011,'s411a@example.com', 2, 111232333446);
call RegistrarVenda (10012,'exemplo@email.com', 1, 111232333501);
-- Filtros
DELIMITER //

CREATE PROCEDURE sp_ordenaProduto(
    IN opc VARCHAR(10)
)
BEGIN
    IF opc = 'pormenor' THEN
        SELECT *
        FROM tb_produto
        ORDER BY Valor ASC;
    ELSEIF opc = 'pormaior' THEN
        SELECT *
        FROM tb_produto
        ORDER BY Valor DESC;
    ELSE
        SELECT *
        FROM tb_produto;
    END IF;
END //

DELIMITER ;
call sp_ordenaProduto('');
call sp_ordenaProduto('pormenor');
call sp_ordenaProduto('pormaior');

-- drop procedure updFuncEnd;
-- alterar funcionario avisar os erros que der
DELIMITER //

DELIMITER //

CREATE PROCEDURE updFuncEnd(
	IN vId_func INT,
    IN vNome_func VARCHAR(200),    
    IN vCep_func numeric(8),
    IN vLogradouro VARCHAR(50), 
    IN vNum INT,
    IN vNomeCid VARCHAR(200),
    IN vNomeUF CHAR(2),
    IN vEmail VARCHAR(200),
    IN vSenha VARCHAR(200),
    IN vTel BIGINT,
    IN vTipo CHAR(1)
)
BEGIN
	DECLARE vId_estado, vId_Cidade INT;
 
  /*  -- Verificar se algum parâmetro é NULL
    IF vNome_func IS NULL OR vId_func IS NULL OR vCep_func IS NULL OR vLogradouro IS NULL OR vNum IS NULL OR vNomeCid IS NULL OR vNomeUF IS NULL OR vEmail IS NULL OR vSenha IS NULL OR vTel IS NULL OR vTipo IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Nenhum campo pode ser nulo.';
    END IF;
*/	IF NOT EXISTS (SELECT ID_FUNC FROM tb_funcionario WHERE ID_FUNC = vId_func) THEN
			SELECT 'ID FUNCIONARIO NAO ENCONTRADO';
	ELSE 
    -- Verificar se o endereço já existe
    IF NOT EXISTS (SELECT Cep FROM Tb_Endereco WHERE Cep = vCep_func) THEN
		IF NOT EXISTS (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF) THEN
			INSERT INTO Tb_Estado(NomeUf)
			VALUES (vNomeUF);
			SET vId_estado = (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF);
		END IF;
		IF NOT EXISTS (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid) THEN
			INSERT INTO Tb_Cidade(NomeCid)
			VALUES (vNomeCid);
			SET vId_Cidade = (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid);
		END IF;
		-- Inserir o novo endereço
		INSERT INTO Tb_Endereco(Cep, Logradouro, IdUf, IdCid)
		VALUES (vCep_func, vLogradouro, vId_estado, vId_Cidade);
		SELECT 'Endereço cadastrado!!';
         -- Atualizar o endereço do funcionário
		UPDATE Tb_Funcionario
		SET Cep_Func = vCep_func,
			Nome_Func = vNome_func,
			num = vNum,
			tel = vTel,
			tipo = vTipo
		WHERE id_func = vId_func;
		SELECT 'campos alterados com sucesso !!';
		-- ceP EXISTENTE LOGO ENDERECO EXISTENTE
	ELSE 
		 -- Atualizar o endereço do funcionário
		UPDATE Tb_Funcionario
		SET Cep_Func = vCep_func,
			Nome_Func = vNome_func,
			num = vNum,
			tel = vTel,
			tipo = vTipo
		WHERE id_func = vId_func;
		SELECT 'campos alterados com sucesso !!';
		END IF;
	END IF;
END //
DELIMITER ;
SELECT ID_FUNC FROM tb_funcionario WHERE ID_FUNC = 2;
 SELECT * FROM TB_FUNCIONARIO;

CALL updFuncEnd(
    1,                        -- ID do funcionário
	'lo',   				  -- Nome do funcionário
    22446687,                  -- CEP do endereço do funcionário
    'Rua d emerda',           -- Logradouro do endereço do funcionário
    123,                      -- Número do endereço do funcionário
    ' taro',          -- Nome da cidade do endereço do funcionário
    'SP',                       -- UF (Unidade Federativa) do endereço do funcionário
    'luisidebel@gmail.com',        -- Email do funcionário
    'senha123',                 -- Senha do funcionário
    119796012340,                 -- Telefone do funcionário
    'G'                         -- Tipo do funcionário (por exemplo, 'A' para Administrativo)
);

desc tb_estado;
select * from Tb_Endereco;
select * from Tb_Endcliente;
select * from Tb_Estado;
select * from Tb_cidade;
select * from TB_FUNCIONARIO;

select * from vw_FuncEnd order by Id_func;
select *  from tb_funcionario;
select count(*) from tb_produto where nome Like 'placa%';
select * from tb_produto where nome Like 'placa%';
select distinct nomeuf from tb_estado;

select * from tb_produto;  
/*
DELIMITER //

CREATE TRIGGER DiminuirQtdProduto 
AFTER INSERT ON Tb_ItemVenda 
FOR EACH ROW 
BEGIN 
    DECLARE nova_qtd DECIMAL(8,2); 
    DECLARE produto_existente DECIMAL(8,2); 
    
    SELECT QtdEst INTO produto_existente FROM tb_produto WHERE CodBarras = NEW.CodBarras; 
    
    IF produto_existente IS NOT NULL THEN 
        SET nova_qtd = produto_existente - NEW.Qtd; 
        UPDATE tb_produto SET QtdEst = nova_qtd WHERE CodBarras = NEW.CodBarras; 
    END IF; 
END;//

DELIMITER ;*/

-- Views
select * from vw_cliCad;
select * from vw_ClienteEnd;
select * from vw_FuncEnd order by Id_func;

desc tb_login;
desc tb_cliente;
desc TB_FUNCIONARIO;
desc TB_endereco;
desc tb_pj;
desc tb_funcionario;
desc tb_produto;
desc tb_vendas;

select * from tb_login;
select * from TB_cliente;
select * from Tb_Endereco;
select * from Tb_Endcliente;
select * from Tb_Estado;
select * from Tb_cidade;
select * from TB_FUNCIONARIO;
select * from TB_pf;
select * from tb_pj;
select * from tb_login;
select * from TB_cliente;
select * from TB_funcionario;
select * from tb_produto;
select * from Tb_ItemVenda;
select * from Tb_Vendas;
show tables;