CREATE TABLE IF NOT EXISTS process.register (
	id bigserial NOT NULL,
	"token" text NOT NULL,
	url text NOT NULL,
	name text NOT NULL,
	process_types jsonb NOT NULL,
	CONSTRAINT register_pk PRIMARY KEY (id)
) ;

CREATE UNIQUE INDEX IF NOT EXISTS register_url_name_uidx ON process.register USING btree (name, url) ;