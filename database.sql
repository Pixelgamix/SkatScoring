DROP TABLE IF EXISTS "skatuser";
CREATE TABLE "public"."skatuser" (
    "userid" uuid NOT NULL,
    "username" character varying(255) NOT NULL,
    "uservalid" boolean DEFAULT true NOT NULL,
    CONSTRAINT "skatuser_userid" PRIMARY KEY ("userid"),
    CONSTRAINT "skatuser_username" UNIQUE ("username")
) WITH (oids = false);
