-- drop database bdGameFesth;
/*
CREATE USER 'luis'@'localhost' IDENTIFIED BY '12345678';
grant all privileges on bdgamefesth.* To 'luis'@'localhost';
flush privileges;
*/
CREATE DATABASE bdGameFesth;
USE bdGameFesth;

CREATE TABLE tb_produto( 
    CodBarras bigint primary key, 
    Nome varchar(100) not null,
    Marca varchar(100) not null,
    Descricao varchar(400) not null,
    ImagemProduto varchar(255) not null,
    Valor decimal (8,2) check (Valor > 0) not null, 
    QtdEst smallint check (QtdEst >= 0) not null
);
CREATE TABLE tb_login(
	id_Login int primary key auto_increment,
    email varchar(200) not null unique,
    senha  varchar(200) not null,
    tipo_login char(1)

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
    Situacao char(1),
	id_log int not null,
    foreign key (id_log) references tb_login(id_Login),
    foreign key (Cep_Func) references Tb_Endereco(CEP) 
);
-- ALTER TABLE Tb_Funcionario ADD num int;
-- ALTER TABLE tb_endcliente DROP FOREIGN KEY tb_endcliente_ibfk_1;
-- ALTER TABLE tb_endcliente ADD FOREIGN KEY (cep) references tb_Endereco(CEP);
-- DROP procedure spInsereProduto;

-- view funcionario
create view vw_FuncEnd as
select Id_func, id_log, Nome_Func, DataAdmissao, Tel, Tipo,Situacao, Num, TF.Cep_Func, Logradouro, NomeUf, NomeCid, email, senha
from TB_FUNCIONARIO TF
join Tb_Endereco TE on TF.Cep_Func = TE.CEP
join Tb_Estado TES on TE.idUf = TES.idUF
join Tb_cidade TC on TE.idCid = TC.IdCid
join tb_login TL on TF.id_log = TL.id_login;

select * from vw_FuncEnd order by Id_func;

-- view cliente
create view vw_ClienteEnd as
select Id_cliente, id_log, Nome, Nascimento, Sexo,Situacao, Telefone, Num, TEC.CEP, Logradouro, NomeUf, NomeCid, email, senha
from tb_cliente TCL
join TB_endCLiente TEC on TCL.id_cliente = TEC.idcli
join Tb_Endereco TE on TEC.Cep = TE.CEP
join Tb_Estado TES on TE.idUf = TES.idUF
join Tb_cidade TC on TE.idCid = TC.IdCid
join tb_login TL on TCL.id_log = TL.id_login;

select * from vw_ClienteEnd;

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

