-- Create the user 
CREATE USER process WITH PASSWORD 'processuser';
CREATE USER testuser WITH PASSWORD 'test';

--create schema
CREATE SCHEMA if not exists process authorization process;


---------------------
ALTER SCHEMA process OWNER TO process;
GRANT select, insert, update, delete ON ALL TABLES IN SCHEMA process TO process;
GRANT ALL ON ALL SEQUENCES IN SCHEMA process TO process;
GRANT ALL ON ALL FUNCTIONS IN SCHEMA process TO process;
grant ALL on schema process to process;

GRANT usage, select ON ALL SEQUENCES IN SCHEMA process TO testuser;
grant usage on schema process to testuser;



commit;