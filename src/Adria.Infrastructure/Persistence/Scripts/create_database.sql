DROP TABLE IF EXISTS completed_games;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS artifacts;
DROP TABLE IF EXISTS games;

create table users (
    id CHAR(36) NOT NULL PRIMARY KEY,
    username VARCHAR(40) NOT NULL UNIQUE,
    INDEX idx_username (username)
);

ALTER TABLE `users`
    ADD CONSTRAINT chk_username_valid
        CHECK (
            CHAR_LENGTH(username) BETWEEN 3 AND 40
                AND username REGEXP '^[a-zA-Z0-9._-]+$'
    );

create table games(
    id CHAR(36) NOT NULL  PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    latitute DOUBLE(7,4) not null,
    longitude DOUBLE(7,4) not null,
    type VARCHAR(50) not null ,
    continent VARCHAR(15) not null,
    year int not null,
    CONSTRAINT chk_latitute CHECK ( latitute BETWEEN -90 AND 90),
    CONSTRAINT chk_longitude CHECK ( longitude BETWEEN -180 AND 180)
);

create table artifacts(
    id CHAR(36) NOT NULL PRIMARY KEY,
    game_id CHAR(36) NOT NULL,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(1000) NOT NULL,
    CONSTRAINT fk_game FOREIGN KEY (game_id) references games(id)
);

create table completed_games(
    user_id CHAR(36) NOT NULL,
    game_id CHAR(36) NOT NULL,
    CONSTRAINT pk_completed_games PRIMARY KEY (user_id, game_id),
    CONSTRAINT fk_completed_games_game_id FOREIGN KEY (game_id) REFERENCES games(id),
    CONSTRAINT fk_completed_games_user_id FOREIGN KEY (user_id) REFERENCES users(id)
);


INSERT INTO `users` (`id`, `username`) VALUES
('550e8400-e29b-41d4-a716-446655440000', 'AliceSmith'),
('550e8400-e29b-41d4-a716-446655440001', 'BobJohnson'),
('550e8400-e29b-41d4-a716-446655440002', 'CharlieBrown'),
('550e8400-e29b-41d4-a716-446655440003', 'DianaTaylor'),
('550e8400-e29b-41d4-a716-446655440004', 'EveAnderson'),
('550e8400-e29b-41d4-a716-446655440005', 'FrankThomas'),
('550e8400-e29b-41d4-a716-446655440006', 'GraceJackson'),
('550e8400-e29b-41d4-a716-446655440007', 'HeidiWhite'),
('550e8400-e29b-41d4-a716-446655440008', 'IvanHarris'),
('550e8400-e29b-41d4-a716-446655440009', 'JudyMartin'),
('550e8400-e29b-41d4-a716-44665544000a', 'KevinThompson'),
('550e8400-e29b-41d4-a716-44665544000b', 'LauraGarcia'),
('550e8400-e29b-41d4-a716-44665544000c', 'MalloryMartinez'),
('550e8400-e29b-41d4-a716-44665544000d', 'NiajRobinson'),
('550e8400-e29b-41d4-a716-44665544000e', 'OliviaClark'),
('550e8400-e29b-41d4-a716-44665544000f', 'PeggyRodriguez'),
('550e8400-e29b-41d4-a716-446655440010', 'QuentinLewis'),
('550e8400-e29b-41d4-a716-446655440011', 'RupertLee'),
('550e8400-e29b-41d4-a716-446655440012', 'SybilWalker'),
('550e8400-e29b-41d4-a716-446655440013', 'TrentHall'),
('550e8400-e29b-41d4-a716-446655440014', 'UmaYoung'),
('550e8400-e29b-41d4-a716-446655440015', 'VictorAllen'),
('550e8400-e29b-41d4-a716-446655440016', 'WendyKing'),
('550e8400-e29b-41d4-a716-446655440017', 'XanderWright'),
('550e8400-e29b-41d4-a716-446655440018', 'YvonneScott'),
('550e8400-e29b-41d4-a716-446655440019', 'ZachAdams'),
('550e8400-e29b-41d4-a716-44665544001a', 'AaronBaker'),
('550e8400-e29b-41d4-a716-44665544001b', 'BethCarter'),
('550e8400-e29b-41d4-a716-44665544001c', 'CameronDiaz'),
('550e8400-e29b-41d4-a716-44665544001d', 'DerekEvans'),
('550e8400-e29b-41d4-a716-44665544001e', 'ElaineFoster'),
('550e8400-e29b-41d4-a716-44665544001f', 'FredGraham'),
('550e8400-e29b-41d4-a716-446655440020', 'GloriaHughes'),
('550e8400-e29b-41d4-a716-446655440021', 'HankIverson'),
('550e8400-e29b-41d4-a716-446655440022', 'IreneJohnson'),
('550e8400-e29b-41d4-a716-446655440023', 'JackKnight'),
('550e8400-e29b-41d4-a716-446655440024', 'KaraLewis'),
('550e8400-e29b-41d4-a716-446655440025', 'LeoMorgan'),
('550e8400-e29b-41d4-a716-446655440026', 'MonaNelson'),
('550e8400-e29b-41d4-a716-446655440027', 'NathanOwen'),
('550e8400-e29b-41d4-a716-446655440028', 'OlgaPerez'),
('550e8400-e29b-41d4-a716-446655440029', 'PaulQuinn'),
('550e8400-e29b-41d4-a716-44665544002a', 'QueenieReed'),
('550e8400-e29b-41d4-a716-44665544002b', 'RobertStone'),
('550e8400-e29b-41d4-a716-44665544002c', 'SamanthaTurner'),
('550e8400-e29b-41d4-a716-44665544002d', 'TomUnderwood'),
('550e8400-e29b-41d4-a716-44665544002e', 'UrsulaVaughn'),
('550e8400-e29b-41d4-a716-44665544002f', 'VictorWhite'),
('550e8400-e29b-41d4-a716-446655440030', 'WandaXavier'),
('550e8400-e29b-41d4-a716-446655440031', 'XenaYoung'),
('550e8400-e29b-41d4-a716-446655440032', 'YusufZimmerman'),
('550e8400-e29b-41d4-a716-446655440033', 'ZaraAllen');