CALL spInsereProduto(111232333445,'Bola de Futebol', 'nosso', '/Imagens/renan.jfif','bob marley', 79.90, 8);
CALL spInsereProduto(11123233345,'Bola de ', 'nosso', '/Imagens/renan.jfif','bob marley', 79.90, 8);
CALL spInsereProduto(11123233445,'Bola de Futebol', 'nosso', '/Imagens/renan.jfif','bob marley', 79.90, 8);
CALL spInsereProduto(11232333445,'Bola de Futebol', 'nosso', '/Imagens/renan.jfif','bob marley', 9.90, 8);
CALL spInsereProduto(211232333445,'de Futebol', 'nosso', '/Imagens/renan.jfif','bob marley', 100.90, 8);
CALL spInsereProduto(311232333445,'quadra de Futebol', 'nosso', '/Imagens/renan.jfif','bob marley', 50.90, 8);
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
CALL InserirFuncionarios('Silva', 123456789, 'G', '12345678', 'Rua A', 123, 'Cidade Alpha', 'SP', 'Cavalotatuado6262@gmail.com', '123123');
CALL InserirFuncionarios  ('Silveira', 987654321, 'F', '87654888', 'Rua B', 456, 'Cidade Beta', 'RJ', 'silveira@email.com', '456456');
CALL InserirFuncionarios  ('Santos', 987654321, 'G', '12345698', 'Rua C', 789, 'Cidade Gamma', 'MG', 'santos@email.com', '789789');
CALL InserirFuncionarios  ('Pereira', 123456789, 'F', '12345678', 'Rua D', 321, 'Cidade Delta', 'RS', 'pereira@email.com', '321321');
CALL InserirFuncionarios ('Costa', 987654321, 'G', '87654321', 'Rua E', 654, 'Cidade Epsilon', 'BA', 'costa@email.com', '654654');
CALL InserirFuncionarios ('Ferreira', 123456789, 'F', '12345678', 'Rua F', 987, 'Cidade Zeta', 'PE', 'ferreira@email.com', '987987');
CALL InserirFuncionarios ('Almeida', 987654321, 'G', '87654321', 'Rua G', 147, 'Cidade Eta', 'SC', 'almeida@email.com', '147147');
CALL InserirFuncionarios   ('Souza', 123456789, 'F', '12345678', 'Rua H', 258, 'Cidade Theta', 'PR', 'souza@email.com', '258258');
CALL InserirFuncionarios  ('Gomes', 987654321, 'G', '87654321', 'Rua I', 369, 'Cidade Iota', 'ES', 'gomes@email.com', '369369');
CALL InserirFuncionarios  ('Carvalho', 123456789, 'F', '12345678', 'Rua J', 159, 'Cidade Kappa', 'DF', 'carvalho@email.com', '159159');


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
CALL InserirPF('Roberto', '1992-01-03', 'M', '1234543898','A', 123358, 12345678908,'12344318', 'Rua bcd', 1322, 'Ouro gtoso', 'MG', 'Rbpeixotojr@gmail.com', 'senha123');
-- drop procedure InserirPJ;
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
	 ELSE
		IF NOT EXISTS(SELECT id_login from tb_login where Email = vEmail) THEN
			INSERT INTO tb_login (email, senha, tipo_login) VALUES (vEmail, vSenha, vTipo_login);
			 SET vId_log = (SELECT id_login from tb_login where email = vEmail);
		else 
			SELECT "email já existe!!";
		END IF;
			-- Insere um novo cliente se ele não existir
		IF NOT EXISTS (SELECT Id_Cliente FROM Tb_Cliente WHERE id_log = vId_log) THEN
			INSERT INTO tb_cliente(Nome,Situacao, Nascimento, Sexo, Telefone, id_log) 
			values(vNome, vSituacao,vNascimento, vSexo, vTelefone, vId_log);  -- Supondo valores padrão para campos ausentes
				SET vId_Cliente = LAST_INSERT_ID();
				-- Insere na Tb_PJ se o CNPJ não existir
				IF NOT EXISTS (SELECT Cnpj FROM Tb_PJ WHERE Cnpj = vCnpj) THEN
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

CALL InserirPJ('Nome', '1990-01-01', 'M', '1234567890', 12345678901234, '123456', 'Nome Fantasia', 'Razao Social', '12345678', 'Rua Exemplo', 123, 'Cidade Exemplo', 'UF', 'exemplo@email.com', 'senha');

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
select * from vw_ClienteEnd;
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
					SET vNF = (SELECT Nf + 1 from Tb_Vendas ORDER BY Nf DESC LIMIT 1);
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
INSERT INTO Tb_Vendas (Nf, ValorTotal, Data_, Id_cli)
                    VALUES (0, 0, NOW(), 1);
call RegistrarVenda (10011,'s411a@example.com', 1, 111232333445);
call RegistrarVenda (0,'s411a@example.com', 1, 0);

select * from Tb_Vendas;
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


call sp_ordenaProduto2('');
call sp_ordenaProduto2( "Por Maior","Valor");
call sp_ordenaProduto2('por maior');

