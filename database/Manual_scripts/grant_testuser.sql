GRANT select, insert, update, delete ON process.register  TO testuser;

GRANT usage ON ALL SEQUENCES IN SCHEMA process TO testuser;

----------------------

commit;