use seguranca

select * from Sessao where sessaoID = 'c5e65d38-c915-4952-b44d-ed336c1dcfc6'

-- ID 74 usuário condomino
-- ID 68 usuario da adm
select * from usuario where login = 'afbleal@gmail.com'

update Sessao set usuarioId = '74' where sessaoId = '1875c394-d063-46e9-8a7a-4fd6c110772b'

use [dwm-condominios]
select * from InformativoComentario