INSERT INTO `games` (`id`, `name`, `latitute`, `longitude`, `type`, `continent`, year)
VALUES
    ('550e8400-e29b-41d4-a716-446655440034', 'Minotaur maze', 35.2989, 25.1636, 'Maze', 'Europe', -1800);
INSERT INTO artifacts (id, game_id, name, description) VALUES
    ('550e8400-e29b-41d4-a716-446655440000', '550e8400-e29b-41d4-a716-446655440034', 'artifact1', 'description1'),
    ('550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440034', 'artifact2', 'description2'),
    ('550e8400-e29b-41d4-a716-446655440002', '550e8400-e29b-41d4-a716-446655440034', 'artifact3', 'description3'),
    ('550e8400-e29b-41d4-a716-446655440003', '550e8400-e29b-41d4-a716-446655440034', 'artifact4', 'description4'),
    ('550e8400-e29b-41d4-a716-446655440004', '550e8400-e29b-41d4-a716-446655440034', 'artifact5', 'description5'),
    ('550e8400-e29b-41d4-a716-446655440005', '550e8400-e29b-41d4-a716-446655440034', 'artifact6', 'description6'),
    ('550e8400-e29b-41d4-a716-446655440006', '550e8400-e29b-41d4-a716-446655440034', 'artifact7', 'description7'),
    ('550e8400-e29b-41d4-a716-446655440007', '550e8400-e29b-41d4-a716-446655440034', 'artifact8', 'description8'),
    ('550e8400-e29b-41d4-a716-446655440008', '550e8400-e29b-41d4-a716-446655440034', 'artifact9', 'description9'),
    ('550e8400-e29b-41d4-a716-446655440009', '550e8400-e29b-41d4-a716-446655440034', 'artifact10', 'description10'),
    ('550e8400-e29b-41d4-a716-44665544000a', '550e8400-e29b-41d4-a716-446655440034', 'artifact11', 'description11'),
    ('550e8400-e29b-41d4-a716-44665544000b', '550e8400-e29b-41d4-a716-446655440034', 'artifact12', 'description12'),
    ('550e8400-e29b-41d4-a716-44665544000c', '550e8400-e29b-41d4-a716-446655440034', 'artifact13', 'description13'),
    ('550e8400-e29b-41d4-a716-44665544000d', '550e8400-e29b-41d4-a716-446655440034', 'artifact14', 'description14'),
    ('550e8400-e29b-41d4-a716-44665544000e', '550e8400-e29b-41d4-a716-446655440034', 'artifact15', 'description15');
