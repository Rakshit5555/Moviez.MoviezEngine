CREATE TABLE public.movies (
	movie_id int8 NOT NULL,
	movie_name varchar(50) not null,
	date_of_release timestamptz NOT NULL,
	producer_id int8 NULL,
	poster_link text NULL,
	plot varchar(1000) NULL,
	is_active bool NOT null,
	CONSTRAINT movies_pk PRIMARY KEY (movie_id)
);

CREATE TABLE public.producers (
	producer_id int8 NOT NULL,
	producer_name varchar(50) not null,
	date_of_birth date NOT NULL,
	gender_code varchar(15) NULL,
	company varchar(100) NULL,
	bio varchar(1000) null,
	is_active bool NOT null,
	CONSTRAINT producers_pk PRIMARY KEY (producer_id)
);

CREATE TABLE public.actors (
	actor_id int8 NOT NULL,
	actor_name varchar(50) not null,
	date_of_birth timestamptz NOT NULL,
	gender_code varchar(15) NULL,
	bio varchar(1000) null,
	is_active bool NOT null,
	CONSTRAINT actors_pk PRIMARY KEY (actor_id)
);


CREATE TABLE public.Lkp_Genders (
	Gender_Code varchar(15) NOT NULL,
	Gender_Desc varchar(15) NULL,
	is_active bool NOT null,
	CONSTRAINT Lkp_Genders_pk PRIMARY KEY (Gender_Code)
);
INSERT INTO public.lkp_genders
(gender_code, gender_desc, is_active)
VALUES('MALE', 'Male', true);

INSERT INTO public.lkp_genders
(gender_code, gender_desc, is_active)
VALUES('FEMALE', 'Female', true);

INSERT INTO public.lkp_genders
(gender_code, gender_desc, is_active)
VALUES('NOTSPECIFY', 'Not Specify', true);


CREATE TABLE public.castings (
	movie_id int8 NOT NULL,
	actor_id int8 NOT null,
	is_active bool NOT NULL
);

ALTER TABLE public.movies ADD CONSTRAINT movies_producers_fk FOREIGN KEY (producer_id) REFERENCES public.producers(producer_id);
ALTER TABLE public.castings ADD CONSTRAINT castings_actors_fk FOREIGN KEY (actor_id) REFERENCES public.actors(actor_id);
ALTER TABLE public.castings ADD CONSTRAINT castings_movies_fk FOREIGN KEY (movie_id) REFERENCES public.movies(movie_id);

alter table public.actors add constraint actors_lkp_genders_fk foreign key (gender_code) references public.lkp_genders(gender_code);
alter table public.producers  add constraint producers_lkp_genders_fk foreign key (gender_code) references public.lkp_genders(gender_code);


ALTER TABLE public.producers ALTER COLUMN producer_id ADD GENERATED ALWAYS AS IDENTITY;
ALTER TABLE public.movies ALTER COLUMN  movie_id add GENERATED ALWAYS AS IDENTITY;
ALTER TABLE public.actors ALTER COLUMN actor_id add GENERATED ALWAYS AS IDENTITY;

ALTER TABLE public.castings ADD COLUMN cast_id int8 primary key GENERATED ALWAYS AS IDENTITY;

ALTER TABLE public.movies ADD CONSTRAINT movies_name_un UNIQUE (movie_name);