use apps;
CREATE TABLE usuarios (id INT auto_increment PRIMARY KEY, chat_id text, idioma_selecionado_origem VARCHAR(2) DEFAULT 'en', idioma_selecionado_destino VARCHAR(2) DEFAULT 'pt'), admin boolean not null default 0;

use apps;
show tables;

use apps;
INSERT INTO usuarios (chat_id ) VALUES ('xxxxxxxxxxxxx');

use apps;
SELECT * FROM usuarios;

use apps;
SELECT * FROM usuarios WHERE chat_id = xxxxxxxxxxxxxx;

use apps;
DELETE FROM usuarios WHERE id = 2;
