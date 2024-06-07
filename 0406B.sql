-- drop database bdGameFesth;
-- show tables;

CREATE DATABASE bdGameFesth;
USE bdGameFesth;
drop database bdGameFesth;
CREATE TABLE tb_login(
	id_Login int primary key auto_increment,
    email varchar(200) not null unique,
    senha  varchar(200) not null,
    tipo_login char(1)
);

CREATE  TABLE Tb_Produto( 
    CodBarras bigint primary key, 
    Nome varchar(100) not null, 
    ImagemProduto varchar(255) not null,
    Valor decimal (8,2) check (Valor > 0) not null, 
    Qtd smallint check (Qtd >= 0) not null
);

CREATE TABLE Tb_Cliente( 
    Id_cliente int primary key auto_increment, 
    Nome varchar(200) not null, 
    Nascimento date not null, 
    Sexo char(1), 
    Telefone varchar(14) not null,
    Situacao char(1),
    id_log int not null,
    foreign key (id_log) references tb_login(id_Login) 
);

CREATE TABLE Tb_Vendas( 
    Nf int primary key, 
    ValorTotal decimal (6,2) not null, 
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

CREATE TABLE Tb_Cidade( 
    IdCid int primary key auto_increment, 
    NomeCid varchar(200) unique not null 
);

CREATE TABLE Tb_Endereco( 
	CEP numeric(8) primary key not null, 
    Logradouro varchar(50), 
    IdUf int not null, 
    IdCid int not null, 
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
    Tipo char(1), 
    num int,
    Cep_Func numeric(8) not null,
	id_log int not null,
    foreign key (id_log) references tb_login(id_Login),
    foreign key (Cep_Func) references Tb_Endereco(CEP) 
);
-- ALTER TABLE Tb_Funcionario ADD num int;
-- ALTER TABLE tb_endcliente DROP FOREIGN KEY tb_endcliente_ibfk_1;
-- ALTER TABLE tb_endcliente ADD FOREIGN KEY (cep) references tb_Endereco(CEP);
-- DROP procedure spInsereProduto;

-- Insere produto
DELIMITER $$
CREATE PROCEDURE spInsereProduto (IN vCodBarras BIGINT,
 IN  vNome VARCHAR(100),
 IN vImagemProduto VARCHAR(255), 
 IN vValor DECIMAL(8,2), 
 IN vQtd smallint)
BEGIN
	INSERT INTO Tb_Produto (CodBarras, ImagemProduto, Nome, Valor, Qtd) VALUES (vCodBarras, vImagemProduto, vNome, vValor, vQtd);
END $$
DELIMITER ;

CALL spInsereProduto(111232333445, '/Imagens/renan.jfif','Bola de Futebol', 79.90, 8);

-- DROP procedure sp_InsereFuncionario;
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
    DECLARE vTIPO_LOGIN CHAR(1);
    
    SET vEstadoId = (SELECT IdUf from tb_Estado where NomeUf = vNomeUf);
    SET vCidadeId = (SELECT idCid from tb_Cidade where NomeCid = vNomeCid);
    SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    SET vTIPO_LOGIN = 'F';

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
	INSERT INTO Tb_Funcionario (Nome_Func, DataAdmissao, Tel, Tipo, num, id_log, Cep_Func) 
		values(vNome, CURRENT_Date(), vTel, vTipo, vNum, vId_log, vCep);
	else 
		SELECT "Cadastro já existe!!";
    END IF;
END//
DELIMITER ;

CALL InserirFuncionarios (
    'ilva', -- vNome
    123456789, -- vTel
    'G', -- vTipo 
    '12345678', -- vCep
    'Rua Exemplo1', -- vLogradouro
    123, -- vNum
    'Cidade Exemplo', -- vNomeCid
    'Sp',
    'João@email.com',-- email
    '123123'-- senha
);
CALL InserirFuncionarios('Silva', 123456789, 'G', '12345678', 'Rua Exemplo1', 123, 'Cidade Exemplo', 'SP', 'joao@email.com', '123123');
CALL InserirFuncionarios('Silveira', 987654321, 'F', '87654888', 'Rua Teste', 456, 'Cidade Teste', 'RJ', 'silveira@email.com', '456456');
CALL InserirFuncionarios('Santos', 987654321, 'G', '12345698', 'Rua Exemplo2', 789, 'Cidade asd', 'SP', 'santos@email.com', '789789');
CALL InserirFuncionarios('Pereira', 123456789, 'F', '12345678', 'Rua Teste', 321, 'Cidade as', 'RJ', 'pereira@email.com', '321321');
CALL InserirFuncionarios('Costa', 987654321, 'G', '87654321', 'Rua Exemplo3', 654, 'Cidade sdf', 'SP', 'costa@email.com', '654654');
CALL InserirFuncionarios('Ferreira', 123456789, 'F', '12345678', 'Rua Teste', 987, 'Cidade fdgh', 'RJ', 'ferreira@email.com', '987987');
CALL InserirFuncionarios('Almeida', 987654321, 'G', '87654321', 'Rua Exemplo4', 147, 'Cidade ghjk', 'SP', 'almeida@email.com', '147147');
CALL InserirFuncionarios('Souza', 123456789, 'F', '12345678', 'Rua Teste', 258, 'fghj Teste', 'RJ', 'souza@email.com', '258258');
CALL InserirFuncionarios('Gomes', 987654321, 'G', '87654321', 'Rua Exemplo5', 369, 'ghjf Exemplo', 'SP', 'gomes@email.com', '369369');
CALL InserirFuncionarios('Carvalho', 123456789, 'F', '12345678', 'Rua Teste', 159, 'gfhj Teste', 'RJ', 'carvalho@email.com', '159159');
CALL InserirFuncionarios('Silva', 123456789, 'G', '12345678', 'Rua A', 123, 'Cidade Alpha', 'SP', 'joaoa@email.com', '123123');
CALL InserirFuncionarios  ('Silveira', 987654321, 'F', '87654888', 'Rua B', 456, 'Cidade Beta', 'RJ', 'silveira@email.com', '456456');
CALL InserirFuncionarios  ('Santos', 987654321, 'G', '12345698', 'Rua C', 789, 'Cidade Gamma', 'MG', 'santos@email.com', '789789');
CALL InserirFuncionarios  ('Pereira', 123456789, 'F', '12345678', 'Rua D', 321, 'Cidade Delta', 'RS', 'pereira@email.com', '321321');
CALL InserirFuncionarios ('Costa', 987654321, 'G', '87654321', 'Rua E', 654, 'Cidade Epsilon', 'BA', 'costa@email.com', '654654');
CALL InserirFuncionarios ('Ferreira', 123456789, 'F', '12345678', 'Rua F', 987, 'Cidade Zeta', 'PE', 'ferreira@email.com', '987987');
CALL InserirFuncionarios ('Almeida', 987654321, 'G', '87654321', 'Rua G', 147, 'Cidade Eta', 'SC', 'almeida@email.com', '147147');
CALL InserirFuncionarios   ('Souza', 123456789, 'F', '12345678', 'Rua H', 258, 'Cidade Theta', 'PR', 'souza@email.com', '258258');
CALL InserirFuncionarios  ('Gomes', 987654321, 'G', '87654321', 'Rua I', 369, 'Cidade Iota', 'ES', 'gomes@email.com', '369369');
CALL InserirFuncionarios  ('Carvalho', 123456789, 'F', '12345678', 'Rua J', 159, 'Cidade Kappa', 'DF', 'carvalho@email.com', '159159');


/*
-- procedure cliente em producao
DELIMITER //
CREATE PROCEDURE InserirCliente(
 IN vNome VARCHAR(200),
 IN vNascimento date,
 IN vSexo char(1),
 IN vTelefone varchar(14),
 IN vCep char(8),
 IN vLogradouro VARCHAR(50), 
 IN vNum INT,
 IN vNomeCid VARCHAR(200),
 IN vNomeUf CHAR(2),
  IN vEmail varchar(200),
 IN vSenha varchar(200))
BEGIN
    DECLARE vEstadoId, vCidadeId, vId_log, vId_Cliente INT;
    DECLARE vTIPO_LOGIN CHAR(1);
    
    SET vEstadoId = (SELECT IdUf from tb_Estado where NomeUf = vNomeUf);
    SET vCidadeId = (SELECT idCid from tb_Cidade where NomeCid = vNomeCid);
    
    SET vId_Cliente = (SELECT Id_cliente from tb_cliente where id_log = vId_log);
    SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    SET vTIPO_LOGIN = 'C';

	IF NOT EXISTS(SELECT id_login from tb_login where Email = vEmail) THEN
		INSERT INTO tb_login (email, senha, tipo_login) VALUES (vEmail, vSenha, vTipo_login);
         SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    else 
		SELECT "email já existe!!";
    END IF;
	IF NOT EXISTS (SELECT Id_cliente from tb_cliente where Id_cliente = vId_Cliente) THEN 
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
	
	else 
		SELECT "Cadastro já existe!!";
    END IF;
END//
DELIMITER ;

CALL InserirCliente (
    'Maria Silva', -- vNome
    '1990-05-15', -- vNascimento
    'F', -- vSexo
    '1234567890', -- vTelefone
    '12345678', -- vCep
    'Rua Exemplo', -- vLogradouro
    123, -- vNum
    'Cidade Exemplo', -- vNomeCid
    'UF', -- vNomeUf
    'maria@email.com', -- vEmail
    'senha123' -- vSenha
);
*/
desc tb_cliente;
DELIMITER $$

CREATE PROCEDURE InserirPF(
 IN vNome VARCHAR(200),
 IN vNascimento date,
 IN vSexo char(1),
 IN vTelefone varchar(14),
 IN vSituacao char(1),
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
     DECLARE vTIPO_LOGIN CHAR(1);
    

    -- Recupera Id_estado e Id_Cidade com base nos nomes de estado e cidade fornecidos
    SET vId_estado = (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF);
    SET vId_Cidade = (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid);
	SET vId_Cliente = (SELECT Id_cliente from tb_cliente where id_log = vId_log);
	SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    SET vTIPO_LOGIN = 'C';

	
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
			END IF;
			-- Vincula o cliente ao endereço
		IF NOT EXISTS (SELECT cep FROM Tb_EndCliente WHERE Cep = vCep And IdCli = vId_Cliente) THEN
			INSERT INTO Tb_EndCliente(Cep, IdCli, num)
			VALUES (vCep, vId_Cliente, vNum);
			SELECT 'Endereço cadastrado na Tb_EndCliente!!';
			END IF;
    END IF;
END $$

DELIMITER ;

CALL InserirPF('sasa', '1993-04-02', 'M', '1234543899','A', 123359, 12345678909, '12344319', 'Rua ABC', 128, 'Cidade do rk', 'RJ', 's11a@example.com', 'sen2ha');
CALL InserirPF('susu', '1992-01-03', 'F', '1234543898','A', 123358, 12345678908,'12344318', 'Rua bcd', 1322, 'Ouro gtoso', 'MG', 's411a@example.com', 'sen2ha');
CALL InserirPF('sisi', '1983-02-04', 'M', '1234543897','A', 123352, 12345678902,'12344318', 'Rua cde', 321, 'Ouro fino', 'MG', 'sa@example.com', 'se3nha');


-- nao funcionando esta inseridno nas tabelas clientes mas nao na pj
/*
DELIMITER $$

CREATE PROCEDURE InserirPJ(
 IN vNome VARCHAR(200),
 IN vNascimento date,
 IN vSexo char(1),
 IN vTelefone varchar(14),
 IN vSituacao char(1),
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
     DECLARE vTIPO_LOGIN CHAR(1);
    

    -- Recupera Id_estado e Id_Cidade com base nos nomes de estado e cidade fornecidos
    SET vId_estado = (SELECT IdUf FROM Tb_Estado WHERE NomeUf = vNomeUF);
    SET vId_Cidade = (SELECT IdCid FROM Tb_Cidade WHERE NomeCid = vNomeCid);
	SET vId_Cliente = (SELECT Id_cliente from tb_cliente where id_log = vId_log);
	SET vId_log = (SELECT id_login from tb_login where email = vEmail);
    SET vTIPO_LOGIN = 'C';

	
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
			INSERT INTO tb_cliente(Nome, Nascimento, Sexo, Telefone,Situacao, id_log) 
			values(vNome, vNascimento, vSexo, vTelefone, vSituacao, vId_log); 
				SET vId_Cliente = LAST_INSERT_ID();
				-- Insere na Tb_PJ se o CNPJ não existir
				IF NOT EXISTS (SELECT Cpf FROM Tb_PF WHERE Cpf = vCPF) THEN
					INSERT INTO Tb_PJ(Cnpj, IE, NomeFantasia, RazaoSocial ,Id_Cli)
					VALUES (vCnpj, vIE, vNomeFantasia, vRazaoSocial,vId_Cliente);
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
			END IF;
			-- Vincula o cliente ao endereço
		IF NOT EXISTS (SELECT cep FROM Tb_EndCliente WHERE Cep = vCep And IdCli = vId_Cliente) THEN
			INSERT INTO Tb_EndCliente(Cep, IdCli, num)
			VALUES (vCep, vId_Cliente, vNum);
			SELECT 'Endereço cadastrado na Tb_EndCliente!!';
			END IF;
    END IF;
END $$

DELIMITER ;

CALL InserirPJ(
    'Nome da Empresa',
    '2000-01-01', -- Data de Nascimento
    'M', -- Sexo (M para masculino, F para feminino, etc.)
    '1234567890', -- Telefone
    12345678901234, -- CNPJ
    '123456789', -- Inscrição Estadual
    'Nome Fantasia', 
    'Razao Social', 
    '12345678', -- CEP
    'Rua Exemplo', -- Logradouro
    123, -- Número
    'Cidade Exemplo', 
    'UF', -- Estado
    'email@example.com', 
    'senha123'
);

CALL InserirPJ('sasa', '1990-01-01', 'M', '1234543899', 123359, '', 'Olha o capa', 'Rua ABC', 128, 'Cidade do rk', 'RJ', 's11a@example.com', 'senha');
CALL InserirPJ('susu', '1990-01-01', 'M', '1234543898', 123358, 12345678908, '12344318', 'Rua casca', 128, 'Ouro fino', 'MG', 's411a@example.com', 'senha');
CALL InserirPJ('sisi', '1990-01-01', 'M', '1234543897', 123352, 12345678902, '12344318', 'Rua casca', 128, 'Ouro fino', 'MG', 'sa@example.com', 'senha');
*/

-- drop procedure InserirPJ;




-- view funcionario
create view vw_FuncEnd as
select Id_func, id_log, Nome_Func, DataAdmissao, Tel, Tipo, Num, TF.Cep_Func, Logradouro, NomeUf, NomeCid, email, senha
from TB_FUNCIONARIO TF
join Tb_Endereco TE on TF.Cep_Func = TE.CEP
join Tb_Estado TES on TE.idUf = TES.idUF
join Tb_cidade TC on TE.idCid = TC.IdCid
join tb_login TL on TF.id_log = TL.id_login;
drop view vw_FuncEnd;
select * from vw_FuncEnd order by Id_func;


-- view cliente
create view vw_ClienteEnd as
select Id_cliente, id_log, Nome, Nascimento, Sexo, Telefone, Num, TEC.CEP, Logradouro, NomeUf, NomeCid, email, senha
from tb_cliente TCL
join TB_endCLiente TEC on TCL.id_cliente = TEC.idcli
join Tb_Endereco TE on TEC.Cep = TE.CEP
join Tb_Estado TES on TE.idUf = TES.idUF
join Tb_cidade TC on TE.idCid = TC.IdCid
join tb_login TL on TCL.id_log = TL.id_login;
drop view vw_ClienteEnd;
select * from vw_ClienteEnd;

/*
desc tb_cliente;
desc tb_endcliente;
desc tb_endereco;
desc tb_cidade;
desc tb_estado;
desc tb_pj;
desc tb_pf;
*/
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

select * from tb_login;
select * from TB_cliente;
select * from Tb_Endereco;
select * from Tb_Endcliente;
select * from Tb_Estado;
select * from Tb_cidade;
select * from TB_FUNCIONARIO;
select * from TB_pf;
select * from tb_pj;
show tables;

update tb_cliente set situacao = 'D' where Id_cliente = 2;
/*
update Tb_Cliente set situacao=@situacao where Id_cliente=@id
-- drop database bdGameFesth;