-- drop PROCEDURE sp_ordenaProduto2
DELIMITER //
CREATE PROCEDURE sp_ordenaProduto2(
    IN opc VARCHAR(10),
    In campo VARCHAR(15)
    
)
BEGIN
    IF opc = 'por menor' THEN
    
		  If campo = 'CodBarras'THEN
			SELECT * FROM tb_produto
			ORDER BY CodBarras ASC;
			
		  ELSEIF campo = 'Valor'THEN
			SELECT * FROM tb_produto
			ORDER BY Valor ASC;
			
		  ELSEIF campo = 'NomeProduto'THEN
			SELECT * FROM tb_produto
			ORDER BY Nome ASC;
          ELSE 
			SELECT * FROM tb_produto
			ORDER BY CodBarras ASC;
          END IF;
    ELSEIF opc = 'por maior' THEN
        If campo = 'CodBarras'THEN
			SELECT * FROM tb_produto
			ORDER BY CodBarras DESC;
			
		  ELSEIF campo = 'Valor'THEN
			SELECT * FROM tb_produto
			ORDER BY Valor DESC;
			
		  ELSEIF campo = 'NomeProduto'THEN
			SELECT * FROM tb_produto
			ORDER BY Nome DESC;
          ELSE 
			SELECT * FROM tb_produto
			ORDER BY CodBarras DESC;
          END IF;
    ELSE
        SELECT *
        FROM tb_produto;
    END IF;
END //

DELIMITER ;


drop procedure updFuncEnd;
-- alterar funcionario avisar os erros que der
DELIMITER //

CREATE PROCEDURE updFuncEnd(
    IN vNome_func VARCHAR(200),
    IN vId_func INT,
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
 
    -- Verificar se algum parâmetro é NULL
    IF vNome_func IS NULL OR vId_func IS NULL OR vCep_func IS NULL OR vLogradouro IS NULL OR vNum IS NULL OR vNomeCid IS NULL OR vNomeUF IS NULL OR vEmail IS NULL OR vSenha IS NULL OR vTel IS NULL OR vTipo IS NULL THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Nenhum campo pode ser nulo.';
    END IF;

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
    END IF;
    -- Atualizar o endereço do funcionário
    UPDATE Tb_Funcionario
    SET Cep_Func = vCep_func,
        Nome_Func = vNome_func,
        num = vNum,
        tel = vTel,
        tipo = vTipo
    WHERE id_func = vId_func;
    SELECT 'campos alterados com sucesso !!';
END //

DELIMITER ;

CALL updFuncEnd(
    'Roberto', -- vNome_func
    1, -- vId_func
    '87654878', -- vCep_func
    'a rua 10 ', -- vLogradouro
    50, -- vNum
    'sao paulo', -- vNomeCid
    'sp', -- vNomeUF
    'roberto@.com', -- vEmail
    'junior', -- vSenha
    39015920, -- vTel
    'G' -- vTipo
);
select * from Tb_funcionario;

UPDATE Tb_Produto 
SET Nome = 'eitanbixosex', 
	Descricao = 'amor é viada',
	Marca = 'Nooso',
    ImagemProduto = '/qwewq/qwe.jfif', 
    Valor = 14.90, 
    QtdeST = 3 
WHERE codbarras = 111232333445;

-- ALTERANDO PRODUTO

update Tb_Produto set Nome = @nome, 
    ImagemProduto = @img, 
    Valor = @valor, 
    Qtd = @qtd 
where codbarras=@codbarras;
 /*
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
select * from tb_produto; 
select * from Tb_ItemVenda;
select * from Tb_Vendas;

select * from tb_login;
select * from TB_cliente;
select * from TB_funcionario;
select * from tb_produto;
select * from Tb_ItemVenda;
select * from Tb_Vendas;
SELECT QtdEst - 10 FROM Tb_Produto WHERE CodBarras =111232333445;
show tables;
show tables;*/

/*
UPDATE Tb_Produto 
SET Nome = 'eitanbixosex', 
	Marca = 'Nooso',
    ImagemProduto = '/qwewq/qwe.jfif', 
    Valor = 14.90, 
    QtdeST = 3 
WHERE codbarras = 111232333445;*/
/*ALTERANDO PRODUTO
update Tb_Produto set Nome = @nome, 
    ImagemProduto = @img, 
    Valor = @valor, 
    Qtd = @qtd 
where codbarras=@codbarras;*/
/*DESATIVANDO O CLIENTE
update Tb_Cliente set situacao=@situacao where Id_cliente=@id
-- drop database bdGameFesth;