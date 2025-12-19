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
    latitude DOUBLE(7,4) not null,
    longitude DOUBLE(7,4) not null,
    type VARCHAR(50) not null ,
    continent VARCHAR(15) not null,
    year int not null,
    description VARCHAR(1000) NOT NULL DEFAULT '',
    CONSTRAINT chk_latitute CHECK ( latitude BETWEEN -90 AND 90),
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

INSERT INTO `games` (`id`, `name`, latitude, `longitude`, `type`, `continent`, year, description)
VALUES -- yes they are all mazes and it is because I was lazy, thank you
    ('550e8400-e29b-41d4-a716-446655442001',
     'Minotaur maze', 35.2989,
     25.1636, 'Maze',
     'Europe',
     -1800,
     'You can control the player either by using WASD or by using the arrow keys');

INSERT INTO `games` (`id`, `name`, latitude, `longitude`, `type`, `continent`, year)
VALUES  ('550e8400-e29b-41d4-a716-446655442002', 'Viking Quest', 59.9139, 10.7522, 'Maze', 'Europe', 900),
    ('550e8400-e29b-41d4-a716-446655442003', 'Dragon Dynasty', 34.3416, 108.9398, 'Maze', 'Asia', 1200),
    ('550e8400-e29b-41d4-a716-446655442004', 'Samurai Trials', 35.6762, 139.6503, 'Maze', 'Asia', 1600),
    ('550e8400-e29b-41d4-a716-446655442005', 'Pharaoh’s Path', 29.9792, 31.1342, 'Maze', 'Africa', -1500),
    ('550e8400-e29b-41d4-a716-446655442006', 'Savanna Survival', -1.2921, 36.8219, 'Maze', 'Africa', 1800),
    ('550e8400-e29b-41d4-a716-446655442007', 'Aztec Conquest', 19.4326, -99.1332, 'Maze', 'NorthAmerica', 1400),
    ('550e8400-e29b-41d4-a716-446655442008', 'Frontier Legends', 39.7392, -104.9903, 'Maze', 'NorthAmerica', 1850),
    ('550e8400-e29b-41d4-a716-446655442009', 'Inca Empire', -13.5319, -71.9675, 'Maze', 'SouthAmerica', 1450),
    ('550e8400-e29b-41d4-a716-446655442010', 'Amazon Mysteries', -3.4653, -62.2159, 'Maze', 'SouthAmerica', 1700),
    ('550e8400-e29b-41d4-a716-446655442011', 'Dreamtime Journey', -25.2744, 133.7751, 'Maze', 'Oceania', 1000),
    ('550e8400-e29b-41d4-a716-446655442012', 'Pacific Navigators', -17.7134, 178.0650, 'Maze', 'Oceania', 1500);

INSERT INTO artifacts (id, game_id, name, description) VALUES
    ('550e8400-e29b-41d4-a716-446655440000', '550e8400-e29b-41d4-a716-446655442001', 'thread of Ariadne', 'When bound to a wall or doorway, the Thread remembers every step taken thereafter. It pulls gently toward the path of return, tightening when danger draws near and slackening when the way is clear. No matter how deep the maze twists, the Thread will always lead its bearer back to the light.'),
    ('550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655442001', 'Sword Of Theseus', 'An ordinary sword given to Theseus by Ariadne to aid him in his mission to slay the Minotaur. The sword is unnamed in ancient sources and has no magical properties, but it was crucial in defeating the beast within the Labyrinth. It symbolizes Theseus’ courage and the practical support of Ariadne’s guidance.');         
