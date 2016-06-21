-- [dwm-condominios]
select * From Edificacao
select * From Condomino order by 1 desc
select * from parc_paradiso..Associado where nome = 'ANDRÉ DE FREITAS BORGES LEAL'
select * from parc_paradiso..Associado where dt_fim is not null
select * from parc_paradiso..Associado where cpf_cnpj is not null
select * From Condomino where CondominoID >= 200
select * From Condomino where email = 'afbleal@gmail.com'
select * From CondominoUnidade where CondominoID >= 200
select * From Parametro
-- Listar unidades desocupadas
select * From Unidade u1 where not Exists(select * From CondominoUnidade u2 where u1.EdificacaoID = u2.EdificacaoID and  u1.UnidadeID = u2.UnidadeID)

update Parametro set Valor = 'S' where paramID = 5 -- habilita envio de e-mail

---------------
--seguranca_db
---------------
use seguranca
select * From seguranca..Transacao where sistemaId = 5 and transacaoId_pai is null order by url
select * From seguranca..Transacao where sistemaId = 5 and exibir = 'S' and transacaoId_pai = 249 order by url 
select * From seguranca..Usuario where empresaId = 3 and login='andreborgesleal@live.com' order by 1 desc

select * From seguranca..Usuario where usuarioId = 15066 order by 1 desc
select * From seguranca..LogAuditoria order by 1 desc
update seguranca..Usuario set senha = '347207DD6E57919082D048294186393F2EE0A5E2' where usuarioId = 68

update Transacao set posicao = 0 where transacaoId = 292

select Count(*) From Unidade



select * From seguranca..Transacao where sistemaId = 5 order by url


use [dwm-condominios]

