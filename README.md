# Projeto Eleições Goiás - 2022
## Projeto desenvolvido para a disciplina de Banco de Dados II na Faculdade Engenheiro Salvador Arena

Este projeto tem como objetivo mostrar os votos de todas as urnas presentes na eleição de 2022 (federal e estadual) no estado de Goiás.
Dentro da pasta existem duas subpastas:

 - Banco_Backup_Eleicoes_2022_GO: onde está o banco de dados (.bak) ecessário para fazer a consulta;
 - Front-End Eleicoes Goias: onde há um arquivo ASP. NET com o front-end para a consulta;

A importação do banco de dados deve ser feita através da recuperação de banco de dados no SQL Server ou no Management SQL Server. Caso tenha dúvida desse processo, acesse esse link:  [Início Rápido: Backup e restauração de banco de dados local do SQL Server](https://learn.microsoft.com/pt-br/sql/relational-databases/backup-restore/quickstart-backup-restore-database?view=sql-server-ver16)


Para a aplicação Front-End, deve-se utilizar o Visual Studio (de preferência o Visual Studio 2019) a fim de utilizar os recursos de pesquisa e exibição dos dados correspondentes ao banco escolhido.

## Conectando com o Banco de Dados dentro do Front-End

Dentro da pasta "Front-End Eleicoes Goias" existe o arquivo *MasterPage.Master.Cs* e dentro dele há uma variável do tipo string e nome *connectionString* cujo valor deve ser alterado de acordo com a sua conexão particular com o banco de dados. Caso tenha dúvida desse processo, acesse o site: [SqlConnection.ConnectionString Propriedade](https://learn.microsoft.com/pt-br/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring?view=dotnet-plat-ext-7.0)

## Finalizando

Caso tenha alguma dúvida e o processo não esteja funcionando, entre em contato comigo na aba de [Discussions](https://github.com/guigarciag/eleicoes-goias-2022/discussions), ficarei muito feliz em ajudá-lo(a)! 😄

Te vejo na próxima!
