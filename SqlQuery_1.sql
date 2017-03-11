use [seguranca-2017-03-08]

insert into GrupoTransacao(grupoId, transacaoId, situacao)
select 35, transacaoId, 'A'
from GrupoTransacao 
where grupoId = 34
	  and transacaoId not in (981, 982, 985, 986, 1038)
	  and not transacaoId between  987 and 1015
	  and not transacaoId between  1018 and 1025
	  and not transacaoId between  1123 and 1035
	  				

delete from GrupoTransacao
where grupoId = 35


select * From Grupo where grupoId = 35
select * From Transacao where sistemaId=9 order by url
select * From GrupoTransacao where transacaoId=1142