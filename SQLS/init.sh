/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P SqlServer2019! -d master -i /tmp/Schema.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P SqlServer2019! -d master -i /tmp/Data.sql
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P SqlServer2019! -d master -i /tmp/DataFixes/2021-03-14-06-31_corrige_sigla_estado_roraima.sql