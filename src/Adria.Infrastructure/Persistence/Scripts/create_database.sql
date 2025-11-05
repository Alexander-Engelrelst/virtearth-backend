DROP TABLE IF EXISTS users;

create table users (
    id CHAR(36) NOT NULL PRIMARY KEY ,
    username NVARCHAR(40) NOT NULL UNIQUE,
    avatar NVARCHAR(20),
    INDEX idx_username (username)
);

ALTER TABLE `users`
    ADD CONSTRAINT chk_username_valid
        CHECK (
            CHAR_LENGTH(username) BETWEEN 3 AND 40
                AND username REGEXP '^[a-zA-Z0-9._-]+$'
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
