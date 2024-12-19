CREATE TABLE IF NOT EXISTS users
(
    id            SERIAL PRIMARY KEY,
    username      VARCHAR UNIQUE NOT NULL,
    password_hash VARCHAR        NOT NULL
);

CREATE TABLE IF NOT EXISTS leagues
(
    id              SERIAL PRIMARY KEY,
    name            VARCHAR NOT NULL unique,
    required_rating INT DEFAULT 0
);

CREATE TABLE IF NOT EXISTS lobbies
(
    id                SERIAL PRIMARY KEY,
    league_id         INT REFERENCES leagues (id) on delete cascade,
    max_players_count INT DEFAULT 2
);

CREATE TABLE IF NOT EXISTS user_lobby_points
(
    id       SERIAL PRIMARY KEY,
    user_id  INT REFERENCES users (id) on delete cascade,
    lobby_id INT REFERENCES lobbies (id) on delete set default,
    points   INT DEFAULT 0 NOT NULL
);

create table if not exists roles(
    id serial primary key,
    role varchar not null
);

insert into roles (role) values ('User');

create table if not exists user_roles(
    id serial primary key,
    user_id int references users(id) on delete cascade ,
    role_id int references roles(id) on delete cascade 
);

CREATE TABLE IF NOT EXISTS user_rating
(
    id            SERIAL PRIMARY KEY,
    user_id       INT REFERENCES users (id) on delete cascade,
    rating_points INT DEFAULT 0
);

CREATE TABLE IF NOT EXISTS user_statistics
(
    id            SERIAL PRIMARY KEY,
    user_id       INT REFERENCES users (id) on delete cascade,
    total_hours   INT DEFAULT 0,
    total_sprints INT DEFAULT 0
);

CREATE OR REPLACE FUNCTION user_after_insert() RETURNS TRIGGER AS
$$
BEGIN
    INSERT INTO user_lobby_points (user_id)
    VALUES (NEW.id);
    INSERT INTO user_statistics (user_id)
    VALUES (NEW.id);
    INSERT INTO user_rating (user_id)
    VALUES (NEW.id);
    INSERT INTO user_money (user_id)
    VALUES (NEW.id);
    INSERT INTO user_roles (user_id, role_id)
    VALUES (NEW.id, (select id from roles where role = 'User'));
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_users_insert
    AFTER INSERT
    ON users
    FOR EACH ROW
EXECUTE FUNCTION user_after_insert();

CREATE TABLE IF NOT EXISTS refresh_tokens
(
    id         SERIAL PRIMARY KEY,
    token      VARCHAR NOT NULL,
    is_revoked BOOL    NOT NULL
);

create table if not exists mini_games
(
    id               serial primary key,
    addressable_name varchar not null unique,
    price            int     not null default 0,
    max_level        int     not null default 1
);

create table if not exists users_mini_games
(
    id              serial primary key,
    user_id         int references users (id) on delete cascade ,
    mini_game_id    int references mini_games (id) on delete set default,
    mini_game_level int not null default 1
);

create table if not exists user_money
(
    id      serial primary key,
    user_id int references users (id) on delete cascade,
    amount  int default 0
);

create table if not exists sprints
(
    id serial primary key,
    winner_id int references users(id) on delete cascade,
    lobby_id int references lobbies(id) on delete cascade 
);

CREATE OR REPLACE FUNCTION sprints_after_insert() RETURNS TRIGGER AS
$$
BEGIN
    update user_statistics set total_sprints = total_sprints + 1 where user_id in 
    (select id from user_lobby_points where lobby_id = new.lobby_id); 
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER trigger_sprint_ended
    AFTER INSERT
    ON sprints
    FOR EACH ROW
EXECUTE FUNCTION sprints_after_insert();